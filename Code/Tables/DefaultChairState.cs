using Godot;
using System;

public partial class DefaultChairState : Node
{
    public void SetNpc(Npc npc, Chair chair)
    {
        chair.npcSprite.Texture = chair.npcTextures[npc.NpcIndex];
        chair.npcSprite.Visible = true;

        chair.IsOccupied = true;
        chair.IsReserved = true;
    }
}
