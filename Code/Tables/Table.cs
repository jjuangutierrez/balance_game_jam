using Godot;
using System;

public partial class Table : StaticBody2D
{
  [Export] PackedScene chairPrefab;
  [Export] Area2D tableArea;
  public bool ThereAreFood;
  Vector2[] _chairPositions = [new Vector2(-29, 0), new Vector2(29, 0)];

  private Chair[] _chairs;

  public override void _Ready()
  {
    _chairs = new Chair[2];
    for (int i = 0; i < 2; i++)
    {
      var chair = chairPrefab.Instantiate<Chair>();
      chair.Position = _chairPositions[i];
      _chairs[i] = chair;
      AddChild(chair);
    }

    _chairs[1].Scale = new Vector2(-1, 1);

    tableArea.BodyEntered += OnArea2DBodyEntered;
    tableArea.BodyExited += OnArea2DBodyExit;
  }

  private void SeatNpc(Npc npc, Chair chair)
  {
    npc.SetProcess(false);
    npc.Visible = false;
    chair.SetNpc(npc);
  }

  public int ReserveChair()
  {
    for (int i = 0; i < _chairs.Length; i++)
    {
      if (!_chairs[i].IsOccupied && !_chairs[i].IsReserved)
      {
        _chairs[i].IsReserved = true;
        return i;
      }
    }
    return -1;
  }

  public bool IsOccupied => Array.TrueForAll(_chairs, chair => chair.IsOccupied || chair.IsReserved);

  private void OnArea2DBodyEntered(Node body)
  {
    if (body is PlayerController player)
      player.table = this;

    if (body is not Npc npc)
      return;

    if (npc.AssignedTable != this || npc.ChairIndex == -1)
      return;

    SeatNpc(npc, _chairs[npc.ChairIndex]);

  }

  private void OnArea2DBodyExit(Node body)
  {
    if (body is PlayerController player)
      player.table = null;
  }
}