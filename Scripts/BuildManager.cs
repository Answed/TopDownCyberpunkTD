using Godot;
using System;
using Godot.Collections;
using TopDownCyberpunkTD.Scripts.Resources;

public partial class BuildManager : Node3D
{
	[Signal] public delegate void BuildMenuOpenEventHandler();
	[Signal] public delegate void BuildMenuCloseEventHandler();
	
	[Export] private float yHeight;
	
	[Export] private Godot.Collections.Array<TowerResource> turrets;
	
	private Camera3D mainCam;
	private StaticBody3D previewturretScene;

	private Player player;
	
	private Vector3 rayOrigin;
	private Vector3 rayEnd;
	private Vector3 rayPosition;
	private Vector3 diffRay;
	private int selectedTurret;
	
	private Area3D buildingArea;
	
	private bool canBuild;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mainCam = GetViewport().GetCamera3D();
		player = GetParent<Player>();
		buildingArea = GetNode<Area3D>("BuildingArea");
		canBuild = true;
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
				buildingArea.GlobalPosition = new Vector3(rayPosition.X, yHeight, rayPosition.Z);
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
		if (player.GetResources() >= turrets[selectedTurret].Cost)
		{
			previewturretScene = GetTowerScene(turrets[selectedTurret].TowerScenePath).Instantiate<StaticBody3D>();
			AddChild(previewturretScene);	
		}
		else
		{
			GetViewport().SetInputAsHandled();
			EmitSignal("BuildMenuClose");
			GD.Print("Peasent");
			// TODO: Add some Kind of Visual Feedback
		}
	}
	private void BuildTurret()
	{
		if (!canBuild) return;
		
		GD.Print("Building turret");
		var newTurret = GetTowerScene(turrets[selectedTurret].TowerScenePath).Instantiate<StaticBody3D>();
		newTurret.Position = new Vector3(rayPosition.X, yHeight, rayPosition.Z);
		GetTree().Root.AddChild(newTurret);
		previewturretScene.QueueFree();
		previewturretScene = null;
		GetViewport().SetInputAsHandled();
		EmitSignal("BuildMenuClose");
		player.DecreaseResources(turrets[selectedTurret].Cost);
	}
	
	private PackedScene GetTowerScene(string TowerScenePath)
	{
		if (string.IsNullOrEmpty(TowerScenePath)) return null;
		return GD.Load<PackedScene>(TowerScenePath);
	}
	
	private void BodyEntered(Node body)
	{
		if (body is StaticBody3D)
		{
			GD.Print("I CANT BUILD HERE");
			canBuild = false;
		}
	}
	
	private void BodyExited(Node body){
		if (body is StaticBody3D)
		{
			GD.Print("I CAN BUILD HERE");
			canBuild = true;
		}
		
	}
}
