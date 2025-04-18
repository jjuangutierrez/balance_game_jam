using Godot;
using System;

public partial class Chair : Node2D
{
  [Export] Texture2D[] npcTextures;
  [Export] Sprite2D npcSprite;
  Npc _npc;

  public bool isOccupied = false;

  public void SetNpc(Npc npc)
  {
    _npc = npc;
    npcSprite.Texture = npcTextures[npc.NpcIndex];
    npcSprite.Visible = true;
  }

  //TODO: Call this after timer timeout
  public void Leave()
  {
    npcSprite.Visible = false;
    _npc.MoveNpcTo(NpcSpawnManger.Instance.spawnPosition);
    _npc.SetProcess(true);
  }
}
