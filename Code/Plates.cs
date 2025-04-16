using Godot;
using System;

public partial class Plates : Node2D
{
  [ExportGroup("Config")]
    [Export] Node2D platesPivot;
    [Export] public float maxTilt = 80;       // Inclinación máxima permitida
    [Export] public float tiltSpeed = 120;       // Inclinación máxima permitida
    [Export] public float gravity = 40f;  // Fuerza de la gravedad
        [Export] public float friction = 10f;
    [Export] public float recenterSpeed = 80f; // Ángulo para autorecentrado
    [Export] public float tiltAcceleration = 400; // Aceleración de inclinación

    private float _currentTilt = 0f;
    private float _angularVelocity = 0f; // Velocidad angular actual (en grados/seg)

  public override void _Process(double delta){
    float dt = (float)delta;
    Vector2 direction = Input.GetVector("left", "right", "up", "down");

    if(direction.X != 0){
       /* float targetTilt = -direction.X * maxTilt;
        _currentTilt = Mathf.MoveToward(_currentTilt, targetTilt, tiltSpeed * dt); */
          _angularVelocity += -direction.X * tiltAcceleration * dt;
    }else{
      /*  float gravityDirection = _currentTilt > 0 ? 1 : -1;
      _currentTilt += gravityDirection * gravity * dt;

      if(Mathf.Abs(_currentTilt) < 5){
        _currentTilt = Mathf.MoveToward(_currentTilt, 0, recenterSpeed * dt);
      } */
      if (_currentTilt != 0)
          _angularVelocity += Mathf.Sign(_currentTilt) * gravity * dt;
    }

/*     _currentTilt = Mathf.Clamp(_currentTilt, -maxTilt, maxTilt);

    // Aplicar rotación
    platesPivot.Rotation = Mathf.DegToRad(_currentTilt); */

       _angularVelocity = Mathf.MoveToward(_angularVelocity, 0, friction * dt);

        // Actualizamos la inclinación según la velocidad angular actual
        _currentTilt += _angularVelocity * dt;
        _currentTilt = Mathf.Clamp(_currentTilt, -maxTilt, maxTilt);

        // Aplicamos la rotación (recordando convertir a radianes)
        platesPivot.Rotation = Mathf.DegToRad(_currentTilt);
  }

}
