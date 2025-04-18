using Godot;
using System;

public partial class Chair : Node2D
{
  [Export] Texture2D[] npcTextures;
  [Export] Sprite2D npcSprite;

  public bool isOccupied = false;

  public void SetNpcTexture(int index)
  {
    if (index >= npcTextures.Length)
      return;

    npcSprite.Texture = npcTextures[index];
    npcSprite.Visible = true;
  }

  public void HideNpc()
  {
    npcSprite.Visible = false;
  }
}
