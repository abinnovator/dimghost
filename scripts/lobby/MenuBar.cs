using Godot;
using System;

public partial class MenuBar : Node2D
{
	[ExportCategory("Node Slots")]
	// [Export] public Area2D ReadyArea;
	// [Export] public Area2D EquipmentArea;
	// [Export] public Area2D LocationArea;
	// [Export] public Area2D ProfileArea;
	// [Export] public Area2D JoinArea;
	[Export] public Label ReadyLabel;
	[Export] public Label EquipmentLabel;
	[Export] public Label LocationLabel;
	[Export] public Label ProfileLabel;
	[Export] public Label JoinLabel;
	[Export] public CanvasLayer UiLayer;
	[ExportCategory("Variables")]
	[Export] public bool EquipmentOpen;
	[Export] public bool LocationOpen;
	[Export] public bool ProfileOpen;
	[Export] public bool JoinOpen;
	private Node _openShop;
	private Node _openJoinScene;
	private const string ShopScenePath = "res://scenes/shop.tscn";
	private const string JoinScenePath = "res://scenes/join_menu.tscn";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		JoinLabel.MouseFilter = Control.MouseFilterEnum.Stop;
		JoinLabel.GuiInput += ToggleJoinMenu;
		EquipmentLabel.MouseFilter = Control.MouseFilterEnum.Stop;
		EquipmentLabel.GuiInput += ToggleShop;

	}
	public override void _ExitTree()
	{
		JoinLabel.GuiInput -= ToggleJoinMenu;
		EquipmentLabel.GuiInput -= ToggleShop;


	}
	// Todo: Add ready/unready toggle feature
	public void ToggleStatus(InputEvent @event){
		if (@event is InputEventMouseButton mouse &&
				  mouse.Pressed &&
				  mouse.ButtonIndex == MouseButton.Left)
		{ }
	}
	// Join Menu Open
	public void ToggleJoinMenu(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouse &&
			   mouse.Pressed &&
			   mouse.ButtonIndex == MouseButton.Left)
		{
			if (IsInstanceValid(_openJoinScene))
			{
				_openJoinScene.QueueFree();
				_openJoinScene = null;
				JoinOpen = false;
				JoinLabel.LabelSettings.FontColor = Colors.White;
				return;
			}

			var joinScene = GD.Load<PackedScene>(JoinScenePath);
			if (joinScene == null)
			{
				GD.PrintErr($"Could not load join menu: {JoinScenePath}");
				return;
			}

			_openJoinScene = joinScene.Instantiate();
			UiLayer.AddChild(_openJoinScene);
			JoinOpen = true;
			JoinLabel.LabelSettings.FontColor = Colors.Green;

		}
	}
	public void ToggleShop(InputEvent @event){
		if (@event is InputEventMouseButton mouse &&
			   mouse.Pressed &&
			   mouse.ButtonIndex == MouseButton.Left)
		{
			if (IsInstanceValid(_openShop))
			{
				_openShop.QueueFree();
				_openShop = null;
				EquipmentOpen = false;
				EquipmentLabel.LabelSettings.FontColor = Colors.White;
				return;
			}

			var shopScene = GD.Load<PackedScene>(ShopScenePath);
			if (shopScene == null)
			{
				GD.PrintErr($"Could not load join menu: {JoinScenePath}");
				return;
			}

			_openShop = shopScene.Instantiate();
			UiLayer.AddChild(_openShop);
			EquipmentOpen = true;
			EquipmentLabel.LabelSettings.FontColor = Colors.Green;


		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
