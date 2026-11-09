using dimghost;
using Godot;
using System;

public partial class ShopItem : Control
{
	[ExportCategory("Node Slots")]
	[Export] public Label NameLabel;
	[Export] public Label MoneyLabel;
	[Export] public Label MaxQuantity;
	[Export] public string Name;
	[Export] public Sprite2D ItemIcon;
	public ShopItemType ShopItemObj;
	[ExportCategory("Buttons")]
	[Export] public Button IncreaseQuantityButton;
	[Export] public Button DecreaseQuantityButton;


	private string assetPath = "res://assets/shop";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IncreaseQuantityButton.Pressed += BuyItem;
		NetworkManager.Instance.ShopPurchaseChanged += OnShopPurchaseChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _ExitTree()
	{
		IncreaseQuantityButton.Pressed -= BuyItem;
		if (NetworkManager.Instance != null)
			NetworkManager.Instance.ShopPurchaseChanged -= OnShopPurchaseChanged;
	}
	// Make a custom to handle buying of items
	public void BuyItem(){
		if (ShopItemObj != null)
			NetworkManager.Instance.BuyShopItem(ShopItemObj.Name, ShopItemObj.cost, ShopItemObj.MaxQuantity);


	}

	private void OnShopPurchaseChanged(string itemName, int amountBought)
	{
		if (ShopItemObj == null || ShopItemObj.Name != itemName) return;
		RefreshValues(Name, ShopItemObj.cost, ShopItemObj.MaxQuantity, amountBought,
			ShopItemObj.ItemSprite, ShopItemObj);
		var shopItems = shop.Instance.getShopItems();
		shop.Instance.RefreshCart(shopItems);
	}
	//  Add a functionm to refresh the values
	public void RefreshValues(string name, int price, int maxQuantity, int amountBought, string assetName, ShopItemType shopItem) {
		Name = name;
		NameLabel.Text = name;
		MoneyLabel.Text = $"¥{price.ToString()}";
		MaxQuantity.Text = $"{amountBought}/{maxQuantity}";
		Texture2D ItemTexture = GD.Load<Texture2D>($"{assetPath}/{assetName}");
		ItemIcon.Texture = ItemTexture;
		ShopItemObj = shopItem;
		IncreaseQuantityButton.Disabled = amountBought >= maxQuantity;
		if (amountBought == 0)
		{
			DecreaseQuantityButton.Disabled = true;
		}
		else
		{
			DecreaseQuantityButton.Disabled = false;
		}


	}
}
