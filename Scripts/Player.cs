using Godot;
using System;
using System.Diagnostics;

public partial class Player : CharacterBody2D
{
	[Export] public int speed { get; set; } = 400;

	public Vector2 screenSize;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void GetInput()
	{
		Velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down") * speed;
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		LookAt(GetGlobalMousePosition());
		Debug.Print(GetGlobalMousePosition().ToString());
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}

}
