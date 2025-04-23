using Godot;

public partial class PlayerController : CharacterBody2D
{
  [Export] float moveSpeed = 10;
  [Export] Sprite2D playerSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] Area2D interactionArea;
  [Export] Dishes dishes;
  private float _stepColdown = 0.3f;
  private float _stepTimer = 0.0f;
  int _lastDirection = 1;
  float _rightDishesX = 16f;
  float _leftDishesX = -16f;

  public Vector2 inputDirection { get; private set; }

  IInteractable _currentInteractable = null;
  AudioManager _audioManager;

  public float MoveSpeed
  {
    get { return moveSpeed; }
  }

  public override void _Ready()
  {
    _audioManager = GetNode<AudioManager>("/root/AudioManager");

    interactionArea.BodyEntered += OnInteractionAreaBodyEntered;
    interactionArea.BodyExited += OnInteractionAreaBodyExited;
    _rightDishesX = dishes.Position.X;
    _leftDishesX = -dishes.Position.X;
  }

  public override void _Process(double delta)
  {
    inputDirection = Input.GetVector("left", "right", "up", "down");

    // Step sound
    if (inputDirection != Vector2.Zero)
    {
      _stepTimer -= (float)delta;
      if (_stepTimer <= 0.0f)
      {
        _audioManager.PlaySound("footSteps", (float)GD.RandRange(1f, 1.1f));
        _stepTimer = _stepColdown;
      }
    }

    HandleAnimation();

    if (_currentInteractable != null && Input.IsActionJustPressed("interact"))
      _currentInteractable.Interact(dishes);
  }

  public override void _PhysicsProcess(double delta) => ApplyMovement();

  private void ApplyMovement()
  {
    Vector2 velocity = inputDirection.Normalized() * moveSpeed;
    Velocity = velocity;
    MoveAndSlide();
  }

  private void HandleAnimation()
  {
    if (inputDirection.Length() > 0)
      animationPlayer.Play("waiter_run");
    else
      animationPlayer.Play("waiter_idle");

    if (inputDirection.X > 0.1f)
    {
      playerSprite.Scale = new Vector2 (1, 1);
      _lastDirection = 1;
    }
    else if (inputDirection.X < -0.1f)
    {
      playerSprite.Scale = new Vector2 (-1, 1);
      _lastDirection = -1;
    }

    float dishesX = _lastDirection == 1 ? _rightDishesX : _leftDishesX;
    dishes.Position = new Vector2(dishesX, dishes.Position.Y);

  }

  private void OnInteractionAreaBodyEntered(Node body)
  {
    if (body is IInteractable interactable)
      _currentInteractable = interactable;
  }

  private void OnInteractionAreaBodyExited(Node body) => _currentInteractable = null;
}
