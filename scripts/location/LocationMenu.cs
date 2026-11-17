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
	[Export] public Label Easy;
	[Export] public Label Normal;
	[Export] public Label Hard;
	[Export] public Label Nightmare;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConnectDifficultyButton(Easy, OnEasyInput);
		ConnectDifficultyButton(Normal, OnNormalInput);
		ConnectDifficultyButton(Hard, OnHardInput);
		ConnectDifficultyButton(Nightmare, OnNightmareInput);

		// Render all the cards
		var locations = GetLocations();
		GD.Print(locations);
		foreach (var location in locations){
			var card = LocationCard.Instantiate<LocationCard>();
			HBox.AddChild(card);

			card.RefreshValues(name: location.Value.Name, size: location.Value.size, LocationImagePath: location.Value.image, ScenePath: location.Value.scenePath);
		}

	}
	public override void _ExitTree()
	{
		DisconnectDifficultyButton(Easy, OnEasyInput);
		DisconnectDifficultyButton(Normal, OnNormalInput);
		DisconnectDifficultyButton(Hard, OnHardInput);
		DisconnectDifficultyButton(Nightmare, OnNightmareInput);
	}

	private void ConnectDifficultyButton(Label button, Control.GuiInputEventHandler handler)
	{
		if (button == null) return;
		button.MouseFilter = Control.MouseFilterEnum.Stop;
		button.GuiInput += handler;
	}

	private void DisconnectDifficultyButton(Label button, Control.GuiInputEventHandler handler)
	{
		if (button == null) return;
		button.GuiInput -= handler;
	}

	private void OnEasyInput(InputEvent @event) => OnDifficultyButtonInput(@event, "easy");
	private void OnNormalInput(InputEvent @event) => OnDifficultyButtonInput(@event, "normal");
	private void OnHardInput(InputEvent @event) => OnDifficultyButtonInput(@event, "hard");
	private void OnNightmareInput(InputEvent @event) => OnDifficultyButtonInput(@event, "nightmare");

	private void OnDifficultyButtonInput(InputEvent @event, string difficulty)
	{
		if (@event is InputEventMouseButton mouse &&
			mouse.Pressed &&
			mouse.ButtonIndex == MouseButton.Left)
		{
			NetworkManager.Instance.TransmitSetDifficulty(difficulty);
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
