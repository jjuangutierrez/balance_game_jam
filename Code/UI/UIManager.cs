using Godot;
using System;

public partial class UIManager : CanvasLayer
{
  [Export] private Label timer;
  [Export] private Label score;
  [Export] private AnimationPlayer animationPlayer;

  public override void _PhysicsProcess(double delta)
  {
    timer.Text = GameManager.Instance.CurrentTime.ToString("0");
    score.Text = $"Score: {GameManager.Instance.CurrentScore}";

    TimerAnimationHandler();
  }

  void TimerAnimationHandler()
  {
    if (GameManager.Instance.CurrentTime < 10)
      animationPlayer.Play("finishing_timer");
  }
}
