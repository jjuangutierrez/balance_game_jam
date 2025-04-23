using Godot;
using System;

public partial class UIManager : CanvasLayer
{
  [Export] private Label timer;
  [Export] private Label score;
  [Export] private AnimationPlayer animationPlayer;

  GameManager _gameManager;
  AudioManager _audioManager;

  public override void _Ready()
  {
    _gameManager = GetNode<GameManager>("/root/GameManager");
    _audioManager = GetNode<AudioManager>("/root/AudioManager");
  }

  public override void _Process(double delta)
  {
    timer.Text = _gameManager.CurrentTime.ToString("0");
    TimerAnimationHandler();
  }

  void TimerAnimationHandler()
  {
    /* TODO */
   /*  if (_gameManager.CurrentTime < 10)
      animationPlayer.Play("finishing_timer"); */
  }
}
