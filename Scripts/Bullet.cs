using Godot;
using System;
using System.Diagnostics;

public partial class Bullet : RigidBody3D
{
	[Export] private float force;
	private Timer timer;

	private bool canBeDestroyed;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		canBeDestroyed = false;
	}

	public void Direction(Vector3 direction)
	{
		Vector3 movementDirection = (direction - Position).Normalized();
		ApplyCentralImpulse(movementDirection * force);
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (LinearVelocity == Vector3.Zero && canBeDestroyed) { QueueFree();}
	}
	
	private void _on_timer_timeout()
	{
		QueueFree();
	}

	private void _onBodyEntered(Node3D body)
	{
		QueueFree();
	}

}
