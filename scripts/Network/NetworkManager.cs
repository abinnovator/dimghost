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
    // Make it b e its own instance
    public static NetworkManager Instance { get; private set; }
    // Create a list for shop item bought
    public Dictionary<long, BoughtShopItem> BoughtItems;
    private UserSave _localUser;

    public override void _EnterTree(){
        Instance = this;
    }

    public override void _Ready(){
        Multiplayer.ConnectedToServer += OnConnectedToServer;
        Multiplayer.ConnectionFailed += OnConnectionFailed;
    }

    public bool Host(UserSave save, int hostSelectedPort)
    {
        // Create a new multiplayer peer
        var peer = new ENetMultiplayerPeer();
        //  Create server
        Error server = peer.CreateServer(hostSelectedPort, maxClients);
        if (server != Error.Ok)
        {
            GD.PrintErr($"Couldn't host: {server}");
            return false;
        }
        // Add the host to their hosted server
        Multiplayer.MultiplayerPeer = peer;
        Players[1] = save;
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
            return false;
        }
        Multiplayer.MultiplayerPeer = peer;
        _localUser = newUser;
        // If everything went fine return true
        return true;
    }

    private void OnConnectedToServer(){
        RpcId(1, MethodName.RegisterPlayer, _localUser.username, _localUser.Xp);
    }

    private void OnConnectionFailed(){
        GD.PrintErr("Connection failed (server full or not reachable)");
        Multiplayer.MultiplayerPeer = null;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void RegisterPlayer(string username, int xp){
        if (!Multiplayer.IsServer()) return;
        long id = Multiplayer.GetRemoteSenderId();
        Players[id] = new UserSave { username = username, Xp = xp };
        GD.Print($"{username} joined as {id}");
    }
}
