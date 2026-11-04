using Godot;
using System;

public partial class JoinMenu : Control
{
	[Export] public Button ConnectButton;
	[Export] public LineEdit IpInput;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConnectButton.Pressed += JoinGame;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void JoinGame (){
		var userSave = SaveManager.LoadSave();
		NetworkManager.Instance.Join(serverPort: IpInput.Text.ToInt(), newUser: userSave, address: Addreses.getCurrentIp());

	}
}
