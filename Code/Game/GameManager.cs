using Godot;
using System;

public partial class GameManager : Node
{
    [Export] public float Score = 0;
    [Export] public float Time = 999999999;
    [Export] public float Plates;
    [Export] public float CurrentTime;

    public override void _Ready()
    {
        CurrentTime = Time;
    }

    public override void _Process(double delta)
    {
        CurrentTime -= (float)delta;
        if (CurrentTime <= 0)
        {
            GD.Print("game over");
            CurrentTime = Time;
            GetTree().ChangeSceneToFile("res://GameOver.tscn");
        }
    }

    public void AddTime(float extraTime)
    {
        CurrentTime += extraTime;
    }

    public void AddPlates(float extraPlates)
    {
        Plates += extraPlates;
    }

    public void AddScore(float extraScore)
    {
        Score += extraScore;
    }
}
