using Godot;
using System;

public partial class Plates : Node2D
{
  [ExportGroup("Config")]
  [Export] Node2D platesPivot;
  [Export] public float maxTilt = 80;
  [Export] public float gravity = 80;
  [Export] public float tiltAcceleration = 160;

  private float _currentTilt = 0f;
  private float _angularVelocity = 0f;

  public override void _Process(double delta)
  {
    float dt = (float)delta;
    Vector2 direction = Input.GetVector("left", "right", "up", "down");

    if (direction.X != 0)
    {
      _angularVelocity += -direction.X * tiltAcceleration * dt;
    }
    else
    {
      if (_currentTilt != 0)
        _angularVelocity += Mathf.Sign(_currentTilt) * gravity * dt;
    }

    _currentTilt += _angularVelocity * dt;
    _currentTilt = Mathf.Clamp(_currentTilt, -maxTilt, maxTilt);

    platesPivot.Rotation = Mathf.DegToRad(_currentTilt);
  }

}
