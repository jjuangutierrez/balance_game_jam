using Godot;
using System;
using System.Collections.Generic;

public partial class NpcSpawnManger : Node2D
{
  [Export] PackedScene _npc;
  List<Table> _tables = new List<Table>();
  Timer _timer;
  public override void _Ready()
  {
    var tableNodes = GetTree().GetNodesInGroup("Tables");
    foreach (var node in tableNodes)
      _tables.Add(node as Table);

    _timer = GetNode<Timer>("Timer");
    _timer.Start();
  }

  private void CheckTables()
  {
    foreach (var table in _tables)
    {
      if (table.AllSeatsOccupied())
        continue;

      for (int i = 0; i < table.Chairs.Length; i++)
      {
        if (table.Chairs[i].isOccupied)
          continue;

        SpawnNpc(table, i);
        return;
      }
    }
  }

  private void SpawnNpc(Table table, int chairIndex)
  {
    var npc = _npc.Instantiate<Npc>();
    table.SetSeatState(chairIndex, true);
  }

  private void OnTimerTimeOut()
  {
    GD.Print("Timer timeout");
    // CheckTables();
  }

  //TODO: Use global
  private void StopTimer()
  {
    _timer.Stop();
  }
}
