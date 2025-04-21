using Godot;
using System;

public partial class UIManager : CanvasLayer
{
  [Export] private Label timer;
  [Export] private Label score;
  [Export] private AnimationPlayer animationPlayer;
  GameManager _gameManager;

  public override void _Ready()
  {
    _gameManager = GetTree().Root.GetNode<GameManager>("/root/GameManager");

  }

  public override void _PhysicsProcess(double delta)
  {
    timer.Text = _gameManager.CurrentTime.ToString("0");

    TimerAnimationHandler();
  }

  void TimerAnimationHandler()
  {
    if (_gameManager.CurrentTime < 10)
      animationPlayer.Play("finishing_timer");
  }
}
