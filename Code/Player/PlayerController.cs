using Godot;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

public partial class PlayerController : CharacterBody2D
{
  [Export] public float MoveSpeed = 20;
  [Export] public Sprite2D PlayerSprite;
  [Export] AnimationPlayer animationPlayer;
  public Vector2 InputDirection = Vector2.Zero;
  string _lastDirection = "right";


  public override void _Ready()
  {
    animationPlayer.Play("player_idle_right");
  }

  public override void _Process(double delta)
  {
    InputDirection = Input.GetVector("left", "right", "up", "down");
    HandleAnimation();
    
  }

  void HandleAnimation()
  {

    if (InputDirection.X < 0)
    {
      _lastDirection = "left";
      animationPlayer.Play("player_run_left");
    }
    else if (InputDirection.X > 0)
    {
      _lastDirection = "right";
      animationPlayer.Play("player_run_right");
    }
    else if (InputDirection.Y != 0)
    {
      if (_lastDirection == "left")
      {
        animationPlayer.Play("player_run_left");
      }
      else
      {
        animationPlayer.Play("player_run_right");
      }
    }
    else
    {
      if (_lastDirection == "left")
      {
        animationPlayer.Play("player_idle_left");
      }
      else
      {
        animationPlayer.Play("player_idle_right");
      }
    }

  }
}
