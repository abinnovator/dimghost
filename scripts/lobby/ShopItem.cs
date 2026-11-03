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

	private string assetPath = "res://assets/shop";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	//  Add a functionm to refresh the values
	public void RefreshValues(string name, int price, int maxQuantity, int amountBought, string assetName) {
		NameLabel.Text = Name;
		MoneyLabel.Text = price.ToString();
		MaxQuantity.Text = $"{amountBought}/{maxQuantity.ToString()}";
		Texture2D ItemTexture = GD.Load<Texture2D>("{assetPath}/{assetName}");
		ItemIcon.Texture = ItemTexture;

	}
}
