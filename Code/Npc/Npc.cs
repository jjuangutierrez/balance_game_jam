using Godot;

public partial class Npc : CharacterBody2D
{
  [Export] Texture2D[] textures;
  [Export] float stopDistance = 5;
  [Export] float speed = 100;
  [Export] Sprite2D NPCSprite;
  [Export] AnimationPlayer animationPlayer;
  [Export] NavigationAgent2D agent;
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

  public void MoveTo(Vector2 position)
  {
    agent.TargetPosition = position;
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