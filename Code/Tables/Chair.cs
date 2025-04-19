using Godot;
using System;

public partial class Chair : Node2D
{
  [Export] Texture2D[] npcTextures;
  [Export] Sprite2D npcSprite;

  Timer timer;
  Npc _currentNpc;

  public bool IsOccupied { get; private set; }
  public bool IsReserved { get; set; }

  public override void _Ready()
  {
    timer = GetNode<Timer>("Timer");
  }

  public void SetNpc(Npc npc)
  {
    if (npc == null)
      throw new ArgumentNullException(nameof(npc));

    _currentNpc = npc;
    npcSprite.Texture = npcTextures[npc.NpcIndex];
    npcSprite.Visible = true;

    IsOccupied = true;
    IsReserved = true;

    timer.Start(5);
  }

  public void WaitingFood() { }

  private void Talking() { }

  private void Eating() { }

  private void Angry() { }

  public void OnTimerTimeout() => Leave();

  public void Leave()
  {
    if (_currentNpc == null)
      return;

    npcSprite.Visible = false;
    _currentNpc.Visible = true;

    _currentNpc.SetProcess(true);
    _currentNpc.MoveTo(NpcSpawnManager.Instance.exitPoint.GlobalPosition);

    IsOccupied = false;
    IsReserved = false;
    _currentNpc = null;
  }
}