using Godot;
using System;

public partial class Npc : Node2D
{
    [Export] Texture2D[] textures;
    [Export] Sprite2D NPCSprite;

    public override void _Ready()
    {
        NPCSprite.Texture = RandomTexture();
    }


    Texture2D RandomTexture()
    {
        int randomNumber = GD.RandRange(0, textures.Length - 1);
        return textures[randomNumber];
    }
}
