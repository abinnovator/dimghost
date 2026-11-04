using System;
using System.Collections.Generic;
using dimghost;
using Godot;

public partial class NetworkManager: Node{
    // Set the port
    public const int port = 7777;
    // Set the max number of players. So the host +3.
    public const int maxClients = 3;
    // Create a list for the players
    public Dictionary<long, UserSave> Players = new();
    // Make it be its own instance
    public static NetworkManager Instance { get; private set; }
    // Create a list for shop item bought
    public Dictionary<long, BoughtShopItem> BoughtItems;
    private UserSave _localUser;

    // These events let the join menu and lobby react to network changes.
    public event Action<string> ConnectionStatusChanged;
    public event Action PlayersChanged;

    public override void _EnterTree(){
        Instance = this;
    }

    public override void _Ready(){
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
    }

    public override void _ExitTree(){
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
    public bool Join(string address, UserSave newUser, int serverPort){
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

    private void OnConnectedToServer(){
        ConnectionStatusChanged?.Invoke("Connected to server.");
        RpcId(1, MethodName.RegisterPlayer, _localUser.username, _localUser.Xp);
    }

    private void OnConnectionFailed(){
        GD.PrintErr("Connection failed (server full or not reachable)");
        ConnectionStatusChanged?.Invoke("Connection failed. Check the server IP and make sure the host is online.");
        Multiplayer.MultiplayerPeer = null;
        Players.Clear();
        PlayersChanged?.Invoke();
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RegisterPlayer(string username, int xp){
        if (!Multiplayer.IsServer()) return;
        long id = Multiplayer.GetRemoteSenderId();

        // Send the current roster to the newly connected player.
        foreach (var (playerId, player) in Players)
            RpcId(id, MethodName.ReceivePlayer, playerId, player.username, player.Xp);

        Players[id] = new UserSave { username = username, Xp = xp };
        GD.Print($"{username} joined as {id}");

        // Update every peer, including the host, with the new player.
        Rpc(MethodName.ReceivePlayer, id, username, xp);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void ReceivePlayer(long id, string username, int xp){
        Players[id] = new UserSave { username = username, Xp = xp };
        PlayersChanged?.Invoke();
    }

    private void OnPeerDisconnected(long id){
        if (!Multiplayer.IsServer() || !Players.ContainsKey(id)) return;
        Players.Remove(id);
        Rpc(MethodName.RemovePlayer, id);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable, CallLocal = true)]
    private void RemovePlayer(long id){
        Players.Remove(id);
        PlayersChanged?.Invoke();
    }
}
