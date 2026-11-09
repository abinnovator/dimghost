using Godot;
using System;

public partial class Profile : Node2D
{
	[Export] public Label Username;
	[Export] public Label Level;
	[Export] public Label Position;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		UserSave save = SaveManager.LoadSave();
		Username.Text = save.username;
		string level = (save.Xp / 1000 + 1).ToString();
		Level.Text = $"Level:{level}";
		string position = "";
		if (level.ToInt() < 20){
			Position.Text = "Rookie";
		}else {
			Position.Text = "Pro";
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
