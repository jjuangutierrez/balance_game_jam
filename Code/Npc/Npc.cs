using Godot;

public partial class Npc : CharacterBody2D
{
  [Export] Texture2D[] textures;
  [Export] float stopDistance = 60;
  [Export] float speed = 70;
  [Export] Sprite2D NPCSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] NavigationAgent2D agent;
  [Export] public Sprite2D EmoticonSprite { get; private set; }

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
  }

  public override void _PhysicsProcess(double delta)
  {
    UpdateMovement();
        agent.DebugEnabled = false;

    }

    private void UpdateMovement()
  {
    if (agent.IsNavigationFinished())
      return;

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

  public void AssignToTable(Table table, int chairIndex)
  {
    AssignedTable = table;
    _chairIndex = chairIndex;
    MoveTo();
  }

  public void MoveTo(Vector2? position = null)
  {
    agent.TargetPosition = position == null ? AssignedTable.GlobalPosition : position.Value;

    if (ProcessMode == ProcessModeEnum.Disabled)
    {
      Visible = true;
      SetProcess(true);
    }
  }

  public void ShowHappyEmotion(Texture2D[] happyTextures)
  {
    EmoticonSprite.Texture = happyTextures[GD.RandRange(0, happyTextures.Length - 1)];
  }

  public void ShowFrustrationEmotion(Texture2D[] frustrationTextures)
  {
    EmoticonSprite.Texture = frustrationTextures[GD.RandRange(0, frustrationTextures.Length - 1)];
  }

  private Texture2D RandomTexture()
  {
    int randomNumber = GD.RandRange(0, textures.Length - 1);
    _npcIndex = randomNumber;
    return textures[randomNumber];
  }
}