using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
public partial class AudioManager : Node
{
  Dictionary<string, AudioStreamPlayer> _sounds;

  public override void _Ready()
  {
    _sounds = GetChildren().OfType<AudioStreamPlayer>()
      .ToDictionary(player => player.Name.ToString(), player => player);

    PlaySound("Music", 1f);

  }


  public void PlaySound(string soundName, float pitchScale = 1f)
  {
    if (_sounds.TryGetValue(soundName, out var player))
    {
      player.Stop();
      player.PitchScale = pitchScale;

      if (soundName.ToLower().Contains("music"))
      {
        player.StreamPaused = false;
        player.Stream = player.Stream.Duplicate() as AudioStream;
      }

      player.Play();
    }
    else
    {
      GD.PrintErr($"AudioManager: sound '{soundName}' not found.");
    }
  }
}