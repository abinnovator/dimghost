using Godot;
using System;

public partial class WelcomeScreen : Node2D
{
	// The button and username input
	[Export] public Button Startbutton;
	[Export] public LineEdit usernameInput;
	[Export] public Button SendUsernameButton;

	private const string LobbyScene = "res://scenes/lobby.tscn";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		usernameInput.Hide();
		SendUsernameButton.Hide();
		Startbutton.Show();

		Startbutton.Pressed += OnStartPressed;
		SendUsernameButton.Pressed += OnSendUsernamePressed;
	}

	private void OnStartPressed()
	{
		// First check if the save exists with the username
		if (SaveManager.SaveExists())
		{
			// Pull the save
			var userSave = SaveManager.LoadSave();
			// Create the server for people to join
			NetworkManager.Instance.Host(userSave, 7777);
			// Teleport to lobby
			SceneManager.Instance.ChangeScene(LobbyScene);

		}
		else
		{
			// Hide the start button
			Startbutton.Hide();
			usernameInput.Show();
			SendUsernameButton.Show();
			usernameInput.GrabFocus();
		}
	}

	private void OnSendUsernamePressed()
	{
		string username = usernameInput.Text.Trim();
		if (username == "") return;

		UserSave newSave = new UserSave { Xp = 0, deaths = 0, username = username, money = 0, wins = 0 };
		SaveManager.Save(newSave);
		SceneManager.Instance.ChangeScene(LobbyScene);
	}
}
