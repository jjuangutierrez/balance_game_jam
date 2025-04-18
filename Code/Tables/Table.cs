using Godot;
using System;

public partial class Table : StaticBody2D
{
  [Export] Chair[] _chairs = [];

  public Chair[] Chairs
  {
    get { return _chairs; }
  }

  public void SetSeatState(int chairIndex, bool isOccupied)
  {
    _chairs[chairIndex].isOccupied = isOccupied;
  }

  public void ShowChairNpc(Npc npc)
  {
    _chairs[npc.NpcIndex].SetNpc(npc);
  }

  public bool AllSeatsOccupied()
  {
    foreach (var chair in _chairs)
    {
      if (!chair.isOccupied)
        return false;
    }

    return true;
  }
}