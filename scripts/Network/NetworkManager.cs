using System;
using System.Collections.Generic;
using System.Linq;
using dimghost;
using Godot;

public partial class NetworkManager : Node
{
    // Set the port
    public const int port = 7777;
    // Set the max number of players. So the host +3.
    public const int maxClients = 3;
    // Create a list for the players
    public Dictionary<long, UserSave> Players = new();
    // Make it be its own instance
    public static NetworkManager Instance { get; private set; }
    // Create a list for shop item bought
    private List<ShopItemType> BoughtItems = new();
    private readonly Dictionary<string, List<long>> BuyersByItem = new();
    private UserSave _localUser;

    // These events let the join menu and lobby react to network changes.
    public event Action<string> ConnectionStatusChanged;
    public event Action PlayersChanged;
    public event Action<string, int> ShopPurchaseChanged;
    // Create a list for chat messages
    private List<ChatMessage> Chatmessages = new();
    private bool _isPurchasingLocally = false;

    // Create a list for the people who are ready
    private Dictionary<long, bool> ReadyStats =  new();

    public override void _EnterTree()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
    }

    public override void _ExitTree()
    {
        Multiplayer.ConnectedToServer -= OnConnectedToServer;
        Multiplayer.ConnectionFailed -= OnConnectionFailed;
        Multiplayer.PeerDisconnected -= OnPeerDisconnected;
        if (Instance == this) Instance = null;
    }

    public bool Host(UserSave save, int hostSelectedPort)
    {
        // Create a new multiplayer peer
        var peer = new ENetMultiplayerPeer();
        // Create server
        Error server = peer.CreateServer(hostSelectedPort, maxClients);
        if (server != Error.Ok)
        {
            GD.PrintErr($"Couldn't host: {server}");
            return false;
        }
        // Add the host to their hosted server
        Multiplayer.MultiplayerPeer = peer;
        Players.Clear();
        Players[1] = save;
        PlayersChanged?.Invoke();
        return true;
    }

    // Join
    public bool Join(string address, UserSave newUser, int serverPort)
    {
        var peer = new ENetMultiplayerPeer();
        // Join server
        Error client = peer.CreateClient(address, serverPort);
        if (client != Error.Ok)
        {
            GD.PrintErr($"Couldn't join: {client}");
            ConnectionStatusChanged?.Invoke($"Could not connect: {client}");
            return false;
        }
        Multiplayer.MultiplayerPeer = peer;
        _localUser = newUser;
        ConnectionStatusChanged?.Invoke("Connecting...");
        return true;
    }

    private void OnConnectedToServer()
    {
        ConnectionStatusChanged?.Invoke("Connected to server.");
        RpcId(1, MethodName.RegisterPlayer, _localUser.username, _localUser.Xp, _localUser.money);
    }

    private void OnConnectionFailed()
    {
        GD.PrintErr("Connection failed (server full or not reachable)");
        ConnectionStatusChanged?.Invoke("Connection failed. Check the server IP and make sure the host is online.");
        Multiplayer.MultiplayerPeer = null;
        Players.Clear();
        PlayersChanged?.Invoke();
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RegisterPlayer(string username, int xp, int money)
    {
        if (!Multiplayer.IsServer()) return;
        long id = Multiplayer.GetRemoteSenderId();

        // Send the current roster to the newly connected player.
        foreach (var (playerId, player) in Players)
            RpcId(id, MethodName.ReceivePlayer, playerId, player.username, player.Xp, player.money);

        // Send the existing shared purchase list to the newly joined player.
        foreach (var (itemName, itemBuyers) in BuyersByItem)
        {
            var item = BoughtItems.Find(boughtItem => boughtItem.Name == itemName);
            if (item == null) continue;
            foreach (long buyerId in itemBuyers)
            {
                if (!Players.TryGetValue(buyerId, out UserSave buyer)) continue;
                RpcId(id, MethodName.BuyShopItem, item.Name, item.cost, item.MaxQuantity,
                    buyerId, buyer.username, buyer.money, true);
            }
        }

        Players[id] = new UserSave { username = username, Xp = xp, money = money };
        GD.Print($"{username} joined as {id}");

        // Update every peer, including the host, with the new player.
        Rpc(MethodName.ReceivePlayer, id, username, xp, money);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void ReceivePlayer(long id, string username, int xp, int money)
    {
        Players[id] = new UserSave { username = username, Xp = xp, money = money };
        PlayersChanged?.Invoke();
    }

    private void OnPeerDisconnected(long id)
    {
        if (!Multiplayer.IsServer() || !Players.ContainsKey(id)) return;
        Players.Remove(id);
        Rpc(MethodName.RemovePlayer, id);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void RemovePlayer(long id)
    {
        Players.Remove(id);
        PlayersChanged?.Invoke();
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void BuyShopItem(string itemName, int cost, int maxQuantity,
        long buyerId = 0, string username = "", int newBalance = -1, bool isBroadcast = false)
    {
        if (isBroadcast)
        {

            bool allowed = Multiplayer.IsServer() ? _isPurchasingLocally : Multiplayer.GetRemoteSenderId() == 1;
            if (!allowed) return;
            if (!BuyersByItem.TryGetValue(itemName, out var buyers))
            {
                buyers = new List<long>();
                BuyersByItem[itemName] = buyers;
            }
            buyers.Add(buyerId);
            BoughtItems.Add(new ShopItemType { Name = itemName, cost = cost, MaxQuantity = maxQuantity });

            if (Players.TryGetValue(buyerId, out UserSave player))
                player.money = newBalance;

            if (Multiplayer.GetUniqueId() == buyerId && _localUser != null)
            {
                _localUser.money = newBalance;
                SaveManager.Save(_localUser);
            }

            ShopPurchaseChanged?.Invoke(itemName, buyers.Count);
            GD.Print($"{username} bought {itemName}; {buyers.Count}/{maxQuantity} purchased.");
            return;
        }

        if (!Multiplayer.IsServer())
        {
            RpcId(1, MethodName.BuyShopItem, itemName, cost, maxQuantity, 0L, "", -1, false);
            return;
        }

        long purchasingPeerId = Multiplayer.GetRemoteSenderId();
        if (purchasingPeerId == 0) purchasingPeerId = 1;
        if (cost < 0 || maxQuantity <= 0 || !Players.TryGetValue(purchasingPeerId, out UserSave buyer))
            return;

        int amountBought = BuyersByItem.TryGetValue(itemName, out var currentBuyers) ? currentBuyers.Count : 0;
        if (amountBought >= maxQuantity || buyer.money < cost)
            return;

        buyer.money -= cost;
        if (purchasingPeerId == 1)
            SaveManager.Save(buyer);

        Rpc(MethodName.BuyShopItem, itemName, cost, maxQuantity,
            purchasingPeerId, buyer.username, buyer.money, true);
        _isPurchasingLocally = true;
        BuyShopItem(itemName, cost, maxQuantity, purchasingPeerId, buyer.username, buyer.money, true);
        _isPurchasingLocally = false;
    }

    // Get all the bought items
    public List<ShopItemType> GetAllBoughtItems()
    {
        return BoughtItems;
    }
    // Get how much of one item was bought
    public int GetAmountBought(ShopItemType item)
    {
        return BuyersByItem.TryGetValue(item.Name, out var buyers) ? buyers.Count : 0;
    }
    // Chat functions
    public List<ChatMessage> GetChatMessages()
    {
        return Chatmessages;
    }
    // Function for updqating across ALL clients
    public void TransmitMessage(ChatMessage message){
        Rpc(MethodName.SendMessage, message.username, message.text);
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    // Function for sending message
    public void SendMessage(string username, string text)
    {
        var message = new ChatMessage { username = username, text = text };
        Chatmessages.Add(message);
        Chat.Instance.RefreshChatMessages(messages: Chatmessages);
    }

    // Function for transmitting player status
    public void TransmitUserStatus(bool status, long id){
        Rpc(MethodName.ChangePlayerStatus, status, id);
    }
    // Function for changing player status
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    public void ChangePlayerStatus(bool status, long id)
    {
        if (Players.ContainsKey(id))
            ReadyStats[id] = status;
    }



}
