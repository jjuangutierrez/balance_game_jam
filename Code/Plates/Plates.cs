using Godot;
using System;

public partial class Plates : Node2D
{
  [ExportGroup("Plate Configuration")]
  [Export] Sprite2D plateSprite;
  [Export] int maxPlates = 10;
  [Export] Node2D platesPivot;
  [Export(PropertyHint.Range, "0,1,0.01")] float curveFactor = 0.5f;

  [ExportGroup("Physics Configuration")]
  [Export] float maxTiltDegrees = 80;
  [Export] float tiltAcceleration = 160;
  [Export] float gravityForce = 80;
  [Export] float deadZoneAngle = 10f;
  [Export] float dampingFactor = 4f;
  [Export] float springConstant = 20f;

  int _plateCount = 0;
  float _currentTiltAngle = 0f;
  float _angularVelocity = 0f;
  float _deltaTime = 0f;
  bool _allPlatesFallen;

  public override void _Process(double delta)
  {
    if (_allPlatesFallen)
      return;

    _deltaTime = (float)delta;
    HandleInput();
    UpdatePhysics();

    OnPlatesFall();
  }

  public void AddPlate(int quantity = 1)
  {
    int newCount = _plateCount + quantity;
    if (newCount <= maxPlates)
    {
      _plateCount = newCount;
      CreateVisualPlates(quantity);
    }
  }

  public void RemovePlate(int quantity = 1)
  {
    int newCount = Math.Max(0, _plateCount - quantity);

    while (_plateCount > newCount && platesPivot.GetChildCount() > 0)
    {
      int lastIndex = platesPivot.GetChildCount() - 1;
      var lastPlate = platesPivot.GetChild(lastIndex);
      platesPivot.RemoveChild(lastPlate);
      lastPlate.QueueFree();
      _plateCount--;
    }
  }

  private void HandleInput()
  {
    if (Input.IsActionJustPressed("interact"))
      AddPlate();
  }

  private void UpdatePhysics()
  {
    Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
    float weightFactor = Mathf.Clamp(_plateCount / (float)maxPlates, 0, 1);

    UpdateAngularVelocity(inputDirection, weightFactor);
    UpdateTiltAngle();
    UpdatePlatesPosition();

    // QueueRedraw(); // Request redraw for debug visualization
  }

  private void UpdateAngularVelocity(Vector2 inputDirection, float weightFactor)
  {
    if (inputDirection.X != 0)
    {
      // Apply manual tilting force based on input
      _angularVelocity += -inputDirection.X * (tiltAcceleration * weightFactor) * _deltaTime;
    }
    else
    {
      // Apply physics when no input
      if (Mathf.Abs(_currentTiltAngle) < deadZoneAngle)
        ApplySpringStabilization();
      else
        ApplyGravityForce();
    }
  }

  private void ApplySpringStabilization()
  {
    // Simulate spring-like return to center with damping
    float springTorque = -springConstant * _currentTiltAngle;
    float dampingTorque = -dampingFactor * _angularVelocity;
    _angularVelocity += (springTorque + dampingTorque) * _deltaTime;
  }

  private void ApplyGravityForce()
  {
    _angularVelocity += Mathf.Sign(_currentTiltAngle) * gravityForce * _deltaTime;
  }

  private void UpdateTiltAngle()
  {
    _currentTiltAngle += _angularVelocity * _deltaTime;
    _currentTiltAngle = Mathf.Clamp(_currentTiltAngle, -maxTiltDegrees, maxTiltDegrees);
  }

  private void CreateVisualPlates(int quantity)
  {
    if (plateSprite == null)
      return;

    for (int i = 0; i < quantity; i++)
    {
      int randomIndex = GD.RandRange(0, 2);
      Sprite2D plate = plateSprite.Duplicate() as Sprite2D;
      plate.Frame = randomIndex;
      plate.Visible = true;
      platesPivot.AddChild(plate);
    }
  }

  private void UpdatePlatesPosition()
  {
    if (_plateCount <= 0 || platesPivot.GetChildCount() == 0)
      return;

    int plateCount = platesPivot.GetChildCount();
    Vector2 endPosition = CalculateEndPlatePosition();
    Vector2 direction = endPosition.Normalized();
    Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
    Vector2 midPoint = endPosition / 2.0f;

    // Calculate curve intensity based on tilt angle
    float angleFactor = -_currentTiltAngle / maxTiltDegrees;
    float curveIntensity = endPosition.Length() / 2.0f * curveFactor * angleFactor;
    Vector2 controlPoint = midPoint + perpendicular * curveIntensity;

    // Position each plate along the curve
    for (int i = 0; i < plateCount; i++)
    {
      float heightRatio = i / (float)plateCount;

      // Get position on quadratic Bezier curve based on height ratio instead of uniform distribution
      Vector2 position = CalculateQuadraticBezier(Vector2.Zero, controlPoint, endPosition, heightRatio);

      // Calculate tangent for rotation
      Vector2 tangent = CalculateBezierTangent(Vector2.Zero, controlPoint, endPosition, heightRatio).Normalized();
      float rotation = Mathf.Atan2(tangent.Y, tangent.X) + Mathf.Pi / 2;

      // Apply position and rotation to plate
      Sprite2D plate = platesPivot.GetChild<Sprite2D>(i);
      plate.Position = position;
      plate.Rotation = rotation;
    }
  }

  private Vector2 CalculateQuadraticBezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
  {
    Vector2 q0 = p0.Lerp(p1, t);
    Vector2 q1 = p1.Lerp(p2, t);
    return q0.Lerp(q1, t);
  }

  private Vector2 CalculateBezierTangent(Vector2 p0, Vector2 p1, Vector2 p2, float t)
  {
    return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
  }

  private Vector2 CalculateEndPlatePosition()
  {
    if (plateSprite == null)
      return Vector2.Zero;

    float spriteHeight = plateSprite.Texture.GetHeight() / 3 * .8f;
    float totalStackHeight = spriteHeight * _plateCount;
    float tiltRadians = Mathf.DegToRad(_currentTiltAngle);

    return new Vector2(
        totalStackHeight * Mathf.Sin(tiltRadians),
        -totalStackHeight * Mathf.Cos(tiltRadians)
    );
  }

  public void OnPlatesFall()
  {
    if (Mathf.Abs(_currentTiltAngle) >= 80)
    {
      _allPlatesFallen = true;
      GD.Print("Game Over");
    }
  }


  // public override void _Draw()
  // {
  //   if (_plateCount <= 0)
  //     return;

  //   Vector2 endPosition = CalculateEndPlatePosition();
  //   Vector2 direction = endPosition.Normalized();
  //   Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
  //   Vector2 midPoint = endPosition / 2.0f;

  //   float angleFactor = -_currentTiltAngle / (float)maxTiltDegrees;
  //   float curveIntensity = endPosition.Length() / 2.0f * curveFactor * angleFactor;
  //   Vector2 controlPoint = midPoint + perpendicular * curveIntensity;

  //   // Draw debug visualization
  //   DrawCircle(midPoint, 4, Colors.Pink);
  //   DrawCircle(controlPoint, 4, Colors.Blue);
  //   DrawLine(midPoint, controlPoint, Colors.Blue);
  //   DrawLine(Vector2.Zero, endPosition, Colors.Red);
  // }
}