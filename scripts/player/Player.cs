using Godot;
using System;

public partial class Player : CharacterBody3D
{
	[Export] public AnimationPlayer AnimationPlayer;
	[Export] public Node3D Head;
	[Export] public Camera3D Camera;

	[Export] public float WalkSpeed = 2.0f;
	[Export] public float SprintSpeed = 4.5f;
	[Export] public float MouseSensitivity = 0.002f;

	private float _gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity");

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		AnimationPlayer.Play("breathing_idle");
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// Mouse look
		if (@event is InputEventMouseMotion motion && Input.MouseMode == Input.MouseModeEnum.Captured)
		{
			RotateY(-motion.Relative.X * MouseSensitivity);
			Head.RotateX(-motion.Relative.Y * MouseSensitivity);

			Vector3 headRot = Head.Rotation;
			headRot.X = Mathf.Clamp(headRot.X, Mathf.DegToRad(-85), Mathf.DegToRad(85));
			Head.Rotation = headRot;
		}

		if (@event.IsActionPressed("ui_cancel"))
			Input.MouseMode = Input.MouseModeEnum.Visible;
		else if (@event is InputEventMouseButton click && click.Pressed)
			Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!IsOnFloor())
			velocity.Y -= _gravity * (float)delta;

		Vector2 input = Input.GetVector("move_left", "move_right", "move_front", "move_back");
		Vector3 direction = (Transform.Basis * new Vector3(input.X, 0, input.Y)).Normalized();

		bool sprinting = Input.IsActionPressed("sprint") && input.Y < 0;
		float speed = sprinting ? SprintSpeed : WalkSpeed;

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * speed;
			velocity.Z = direction.Z * speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, speed);
			velocity.Z = Mathf.MoveToward(velocity.Z, 0, speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		UpdateAnimation(direction != Vector3.Zero, sprinting);
	}

	private void UpdateAnimation(bool moving, bool sprinting)
	{
		string anim = !moving ? "breathing_idle" : sprinting ? "Sprint" : "move_front";
		if (AnimationPlayer.CurrentAnimation != anim)
			AnimationPlayer.Play(anim, 0.2);
	}
}
