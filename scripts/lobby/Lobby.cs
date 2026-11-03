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
	[Export] public CanvasLayer UiLayer;

	private const string ShopScenePath = "res://scenes/shop.tscn";
	private Node _openShop;

	public override void _Ready()
	{
		GetViewport().PhysicsObjectPicking = true;
		ShopArea.InputEvent += OnShopInput;

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
	}

	private void OnShopInput(Node camera, InputEvent @event, Vector3 position, Vector3 normal, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouse && mouse.Pressed && mouse.ButtonIndex == MouseButton.Left)
		{
			OpenShop();
		}
	}

	private void OpenShop()
	{
		if (IsInstanceValid(_openShop)) return;
		var shopScene = GD.Load<PackedScene>(ShopScenePath);
		_openShop = shopScene.Instantiate();
		UiLayer.AddChild(_openShop);
	}
	// Function for handling setting the location

}
