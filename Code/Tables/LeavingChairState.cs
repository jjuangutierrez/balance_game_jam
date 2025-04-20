using Godot;
using System;

public partial class LeavingChairState : Node
{
    public void Leave(Npc npc, Chair chair)
    {
        chair.npcSprite.Visible = false;
        npc.Visible = true;

        npc.SetProcess(true);
        npc.MoveTo(NpcSpawnManager.Instance.exitPoint.GlobalPosition);

        chair.IsOccupied = false;
        chair.IsReserved = false;
    }
}
