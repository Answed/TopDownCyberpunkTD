namespace TopDownCyberpunkTD.Scripts.Resources;
using Godot;

[GlobalClass]
public partial class TowerResource : Resource
{
	[Export]
	public int Cost { get; set; }

	// This hint provides a file picker in the inspector restricted to scene files
	[Export(PropertyHint.File, "*.tscn,*.scn")]
	public string TowerScenePath { get; set; }
}
