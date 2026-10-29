using Godot;
using System;

public partial class shop : ColorRect
{
	[ExportCategory("Menu Slots")]
	[Export] public Label Ready;
	[Export] public Label Join;
	[Export] public Label Equipment;
	[Export] public Label Location;
	[Export] public Label Profile;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
