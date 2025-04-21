using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
    [Export] public float Score = 0;
    [Export] public float Time = 999;
    [Export] public float Dishes;
    [Export] public float CurrentTime;
    [Export] AnimationPlayer transitionAnimation;

    private string nextScene = "";

    public override void _Ready()
    {
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

    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "down" && !string.IsNullOrEmpty(nextScene))
        {
            GetTree().ChangeSceneToFile(nextScene);
            transitionAnimation.Play("up");

            if (nextScene == "res://TestScene.tscn")
            {
                CurrentTime = Time;
            }
            nextScene = "";
        }
    }

    public void AddTime(float extraTime)
    {
        CurrentTime += extraTime;
    }

    public void AddScore(float extraScore)
    {
        Score += extraScore;
    }

}