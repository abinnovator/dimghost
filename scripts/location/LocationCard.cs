using dimghost;
using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class LocationCard : Node2D
{
	[Export] public Label LocationName;
	[Export] public Label LocationSize;
	[Export] public Sprite2D LocationImage;
	[Export] public Area2D ClickableArea;
	[Export] public PanelContainer Card;
	[Export] public string LocationScenePath;
	[Export] public bool isSelected;


   	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
			if (ClickableArea != null)
			{
				ClickableArea.InputEvent += OnClickableAreaInputEvent;
			}


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public override void _ExitTree(){
		ClickableArea.InputEvent -= OnClickableAreaInputEvent;
	}


	public void RefreshValues(string name, string size, string LocationImagePath, string ScenePath)
	{
		LocationName.Text = name;
		LocationSize.Text = size;
		Texture2D picture = GD.Load<Texture2D>(LocationImagePath);
		LocationImage.Texture = picture;
		LocationScenePath = ScenePath;
	}
	private void OnClickableAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				CardOnClick();
			}
		}
	}
	public void CardOnClick(){
		isSelected = !isSelected;
		if (isSelected)
		{
			NetworkManager.Instance.TransmitSetLocation(LocationName.Text, LocationScenePath);
			var styleBox = Card.GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;

			if (styleBox != null)
			{
				styleBox.BorderColor = Colors.Green;
				styleBox.SetBorderWidthAll(2);
				Card.AddThemeStyleboxOverride("panel", styleBox);
			}

		}
		else
		{
			NetworkManager.Instance.TransmitSetLocation("", "");
			var styleBox = Card.GetThemeStylebox("panel").Duplicate() as StyleBoxFlat;

			if (styleBox != null)
			{
						 styleBox.BorderColor = Colors.Green;
						 styleBox.SetBorderWidthAll(0);
						 Card.AddThemeStyleboxOverride("panel", styleBox);
			}
		}
	}
}
