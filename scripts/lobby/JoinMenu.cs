using Godot;

public partial class JoinMenu : Control
{
	[Export] public Button ConnectButton;
	[Export] public LineEdit IpInput;
	[Export] public Label StatusLabel;

	public override void _Ready()
	{
		ConnectButton.Pressed += JoinGame;
		NetworkManager.Instance.ConnectionStatusChanged += OnConnectionStatusChanged;
	}

	public override void _ExitTree()
	{
		if (NetworkManager.Instance != null)
			NetworkManager.Instance.ConnectionStatusChanged -= OnConnectionStatusChanged;
	}

	private void JoinGame()
	{
		string address = IpInput.Text.Trim();
		if (string.IsNullOrWhiteSpace(address))
		{
			StatusLabel.Text = "Enter the server IP address.";
			return;
		}

		ConnectButton.Disabled = true;
		StatusLabel.Text = "Connecting...";

		var userSave = SaveManager.LoadSave();
		if (!NetworkManager.Instance.Join(address, userSave, NetworkManager.port))
		{
			StatusLabel.Text = "Could not start connection. Check the IP and try again.";
			ConnectButton.Disabled = false;
		}
	}

	private void OnConnectionStatusChanged(string message)
	{
		StatusLabel.Text = message;
		if (message != "Connecting...")
			ConnectButton.Disabled = false;
	}
}
