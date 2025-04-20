using Godot;
using System;

public partial class UIManager : CanvasLayer
{
  [Export] private Label timer;
  [Export] private Label score;
  [Export] private AnimationPlayer animationPlayer;

  public override void _PhysicsProcess(double delta)
  {
    timer.Text = GameManager.Instance.currentTime.ToString("0");
    TimerAnimationHandler();
  }

  void TimerAnimationHandler()
  {
    if (GameManager.Instance.currentTime < 10)
      animationPlayer.Play("finishing_timer");
  }
}
