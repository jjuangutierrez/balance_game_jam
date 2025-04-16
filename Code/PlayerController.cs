using Godot;
using System;
using System.Diagnostics;

public partial class PlayerController : CharacterBody2D
{
  [Export] float moveSpeed = 20;
  Vector2 _inputDirection = Vector2.Zero;

  public override void _Ready(){

  }

  public override void _Process(double delta){
    _inputDirection = Input.GetVector("left", "right", "up", "down");
  }

  public override void _PhysicsProcess(double delta){
    Vector2 velocity = Velocity;

    velocity.X = _inputDirection.X * moveSpeed;
    velocity.Y = _inputDirection.Y * moveSpeed;

    Velocity = velocity * moveSpeed;
    MoveAndSlide();
  }
}
