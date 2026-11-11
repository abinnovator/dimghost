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
	[Export] public string LocationScenePath;

   	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void RefreshValues(string name, string size, string LocationImagePath, string ScenePath)
	{
		LocationName.Text = name;
		LocationSize.Text = size;
		Texture2D picture = GD.Load<Texture2D>(LocationImagePath);
		LocationImage.Texture = picture;
		LocationScenePath = ScenePath;
	}
	public void CardOnClick(){
		NetworkManager.Instance.TransmitSetLocation(LocationName.Text, LocationScenePath);
	}
}
