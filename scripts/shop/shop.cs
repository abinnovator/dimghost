using dimghost;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public partial class shop : ColorRect
{
	[ExportCategory("Menu Slots")]
	[Export] public Label Ready;
	[Export] public Label Join;
	[Export] public Label Equipment;
	[Export] public Label Location;
	[Export] public Label Profile;
	[Export] public ScrollContainer ShopItemsContainer;
	[Export] public HBoxContainer ContinerBox;
	[Export] public Area2D EquipmentArea;

	[Export] public PackedScene ShopItemScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	Equipment.MouseFilter = Control.MouseFilterEnum.Stop;
	Equipment.GuiInput += OnEquipmentGuiInput;

		//  First we have to read all the items
		Dictionary<string, ShopItemType> shopItems = getShopItems();
		//  Then we have to render them with the shjop item in the hbox
		foreach (var (name, item) in shopItems){
			var card = ShopItemScene.Instantiate<ShopItem>();
			ContinerBox.AddChild(card);
			// Lets get the amoutn bought
			int AmountBought = NetworkManager.Instance.GetAmountBought(item);
			card.RefreshValues(name, price: item.cost, maxQuantity: item.MaxQuantity, amountBought:AmountBought , assetName: item.ItemSprite, shopItem:item);
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public Dictionary<string, ShopItemType> getShopItems()
	{
		// Read all the items i put in the json file
		using var file = Godot.FileAccess.Open("res://datasets/shop.json", Godot.FileAccess.ModeFlags.Read);
		if (file == null) { GD.PrintErr(Godot.FileAccess.GetOpenError()); return new(); }
		string currentItems = file.GetAsText();
		// Turn it into a object/distionary
		Dictionary<string, ShopItemType> CurrentItemsDictionary = JsonSerializer.Deserialize<Dictionary<string, ShopItemType>>(currentItems);
		return CurrentItemsDictionary;
	}

	private void OnEquipmentGuiInput(InputEvent @event)
	{
	if (@event is InputEventMouseButton mouse &&
		mouse.Pressed &&
		mouse.ButtonIndex == MouseButton.Left)
	{
		QueueFree();
	}
	}
}
