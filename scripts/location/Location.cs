using dimghost;
using Godot;
using System;

public partial class Location : Node3D
{
	[ExportCategory("variables")]
	[Export] public Ghost ghost;
	[Export] public Level difficulty;
	[Export] public bool hunting = false;
	[ExportCategory("Nodes")]
	[Export] public NavigationRegion3D NavigationArea;
	[Export] public MultiplayerSpawner PlayerSpawner;
	[Export] public MultiplayerSynchronizer Synchronizer;
	[Export] public Node3D PlayersNode;
	[Export] public Marker3D[] SpawnPoints;
	[Export] public PackedScene Player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Take the randomly chosen ghost from network manager
		ghost = NetworkManager.Instance.ghost;
		// Take the difficulty from network manager
		difficulty = NetworkManager.Instance.level;
		if (IsMultiplayerAuthority()){
			SpawnPlayers();
		}


	}
	// Create a function for spawning players
	// CAUTION: ONLY TO BE RUN ON THE SERVER NEVRWEJKJENFJN ON THE CLIENT
	public void SpawnPlayers(){
		if (!Multiplayer.IsServer()) return;
		int i = 0;
		foreach (long id in NetworkManager.Instance.Players.Keys){
			var player = Player.Instantiate<Player>();
			player.Name = id.ToString();
			player.Position = SpawnPoints[i].GlobalPosition;
			PlayersNode.AddChild(player);
			i++;
		}

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
