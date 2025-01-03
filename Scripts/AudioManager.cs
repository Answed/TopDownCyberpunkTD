using Godot;
using System;
using System.Collections.Generic;

public partial class AudioManager : Node
{
	
	Dictionary<String,AudioStreamPlayer> sounds = new Dictionary<String,AudioStreamPlayer>();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadAllSounds();
	}

	private void LoadAllSounds()
	{
		foreach (Node audioStreamPlayer in GetChildren())
		{
			if(audioStreamPlayer is AudioStreamPlayer)
				sounds.Add(audioStreamPlayer.Name, (AudioStreamPlayer)audioStreamPlayer);
			else continue;
			GD.Print("Loading audio stream player: " + audioStreamPlayer.Name);
		}
		GD.Print("Audio Manager loaded");
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PlaySound(String SoundName)
	{
		sounds[SoundName].Play();
		GD.Print("Playing sound: " + SoundName);
	}
}
