using Godot;
using System;

enum State
{
	Searching,
	Attacking,
	
}

public partial class Enemy : RigidBody3D
{
	[Export] private int health;
	[Export] private float radius;
	[Export] private float height;

	private Node3D currentPlayerObject; //Includes Towers and Barriers as well.
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		setDetectionArea();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _onBodyEntered(Node3D body)
	{
		GD.Print("Bullet entered");
		health = health - 1;
		GD.Print(health);
		if(health <= 0)
			QueueFree();
	}

	private void _onDetectionRangeEntered(Node3D detectionRange)
	{
		GD.Print("Detection range entered");
		if (currentPlayerObject == null)
		{
			currentPlayerObject = detectionRange;
			GD.Print("Detection range entered " + currentPlayerObject.Name);
		}
	}

	private void _onDetectionRangeExited(Node3D detectionRange)
	{
		currentPlayerObject = null;
	}
	
	private void setDetectionArea()
	{
		CollisionShape3D detectionArea = GetNode<CollisionShape3D>("DetectionRange/Radius");
		CylinderShape3D range = new CylinderShape3D();
		range.Radius = radius;
		range.Height = height;
		detectionArea.SetShape(range);
	}
}
