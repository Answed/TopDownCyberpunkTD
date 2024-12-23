using Godot;
using System;

public partial class Enemy : RigidBody3D
{
	[Export] private int health;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _onBodyEntered(Node3D body)
	{
		GD.Print("Bullet entered");
		health = health - 1;
		
		if(health <= 0)
			QueueFree();
	}
}
