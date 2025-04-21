using Godot;
using System;

public partial class Dishes : Node2D
{
  [ExportGroup("Dish Configuration")]
  [Export] Sprite2D dishSprite;
  [Export] int maxDishes = 10;
  [Export] Node2D dishesPivot;
  [Export(PropertyHint.Range, "0,1,0.01")] float curveFactor = 0.5f;

  [ExportGroup("Physics Configuration")]
  [Export] float maxTiltDegrees = 80;
  [Export] float tiltAcceleration = 160;
  [Export] float gravityForce = 80;
  [Export] float deadZoneAngle = 60f;
  [Export] float dampingFactor = 4f;
  [Export] float springConstant = 20f;

  public int DishCount { get; private set; }
  float _currentTiltAngle = 0f;
  float _angularVelocity = 0f;
  float _deltaTime = 0f;
  bool _allDishesFallen;

  public override void _Process(double delta)
  {
    _deltaTime = (float)delta;

    if (_allDishesFallen)
      return;

    if (DishCount <= 1)
      return;

    UpdatePhysics();
    OnDishesFall();
  }

  public void AddDish(int quantity = 1)
  {
    int newCount = DishCount + quantity;
    if (newCount <= maxDishes)
    {
      _allDishesFallen = false;
      DishCount = newCount;
      CreateVisualDishes(quantity);
    }
  }

  public void RemoveDish(int quantity = 1)
  {
    int newCount = Math.Max(0, DishCount - quantity);

    while (DishCount > newCount && dishesPivot.GetChildCount() > 0)
    {
      int lastIndex = dishesPivot.GetChildCount() - 1;
      var lastDish = dishesPivot.GetChild(lastIndex);
      dishesPivot.RemoveChild(lastDish);
      lastDish.QueueFree();
      DishCount--;
    }

    // Reset tilt when only one dish remains
    if (DishCount <= 1)
    {
      _currentTiltAngle = 0f;
      _angularVelocity = 0f;
      UpdateDishesPosition();
    }
  }

  private void UpdatePhysics()
  {
    Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");

    UpdateAngularVelocity(inputDirection);
    UpdateTiltAngle();
    UpdateDishesPosition();

    // QueueRedraw(); // Request redraw for debug visualization
  }

  private void UpdateAngularVelocity(Vector2 inputDirection)
  {
    if (inputDirection.X != 0)
    {
      // float difficultyFactor = 0.2f + (0.8f * ((float)_plateCount / maxPlates));
      _angularVelocity += -inputDirection.X * tiltAcceleration * /* difficultyFactor * */ _deltaTime;
    }
    else
    {
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

  private void CreateVisualDishes(int quantity)
  {
    if (dishSprite == null)
      return;

    for (int i = 0; i < quantity; i++)
    {
      int randomIndex = GD.RandRange(0, 2);
      Sprite2D dish = dishSprite.Duplicate() as Sprite2D;
      dish.Frame = randomIndex;
      dish.Visible = true;
      dishesPivot.AddChild(dish);
    }
  }

  private void UpdateDishesPosition()
  {
    if (DishCount <= 0 || dishesPivot.GetChildCount() == 0)
      return;

    int dishCount = dishesPivot.GetChildCount();
    Vector2 endPosition = CalculateEndPlatePosition();
    Vector2 direction = endPosition.Normalized();
    Vector2 perpendicular = new Vector2(-direction.Y, direction.X);
    Vector2 midPoint = endPosition / 2.0f;

    // Calculate curve intensity based on tilt angle
    float angleFactor = -_currentTiltAngle / maxTiltDegrees;
    float curveIntensity = endPosition.Length() / 2.0f * curveFactor * angleFactor;
    Vector2 controlPoint = midPoint + perpendicular * curveIntensity;

    // Position each plate along the curve
    for (int i = 0; i < dishCount; i++)
    {
      float heightRatio = i / (float)dishCount;

      // Get position on quadratic Bezier curve based on height ratio instead of uniform distribution
      Vector2 position = CalculateQuadraticBezier(Vector2.Zero, controlPoint, endPosition, heightRatio);

      // Calculate tangent for rotation
      Vector2 tangent = CalculateBezierTangent(Vector2.Zero, controlPoint, endPosition, heightRatio).Normalized();
      float rotation = Mathf.Atan2(tangent.Y, tangent.X) + Mathf.Pi / 2;

      // Apply position and rotation to plate
      Sprite2D dish = dishesPivot.GetChild<Sprite2D>(i);
      dish.Position = position;
      dish.Rotation = rotation;
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
    if (dishSprite == null)
      return Vector2.Zero;

    float spriteHeight = dishSprite.Texture.GetHeight() / 3 * .8f;
    float totalStackHeight = spriteHeight * DishCount;
    float tiltRadians = Mathf.DegToRad(_currentTiltAngle);

    return new Vector2(
      totalStackHeight * Mathf.Sin(tiltRadians),
      -totalStackHeight * Mathf.Cos(tiltRadians)
    );
  }

  public void OnDishesFall()
  {
    // TODO: fallen dishes particles
    if (Mathf.Abs(_currentTiltAngle) >= 80)
    {
      _allDishesFallen = true;
      _currentTiltAngle = 0;
      _angularVelocity = 0f;
      RemoveDish(DishCount);
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