using Godot;
using System;
using System.Collections.Generic;

public partial class NpcSpawnManger : Node2D
{
  public static NpcSpawnManger Instance { get; private set; }

  [Export] PackedScene npcPrefab;
  [Export] public Vector2 spawnPosition;
  List<Table> _tables = new List<Table>();
  Timer _timer;

  public override void _Ready()
  {
    if (spawnPosition == Vector2.Zero)
      spawnPosition = Position;

    if (Instance != null && Instance != this)
      QueueFree();
    else
      Instance = this;

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

        StartNpcNavigation(table, i);
        break;
      }
    }
  }

  private void StartNpcNavigation(Table table, int chairIndex)
  {

    Npc npc = npcPrefab.Instantiate<Npc>();
    AddChild(npc);

    table.SetSeatState(chairIndex, true);
    table.ShowChairNpc(npc);
    npc.Position = spawnPosition;
    npc.MoveNpcTo(table.Position);
  }

  private void OnTimerTimeOut()
  {
    CheckTables();
  }

  public void StopTimer()
  {
    _timer.Stop();
  }
}
