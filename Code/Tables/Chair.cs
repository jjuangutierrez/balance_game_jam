using Godot;
using System;

public partial class Chair : Node2D
{
  [Export] private Texture2D[] npcTextures;
  [Export] private Sprite2D npcSprite;

  private Npc _currentNpc;

  public bool IsOccupied { get; private set; }
  public bool IsReserved { get; set; }

  public void SetNpc(Npc npc)
  {
    if (npc == null)
      throw new ArgumentNullException(nameof(npc));

    _currentNpc = npc;
    npcSprite.Texture = npcTextures[npc.NpcIndex];
    npcSprite.Visible = true;
    IsOccupied = true;
    IsReserved = true;
  }

  public void Leave()
  {
    // if (_currentNpc == null)
    //   return;

    // npcSprite.Visible = false;
    // _currentNpc.MoveTo(NpcSpawnManager.Instance.spawnPoint.GlobalPosition);
    // _currentNpc.SetProcess(true);
    // IsOccupied = false;
    // IsReserved = false;
    // _currentNpc = null;
  }
}
