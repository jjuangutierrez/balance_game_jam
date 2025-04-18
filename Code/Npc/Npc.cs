using Godot;

public partial class Npc : CharacterBody2D
{
  [Export] Texture2D[] textures;
  [Export] float stopDistance = 5;
  [Export] float speed = 100;
  [Export] Sprite2D NPCSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] NavigationAgent2D agent;
  float _distance;
  Vector2 _tablePosition;
  int _npcIndex;

  public int NpcIndex
  {
    get { return _npcIndex; }
  }

  public override void _Ready()
  {
    NPCSprite.Texture = RandomTexture();
    animationPlayer.Play("npc_walk");
  }

  public override void _Process(double delta)
  {
    Vector2 target = agent.GetNextPathPosition();
    if (target != Vector2.Zero)
    {
      _distance = (_tablePosition - Position).Length();
      GD.Print(_distance);

      if (GlobalPosition.DistanceTo(target) > stopDistance)
      {
        var dir = (target - GlobalPosition).Normalized();
        Velocity = dir * speed;
        MoveAndSlide();
      }
      else
      {
        Visible = false;
        SetProcess(false);
      }
    }
  }

  public void MoveNpcTo(Vector2 tablePosition)
  {
    _tablePosition = tablePosition;
    agent.TargetPosition = tablePosition;
    Visible = true;
    SetProcess(true);
  }

  Texture2D RandomTexture()
  {
    int randomNumber = GD.RandRange(0, textures.Length - 1);
    _npcIndex = (byte)randomNumber;
    return textures[randomNumber];
  }
}