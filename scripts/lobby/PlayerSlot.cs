using Godot;
using System;

public partial class PlayerSlot : Node3D
{
	[ExportCategory("PlayerData")]
	[Export] public string PlayerUsername;
	[Export] public int PlayerLevel;
	[Export] public bool IsHost;
	[Export] public bool IsReady;
	[Export] public Label3D UsernameLabel;
	[Export] public Label3D LevelLabel;
	[Export] public Label3D StatusLabel;
	[Export] public bool EmptySlot;

	public override void _Ready()
	{
		// Username
		if (IsHost)
		{
			UsernameLabel.Text = $"{PlayerUsername} | Leader";
			UsernameLabel.FontSize = 36;
		}
		else
		{
			UsernameLabel.Text = $"{PlayerUsername}";
			UsernameLabel.FontSize = 32;
		}
		// Level
		LevelLabel.Text = $" Level {PlayerLevel}";
		// Status
		if (IsReady){
			StatusLabel.Text = "Ready";
			StatusLabel.Modulate = Color.FromHtml("#00ff00");
		}else{
			StatusLabel.Text = "Not Ready";
			StatusLabel.Modulate = Color.FromHtml("#ff0000");
		}


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void UpdateDisplay()
	{
		// Username
		if (!EmptySlot)
		{
			if (IsHost)
			{
				UsernameLabel.Text = $"{PlayerUsername} | Leader";
				UsernameLabel.FontSize = 36;
			}
			else
			{
				UsernameLabel.Text = $"{PlayerUsername}";
				UsernameLabel.FontSize = 32;
			}
			// Level
			LevelLabel.Text = $"Level {PlayerLevel}";
			// Status
			if (IsReady)
			{
				StatusLabel.Text = "Ready";
				StatusLabel.Modulate = Color.FromHtml("#00ff00");
			}
			else
			{
				StatusLabel.Text = "Not Ready";
				StatusLabel.Modulate = Color.FromHtml("#ff0000");
			}
		}else{
			// Hide all labels
			UsernameLabel.Hide();
			LevelLabel.Hide();
			StatusLabel.Hide();
		}
	}
}
