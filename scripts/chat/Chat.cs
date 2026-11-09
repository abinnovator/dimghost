using dimghost;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Chat : Node2D
{
	[Export] public ScrollContainer ChatScrollContainer;
	[Export] public VBoxContainer ChatVBOX;
	[Export] public PackedScene MessageScene;
	[Export] public Button SendButton;
	[Export] LineEdit Input;
	private List<ChatMessage> CurrentMessages;
	public static Chat Instance { get; private set; }
	private int messagesShown;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		SendButton.Pressed += SendMessage;
		Input.TextSubmitted += _ => SendMessage();
		CurrentMessages = NetworkManager.Instance.GetChatMessages();


		foreach (var message in CurrentMessages)
		{
			AddMessageToChat(message);
			messagesShown++;
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _ExitTree(){
		if (Instance == this)
			Instance = null;
	}
	// Function for sending message
	public void SendMessage()
	{
		string messagetext = Input.Text;
		var user = SaveManager.LoadSave();
		ChatMessage messageobject = new() { text = messagetext, username = user.username };
		NetworkManager.Instance.TransmitMessage(message: messageobject);
		Input.Text = "";
	}
	public void RefreshChatMessages(List<ChatMessage> messages)
	{
		var newMessagesCount = messages.Count - messagesShown;
		var newMessages = new List<ChatMessage>();

		for (int i = 0; i < newMessagesCount; i++)
		{
			newMessages.Add(messages[messagesShown + i]);
		}

		messagesShown += newMessagesCount;

		foreach (var message in newMessages)
			AddMessageToChat(message);

		CurrentMessages = messages;
	}

	private void AddMessageToChat(ChatMessage chatMessage)
	{
		GD.Print($"Adding: {chatMessage.text}");
		var messageNode = MessageScene.Instantiate<MessageContainer>();
		messageNode.usernamestr = chatMessage.username;
		messageNode.messagestr = chatMessage.text;
		ChatVBOX.AddChild(messageNode);
		GD.Print($"VBox children now: {ChatVBOX.GetChildCount()}");
	}


}
