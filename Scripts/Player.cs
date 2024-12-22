using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public partial class Player : CharacterBody3D
{
	[Export] private int speed { get; set; } = 400;
	[Export] private PackedScene bulletScene;

	private int resources;
	private Vector3 rayOrigin;
	private Vector3 rayEnd;
	private Vector3 rayPosition;
	
	private Camera3D mainCam;
	private CollisionShape3D rig;
	private Node3D bulletSpawnPosition;

	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{ 
		mainCam = GetViewport().GetCamera3D();
		rig = GetNode<CollisionShape3D>("Rig");
		bulletSpawnPosition = GetNode<Node3D>("Rig/BulletSpawnPosition");
	}

	public void GetInput()
	{
		Vector2 inputVel = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = new Vector3(inputVel.X, 0, inputVel.Y)* speed;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var spaceState = GetWorld3D().DirectSpaceState;

		Vector2 mousePosition = GetViewport().GetMousePosition();
		rayOrigin = mainCam.ProjectRayOrigin(mousePosition);
		rayEnd = rayOrigin + mainCam.ProjectRayNormal(mousePosition) * 2000;

		var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
		var intersection = spaceState.IntersectRay(query);

		if (intersection.Count != 0)
		{
			rayPosition = intersection["position"].AsVector3();
			rig.LookAt(new Vector3(rayPosition.X, Position.Y, rayPosition.Z), new Vector3(0, 1, 0));
		}
		
		GetInput();
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Shoot"))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		Bullet bullet = bulletScene.Instantiate<Bullet>();
		AddChild(bullet);
		bullet.Position = bulletSpawnPosition.GlobalPosition;
		bullet.Direction(new Vector3(rayPosition.X, Position.Y, rayPosition.Z));
	}

	public Vector3 getRayPosition()
	{
		return rayPosition;
	}
}
