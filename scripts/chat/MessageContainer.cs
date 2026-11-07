using Godot;
using System;

public partial class MessageContainer : Control
{
	[Export] public Label username;
	[Export] public Label message;
	[Export] public string messagestr;
	[Export] public string usernamestr;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (username == null || message == null)
		{
			GD.PushError("Assign the username and message Labels in the MessageContainer Inspector.");
			return;
		}

		username.Text = usernamestr;
		message.Text = messagestr;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
