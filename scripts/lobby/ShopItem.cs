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
	[ExportCategory("Slot values")]
	[Export] public int price;
	[Export] public int maxQuantity;
	[Export] public int amountBought;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
