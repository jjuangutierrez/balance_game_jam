using Godot;
using System;

public partial class Table : StaticBody2D
{
  [Export] private PackedScene chairPrefab;
  [Export] private Vector2[] chairPositions;

  private Chair[] _chairs;

  public override void _Ready()
  {
    _chairs = new Chair[2];
    for (int i = 0; i < 2; i++)
    {
      var chair = chairPrefab.Instantiate<Chair>();
      chair.Position = chairPositions[i];
      _chairs[i] = chair;
      AddChild(chair);
    }

    _chairs[1].Scale = new Vector2(-1, 1);
  }

  private void OnArea2DBodyEntered(Node body)
  {
    if (body is not Npc npc)
      return;

    if (npc.AssignedTable != this || npc.ChairIndex == -1)
      return;

    SeatNpc(npc, _chairs[npc.ChairIndex]);
  }

  private void SeatNpc(Npc npc, Chair chair)
  {
    npc.SetProcess(false);
    npc.Visible = false;
    chair.SetNpc(npc);
  }

  public int ReserveChair() => Array.FindIndex(_chairs, chair => !chair.IsOccupied && !chair.IsReserved);
  public bool IsOccupied => Array.TrueForAll(_chairs, chair => chair.IsOccupied || chair.IsReserved);
}