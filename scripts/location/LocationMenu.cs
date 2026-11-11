using dimghost;
using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class LocationMenu : Node2D
{
	[Export] public ScrollContainer containerBox;
	[Export] public HBoxContainer HBox;
	[Export] public PackedScene LocationCard;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Render all the cards
		var locations = GetLocations();
		GD.Print(locations);
		foreach (var location in locations){
			var card = LocationCard.Instantiate<LocationCard>();
			HBox.AddChild(card);

			card.RefreshValues(name: location.Value.Name, size: location.Value.size, LocationImagePath: location.Value.image, ScenePath: location.Value.scenePath);
		}

	}
	public Dictionary<string, LocationType> GetLocations(){
		using var file = Godot.FileAccess.Open("res://datasets/locations.json", Godot.FileAccess.ModeFlags.Read);

		if (file == null) { GD.PrintErr(Godot.FileAccess.GetOpenError()); return new(); }
		string currentLocations = file.GetAsText();
		// Turn it into a object/distionary
		Dictionary<string, LocationType> CurrentLocationsDictionary = JsonSerializer.Deserialize<Dictionary<string, LocationType>>(currentLocations);
		if (CurrentLocationsDictionary == null) return new();
		foreach (var(key,location) in CurrentLocationsDictionary){
			location.Name = key;
		}
		return CurrentLocationsDictionary;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
