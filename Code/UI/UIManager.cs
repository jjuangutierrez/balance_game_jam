using Godot;
using System;

public partial class UIManager : CanvasLayer
{
  [Export] private Label timer;
  [Export] private Label score;
  [Export] private TextureProgressBar progressBar;
  [Export] private AnimationPlayer animationPlayer;

  GameManager _gameManager;

  public override void _Ready()
  {
    _gameManager = GetNode<GameManager>("/root/GameManager");
  }

  public void UpdateTimer(double delta)
  {
    timer.Text = _gameManager.CurrentTime.ToString("0");
  }

  public void UpdateProgressBar()
  {
    progressBar.Value = _gameManager.CurrentSatisfaction;
  }
}
