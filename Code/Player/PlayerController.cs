using Godot;

public partial class PlayerController : CharacterBody2D
{
  [Export] float moveSpeed = 10;
  [Export] Sprite2D playerSprite;
  [Export] AnimationPlayer animationPlayer;
  public Table table;

  public Vector2 inputDirection {get; private set;}

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
    animationPlayer.Play(_ANIM_IDLE_RIGHT);
    animationPlayer.SpeedScale = 1.2f;
  }

  public override void _Process(double delta)
  {
    inputDirection = Input.GetVector("left", "right", "up", "down");
    HandleAnimation();

    if (table != null && Input.IsActionJustPressed("interact"))
    {
      GD.Print("food delivered", table);
      table.ThereAreFood = true;
    }
  }

  public override void _PhysicsProcess(double delta)
  {
    ApplyMovement();
  }

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
}
