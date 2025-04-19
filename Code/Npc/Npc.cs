using Godot;

public partial class Npc : CharacterBody2D
{
  [Export] Texture2D[] textures;
  [Export] float stopDistance = 60;
  [Export] float speed = 80;
  [Export] Sprite2D NPCSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] NavigationAgent2D agent;

  int _npcIndex = -1;
  int _chairIndex = -1;

  public Table AssignedTable { get; private set; }
  public int NpcIndex => _npcIndex;
  public int ChairIndex => _chairIndex;

  public override void _Ready()
  {
    InitializeAppearance();
  }

  private void InitializeAppearance()
  {
    NPCSprite.Texture = RandomTexture();
    animationPlayer.Play("npc_walk");
    animationPlayer.SpeedScale = 2f;
  }

  public override void _PhysicsProcess(double delta)
  {
    UpdateMovement();
  }

  private void UpdateMovement()
  {
    Vector2 targetPoint = agent.GetNextPathPosition();
    Vector2 target = ToLocal(targetPoint).Normalized();
    Velocity = target * speed;

    UpdateSpriteDirection();
    MoveAndSlide();
  }

  private void UpdateSpriteDirection()
  {
    if (Velocity.X < 0)
      NPCSprite.FlipH = true;
    else if (Velocity.X > 0)
      NPCSprite.FlipH = false;
  }

  public void MoveTo(Vector2? position = null)
  {
    if (position == null)
      agent.TargetPosition = AssignedTable.GlobalPosition;
    else
      agent.TargetPosition = position.Value;

    if (ProcessMode == ProcessModeEnum.Disabled)
    {
      Visible = true;
      SetProcess(true);
    }
  }

  public void AssignToTable(Table table, int chairIndex)
  {
    AssignedTable = table;
    _chairIndex = chairIndex;
    MoveTo();
  }

  private Texture2D RandomTexture()
  {
    int randomNumber = GD.RandRange(0, textures.Length - 1);
    _npcIndex = randomNumber;
    return textures[randomNumber];
  }
}