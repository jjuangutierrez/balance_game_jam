using Godot;
using System;
using System.Collections.Generic;

public partial class Npc : Node2D
{
  [Export] Texture2D[] textures;
  [Export] Sprite2D NPCSprite;
  Table _currentTable;
  int _npcIndex;

  public int NpcIndex
  {
    get { return _npcIndex; }
  }

  public override void _Ready()
  {
    NPCSprite.Texture = RandomTexture();
  }


  Texture2D RandomTexture()
  {
    int randomNumber = GD.RandRange(0, textures.Length - 1);
    _npcIndex = (byte)randomNumber;
    return textures[randomNumber];
  }
}
