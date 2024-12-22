using Godot;
using System;

public partial class AutoTurret : StaticBody3D
{
	[Export] private float radius;
	[Export] private float height;

	private Node3D currentEnemy;

	private MeshInstance3D tower;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		setDetectionArea();
		tower = GetNode<MeshInstance3D>("Tower");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		if(currentEnemy != null)
			tower.LookAt(new Vector3(currentEnemy.Position.X, Position.Y, currentEnemy.Position.Z), new Vector3(0, 1, 0));
	}
	
	private void _onDetectionRangeEntered(Node3D node)
	{
		GD.Print(node);
		if (currentEnemy == null) 
			currentEnemy = node; 
		GD.Print(currentEnemy);
	}
	private void _onDetectionRangeExited(Node3D node)
	{
		if (currentEnemy == node)
			currentEnemy = null;
		GD.Print("Uff");
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
