using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameManager : Node
{
    [Export] public float CurrentScene = 0;
    [Export] public float Time = 0;
    [Export] public float Dishes;
    [Export] public float CurrentTime;
    [Export] public float RecordTime;
    [Export] public float CurrentSatisfaction { get; private set; } = 50;
    [Export] AnimationPlayer transitionAnimation;
    [Export] UIManager _UiManager;

    private string nextScene = "";

    public override void _Ready()
    {
        if (transitionAnimation != null)
        {
            transitionAnimation.Play("up");
            transitionAnimation.AnimationFinished += OnAnimationFinished;
        }

        _UiManager = GetNode<UIManager>("/root/UI");
    }

    public override void _Process(double delta)
    {
        // TODO: change game scene

        if (_UiManager == null)
        {
            _UiManager = GetNode<UIManager>("/root/UI");
            if (_UiManager == null) return;
        }

        var currentScene = GetTree().CurrentScene;
        if (currentScene == null) return;

        switch (currentScene.SceneFilePath)
        {
            case "res://MainMenu.tscn":
                _UiManager.Hide();
                break;
            case "res://GameOver.tscn":
                _UiManager.Hide();
                break;
            case "res://TestScene.tscn":
                _UiManager.Show();
                _UiManager.UpdateTimer(delta);
                _UiManager.UpdateProgressBar();
                CurrentTime += (float)delta;

                if (CurrentSatisfaction <= 0)
                {
                    if (CurrentTime > RecordTime)
                        RecordTime = CurrentTime;
                    ChangeScene("res://GameOver.tscn");
                }
                break;
            default:
                break;
        }

    }

    public void ChangeScene(string scenePath)
    {
        nextScene = scenePath;

        if (transitionAnimation != null)
        {
            transitionAnimation.Play("down");
        }
        else
        {
            GetTree().ChangeSceneToFile(scenePath);

            if (scenePath == "res://TestScene.tscn")
            {
                ResetGameValues();
            }
        }
    }


    private void OnAnimationFinished(StringName animName)
    {
        if (animName == "down" && !string.IsNullOrEmpty(nextScene))
        {
            GetTree().ChangeSceneToFile(nextScene);

            if (transitionAnimation != null)
                transitionAnimation.Play("up");

            if (nextScene == "res://TestScene.tscn")
            {
                ResetGameValues();
            }

            nextScene = "";
        }
    }

    public void ResetGameValues()
    {
        CurrentSatisfaction = 50;
        CurrentTime = 0;
    }

    public void AddTime(float extraTime)
    {
        CurrentTime += extraTime;
    }

    public void DecreaseSatisfaction(int extraSatisfaction)
    {
        CurrentSatisfaction = Mathf.Clamp(CurrentSatisfaction - extraSatisfaction, 0, 100);
    }

    public void IncreaseSatisfaction(int extraSatisfaction)
    {
        CurrentSatisfaction = Mathf.Clamp(CurrentSatisfaction + extraSatisfaction, 0, 100);
    }

}