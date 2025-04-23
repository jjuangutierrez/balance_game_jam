using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    [Export] public float CurrentScene = 0;
    [Export] public float Time = 60;
    [Export] public float Dishes;
    [Export] public float CurrentTime;
    [Export] public float CurrentSatisfaction;
    [Export] AnimationPlayer transitionAnimation;
    private Dictionary<string, AudioStream> _sounds = new();
    private AudioStreamPlayer _audioStreamPlayer;

    private string nextScene = "";

    public override void _Ready()
    {
        _audioStreamPlayer = new AudioStreamPlayer();
        AddChild(_audioStreamPlayer);

   /*      // load sounds
        _sounds["steps"] = GD.Load<AudioStream>("res://Sounds/steps.wav");
        _sounds["pickup"] = GD.Load<AudioStream>("res://Sounds/pickup.wav");
        _sounds["breakinDishes"] = GD.Load<AudioStream>("res://Sounds/breakin dishes.wav");
        _sounds["bell"] = GD.Load<AudioStream>("res://Sounds/bell.wav");
        _sounds["soundRage"] = GD.Load<AudioStream>("res://Sounds/sound rage.wav"); */


        CurrentTime = Time;
        transitionAnimation.Play("up");

        transitionAnimation.AnimationFinished += OnAnimationFinished;
    }

    public override void _Process(double delta)
    {
        var currentScene = GetTree().CurrentScene;
        if (currentScene != null && currentScene.SceneFilePath == "res://TestScene.tscn")
        {
            CurrentTime -= (float)delta;
            if (CurrentTime <= 0)
            {
                GD.Print("game over");
                ChangeScene("res://GameOver.tscn");
                CurrentTime = 0;
            }
        }
    }

    public void ChangeScene(string scenePath)
    {
        nextScene = scenePath;
        transitionAnimation.Play("down");
    }

    public void PlaySound(string name)
    {
        if (_sounds.ContainsKey(name))
        {
            var player = new AudioStreamPlayer();
            player.Stream = _sounds[name];

            if (name == "steps")
                player.VolumeDb = -20;

            if (name == "pickup")
                player.VolumeDb = -20;

            if (name == "breakinDishes")
                player.VolumeDb = -10;

            if (name == "soundRage")
                player.VolumeDb = -10;

            AddChild(player);
            player.Play();

            // Delete node audio when that sfx its finished
            player.Finished += () => player.QueueFree();
        }
        else
        {
            GD.Print($"[Audio] sound not found!: {name}");
        }
    }



    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "down" && !string.IsNullOrEmpty(nextScene))
        {
            GetTree().ChangeSceneToFile(nextScene);
            transitionAnimation.Play("up");

            if (nextScene == "res://TestScene.tscn")
            {
                CurrentTime = Time;
                CurrentSatisfaction = 0;
            }
            nextScene = "";
        }
    }

    public void AddTime(float extraTime)
    {
        CurrentTime += extraTime;
    }

    public void IncreaseSatisfaction(int extraSatisfaction)
    {
        CurrentSatisfaction += extraSatisfaction;
    }

}