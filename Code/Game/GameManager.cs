using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
  public static GameManager Instance { get; private set; }

  [Export] public float score = 0;
  [Export] public float time = 999;
  [Export] public float plates;
  [Export] public float currentTime;
  public List<Texture2D> emoticons { get; private set; }

  public override void _Ready()
  {
    SetupInstance();

    currentTime = time;
  }

  private void SetupInstance()
  {
    if (Instance != null && Instance != this)
    {
      QueueFree();
      return;
    }
    Instance = this;
  }

  public override void _Process(double delta)
  {
    currentTime -= (float)delta;
    if (currentTime <= 0)
    {
      GD.Print("game over");
      currentTime = time;
      GetTree().ChangeSceneToFile("res://GameOver.tscn");
    }
  }

  public void AddTime(float extraTime)
  {
    currentTime += extraTime;
  }

  public void AddPlates(float extraPlates)
  {
    plates += extraPlates;
  }

  public void AddScore(float extraScore)
  {
    score += extraScore;
  }
}
