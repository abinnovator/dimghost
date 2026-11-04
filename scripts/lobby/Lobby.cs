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
	private bool isShopOpen = false;
	private bool isJoinMenuOpen = false;


	private const string ShopScenePath = "res://scenes/shop.tscn";
	private const string JoinScenePath = "res://scenes/join_menu.tscn";
	private Node _openShop;
	private Node _openJoinScene;

	public override void _Ready()
	{
		GetViewport().PhysicsObjectPicking = true;
		ShopArea.InputEvent += OnShopInput;
		JoinArea.InputEvent += OnJoinInput;

		UserSave host = NetworkManager.Instance.Players[1];
		var index = 0;
		foreach (UserSave player in NetworkManager.Instance.Players.Values)
		{

			PlayerSlots[index].PlayerUsername = player.username;
			PlayerSlots[index].PlayerLevel = player.Xp / 1000 + 1;
			PlayerSlots[index].UpdateDisplay();
			index++;

		}
		for (; index < PlayerSlots.Length; index++)
		{
			PlayerSlots[index].EmptySlot = true;
			PlayerSlots[index].UpdateDisplay();
		}
		// The hosts slot set to leader enabled
		PlayerSlots[0].IsHost = true;

	}

	public override void _ExitTree()
	{
		ShopArea.InputEvent -= OnShopInput;
		JoinArea.InputEvent -= OnJoinInput;

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


	private void OpenShop()
	{
		if (isShopOpen){
			if (IsInstanceValid(_openShop))
				_openShop.QueueFree()
			;else{
				return;
			}
		}
		else{
			isShopOpen = true;
			if (IsInstanceValid(_openShop)) return;
			var shopScene = GD.Load<PackedScene>(ShopScenePath);
			_openShop = shopScene.Instantiate();
			UiLayer.AddChild(_openShop);
		}
	}
	private void OpenJoinMenu()
	{
		if (isJoinMenuOpen)
		{
			if (IsInstanceValid(_openJoinScene))
				_openJoinScene.QueueFree()
			;else{
				return;
			}
		}
		else
		{
			isJoinMenuOpen = true;
			if (IsInstanceValid(_openJoinScene)) return;
			var joinScene = GD.Load<PackedScene>(JoinScenePath);
			_openJoinScene = joinScene.Instantiate();
			UiLayer.AddChild(_openJoinScene);
		}
	}
	// Function for handling setting the location

}
