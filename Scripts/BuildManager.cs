using Godot;
using System;

public partial class BuildManager : Node3D
{
	[Signal] public delegate void BuildMenuOpenEventHandler();
	[Signal] public delegate void BuildMenuCloseEventHandler();
	
	[Export] private PackedScene[] turrets;
	[Export] private float yHeight;
	
	private Camera3D mainCam;
	private StaticBody3D previewturretScene;
	
	private Vector3 rayOrigin;
	private Vector3 rayEnd;
	private Vector3 rayPosition;
	private Vector3 diffRay;
	private int selectedTurret;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainCam = GetViewport().GetCamera3D();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (previewturretScene != null)
		{
			var spaceState = GetWorld3D().DirectSpaceState;
			Vector2 mousePosition = GetViewport().GetMousePosition();
			rayOrigin = mainCam.ProjectRayOrigin(mousePosition);
			rayEnd = rayOrigin + mainCam.ProjectRayNormal(mousePosition) * 2000;
		
			var query = PhysicsRayQueryParameters3D.Create(rayOrigin, rayEnd);
				
			var intersection = spaceState.IntersectRay(query);
			
			if (intersection.Count > 0)
			{
				rayPosition = intersection["position"].AsVector3();
				previewturretScene.GlobalPosition = new Vector3(rayPosition.X, yHeight, rayPosition.Z);
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		for (int i = 1; i <= 4; i++)
		{
			if (@event.IsActionPressed($"T{i}"))
			{
				GD.Print("BuildMenu");
				EmitSignal(SignalName.BuildMenuOpen);
				Preview(i - 1);
				break; 
			}
		}


		if (@event.IsActionPressed("Shoot") && previewturretScene != null)
		{
			BuildTurret();
		}
	}

	private void Preview(int tower)
	{
		selectedTurret = tower;
		previewturretScene = turrets[tower].Instantiate<StaticBody3D>();
		AddChild(previewturretScene);
	}
	private void BuildTurret()
	{
		var newTurret = turrets[selectedTurret].Instantiate<StaticBody3D>();
		newTurret.Position = new Vector3(rayPosition.X, yHeight, rayPosition.Z);
		GetTree().Root.AddChild(newTurret);
		previewturretScene.QueueFree();
		previewturretScene = null;
		GetViewport().SetInputAsHandled();
		EmitSignal("BuildMenuClose");
	}
}
