using Godot;
using System.Collections.Generic;
using System;
public partial class Lobby : Node3D
{
	[ExportCategory("PlayerSlots")]
	[Export] public PlayerSlot[] PlayerSlots;
	[Export] public PlayerSlot PlayerSlot1;
	[Export] public PlayerSlot PlayerSlot2;
	[Export] public PlayerSlot PlayerSlot3;
	[Export] public PlayerSlot PlayerSlot4;
	[ExportCategory("MenuButtons")]
	[Export] public Label3D Shop;
	[Export] public Label3D Equipment;
	[Export] public Label3D ReadyButton;
	[Export] public Label3D Join;
	[Export] public Label3D Location;
	[Export] public Area3D ShopArea;
	[Export] public Area3D JoinArea;
	[Export] public CanvasLayer UiLayer;
	[Export] public bool isShopOpen = false;
	private bool isJoinMenuOpen = false;


	private const string MenuScenePath = "res://scenes/MenuBar.tscn";
	private const string ShopScenePath = "res://scenes/shop.tscn";
	private const string JoinScenePath = "res://scenes/join_menu.tscn";
	private const string ChatScenePath = "res://scenes/Chat.tscn";
	private Node _Menu;
	private Node _openShop;
	private Node _openJoinScene;
	private Node _Chat;


	public override void _Ready()
	{
		GetViewport().PhysicsObjectPicking = true;
		ShopArea.InputEvent += OnShopInput;
		JoinArea.InputEvent += OnJoinInput;

		NetworkManager.Instance.PlayersChanged += RefreshPlayerSlots;
		RefreshPlayerSlots();
		LoadMenu();
		LoadChat();

	}

	public override void _ExitTree()
	{
		ShopArea.InputEvent -= OnShopInput;
		JoinArea.InputEvent -= OnJoinInput;
		if (NetworkManager.Instance != null)
			NetworkManager.Instance.PlayersChanged -= RefreshPlayerSlots;

	}

	private void OnShopInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left)
		{
			OpenShop();
		}
	}
	private void OnJoinInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx){
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left)
		{
			OpenJoinMenu();
		}
	}
	private void LoadMenu(){
		var menuScene = GD.Load<PackedScene>(MenuScenePath);
		if (menuScene == null)
		{
			GD.PrintErr($"Could not load shop scene: {MenuScenePath}");
			return;
		}

		_Menu = menuScene.Instantiate();
		UiLayer.AddChild(_Menu);
	}

	private void OpenShop()
	{
		if (IsInstanceValid(_openShop))
		{
			_openShop.QueueFree();
			_openShop = null;
			isShopOpen = false;
			return;
		}

		var shopScene = GD.Load<PackedScene>(ShopScenePath);
		if (shopScene == null)
		{
			GD.PrintErr($"Could not load shop scene: {ShopScenePath}");
			return;
		}

		_openShop = shopScene.Instantiate();
		UiLayer.AddChild(_openShop);
		isShopOpen = true;
	}
	private void OpenJoinMenu()
	{
		if (IsInstanceValid(_openJoinScene))
		{
			_openJoinScene.QueueFree();
			_openJoinScene = null;
			isJoinMenuOpen = false;
			return;
		}

		var joinScene = GD.Load<PackedScene>(JoinScenePath);
		if (joinScene == null)
		{
			GD.PrintErr($"Could not load join menu: {JoinScenePath}");
			return;
		}

		_openJoinScene = joinScene.Instantiate();
		UiLayer.AddChild(_openJoinScene);
		isJoinMenuOpen = true;
	}
	private void LoadChat(){
		var ChatScene = GD.Load<PackedScene>(ChatScenePath);
		_Chat = ChatScene.Instantiate();
		UiLayer.AddChild(_Chat);

	}
	public void RefreshPlayerSlots()
	{
		if (PlayerSlots == null || NetworkManager.Instance == null) return;

		var players = new List<KeyValuePair<long, UserSave>>(NetworkManager.Instance.Players);
		players.Sort((a, b) => a.Key.CompareTo(b.Key));
		for (int i = 0; i < PlayerSlots.Length; i++)
		{
			PlayerSlot slot = PlayerSlots[i];
			if (i < players.Count)
			{
				var player = players[i];
				slot.EmptySlot = false;
				slot.IsHost = player.Key == 1;
				slot.PlayerUsername = player.Value.username;
				slot.PlayerLevel = player.Value.Xp / 1000 + 1;
			}
			else
			{
				slot.EmptySlot = true;
				slot.IsHost = false;
				slot.PlayerUsername = "";
				slot.PlayerLevel = 0;
			}
			slot.UpdateDisplay();
		}
	}
	// Function for handling setting the location

}
