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
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Take the randomly chosen ghost from network manager
		ghost = NetworkManager.Instance.ghost;
		// Take the difficulty from network manager
		difficulty = NetworkManager.Instance.level;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
