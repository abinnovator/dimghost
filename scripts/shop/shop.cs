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
	[Export] public VBoxContainer cart;
	[Export] public Area2D EquipmentArea;
	[Export] public Label moneyLabel;

	[Export] public PackedScene ShopItemScene;
	[Export] public PackedScene CartShopItem;
	private const float CartRowHeight = 48f;
	public static shop Instance { get; set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		MouseFilter = Control.MouseFilterEnum.Ignore;
		GetNode<ColorRect>("bg").MouseFilter = Control.MouseFilterEnum.Ignore;
		var user = SaveManager.LoadSave();
		moneyLabel.Text = $"¥{user.money.ToString()}";
		// Equipment.MouseFilter = Control.MouseFilterEnum.Stop;
		// Equipment.GuiInput += OnEquipmentGuiInput;

		//  First we have to read all the items
		Dictionary<string, ShopItemType> shopItems = getShopItems();
		//  Then we have to render them with the shjop item in the hbox
		foreach (var (name, item) in shopItems)
		{
			var card = ShopItemScene.Instantiate<ShopItem>();
			ContinerBox.AddChild(card);
			GD.Print(item.ItemSprite);
			// Lets get the amoutn bought
			int AmountBought = NetworkManager.Instance.GetAmountBought(item);
			card.RefreshValues(name, price: item.cost, maxQuantity: item.MaxQuantity, amountBought: AmountBought, assetName: item.ItemSprite, shopItem: item);
		}
		// Debugging for now
		// Print all items bought when the scene is bought up
		GD.Print(NetworkManager.Instance.GetAllBoughtItems());

		// Now lets add the cart menu in the side
		RefreshCart(shopItems);

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
		foreach (var(key,item) in CurrentItemsDictionary){
			item.Name = key;
		}
		return CurrentItemsDictionary;
	}

	public void RefreshCart(Dictionary<string, ShopItemType> shopItems)
	{
		foreach (Node child in cart.GetChildren())
			child.QueueFree();

		var shown = new HashSet<string>();
		int row = 0;
		// First we have to get the bought items from the network manager
		foreach (ShopItemType item in NetworkManager.Instance.GetAllBoughtItems())
		{
			if (!shown.Add(item.Name)) continue;
			string sprite = shopItems.TryGetValue(item.Name, out var data) ? data.ItemSprite : "";

			var card = CartShopItem.Instantiate<ShopCartItem>();
			cart.AddChild(card);
			card.Position = new Vector2(0, row * CartRowHeight);
			int AmountBought = NetworkManager.Instance.GetAmountBought(item);
			card.SetValues(name: item.Name, quantity: AmountBought, assetPath: $"res://assets/shop/{sprite}");
			row++;
		}
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
