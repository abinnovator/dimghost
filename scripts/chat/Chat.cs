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
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        SendButton.Pressed += SendMessage;
        Input.TextSubmitted += _ => SendMessage();
        CurrentMessages = NetworkManager.Instance.GetChatMessages();


        foreach (var message in CurrentMessages)
                AddMessageToChat(message);

	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    // Function for sending message
    public void SendMessage()
    {
        string messagetext = Input.Text;
        var user = SaveManager.LoadSave();
        ChatMessage messageobject = new() { text = messagetext, username = user.username };
        NetworkManager.Instance.SendMessage(message: messageobject);
    }
    public void RefreshChatMessages(List<ChatMessage> messages)
    {
        // First we will check for new messages
        var newMessages = messages.Except(CurrentMessages).ToList();
        foreach (var message in newMessages)
                AddMessageToChat(message);

            CurrentMessages = messages;

    }
    private void AddMessageToChat(ChatMessage message)
    {
        var messageNode = MessageScene.Instantiate();
        ChatVBOX.AddChild(messageNode);

        messageNode.GetNode<Label>("UsernameLabel").Text = message.username;
        messageNode.GetNode<Label>("MessageLabel").Text = message.text;
    }


}
