using Godot;
using System;

public partial class ShopCartItem : Node2D
{
	[Export] public Label Name;
	[Export] public Label Quantity;
	[Export] public Sprite2D Picture;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void SetValues(string name, int quantity, string assetPath){
		Name.Text = name;
		Quantity.Text = $"×{quantity.ToString()}";
		Texture2D ItemTexture = GD.Load<Texture2D>(assetPath);

		Picture.Texture = ItemTexture;
		Picture.Scale = new Vector2(0.04f, 0.04f);

	}
}
