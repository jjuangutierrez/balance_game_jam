using Godot;

public partial class PlayerController : CharacterBody2D
{
  [Export] float moveSpeed = 10;
  [Export] Sprite2D playerSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] Area2D interactionArea;
  [Export] Dishes dishes;

  public Vector2 inputDirection {get; private set;}

  IInteractable _currentInteractable = null;

  string _lastDirection = "right";

  // Animation name constants
  const string _ANIM_IDLE_RIGHT = "player_idle_right";
  const string _ANIM_IDLE_LEFT = "player_idle_left";
  const string _ANIM_RUN_RIGHT = "player_run_right";
  const string _ANIM_RUN_LEFT = "player_run_left";

  public float MoveSpeed {
    get { return moveSpeed; }
  }

  public override void _Ready()
  {
    interactionArea.BodyEntered += OnInteractionAreaBodyEntered;
    interactionArea.BodyExited += OnInteractionAreaBodyExited;

    animationPlayer.Play(_ANIM_IDLE_RIGHT);
    animationPlayer.SpeedScale = 1.2f;
  }

  public override void _Process(double delta)
  {
    inputDirection = Input.GetVector("left", "right", "up", "down");
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
    if (inputDirection == Vector2.Zero)
    {
      PlayIdleAnimation();
      return;
    }

    if (inputDirection.X != 0)
      _lastDirection = inputDirection.X < 0 ? "left" : "right";

    PlayRunAnimation();
  }

  private void PlayIdleAnimation()
  {
    string animation = _lastDirection == "left" ? _ANIM_IDLE_LEFT : _ANIM_IDLE_RIGHT;
    animationPlayer.Play(animation);
  }

  private void PlayRunAnimation()
  {
    string animation = _lastDirection == "left" ? _ANIM_RUN_LEFT : _ANIM_RUN_RIGHT;
    animationPlayer.Play(animation);
  }

  private void OnInteractionAreaBodyEntered(Node body)
  {
    if (body is IInteractable interactable)
      _currentInteractable = interactable;
  }

  private void OnInteractionAreaBodyExited(Node body) => _currentInteractable = null;
}
