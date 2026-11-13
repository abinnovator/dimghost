using Godot;
using System;

public partial class LobbyPlayer : Node3D
{
	[Export] public string position;
	[Export] public AnimationPlayer AnimationPlayer;
	public LobbyPlayer Instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		if (position == "scared")
		{
			AnimationPlayer.Play("mixamo_com");
		}
		else if (position == "idle"){
			AnimationPlayer.Play("breathing_idle");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void RefreshAnimation(string name){
		if (position == "scared")
		{
			AnimationPlayer.Play("mixamo_com");
		}
		else if (position == "idle"){
			AnimationPlayer.Play("breathing_idle");
		}
	}
}
