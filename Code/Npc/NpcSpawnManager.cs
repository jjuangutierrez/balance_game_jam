using Godot;
using System;
using System.Collections.Generic;

public partial class NpcSpawnManager : Node2D
{
  public static NpcSpawnManager Instance { get; private set; }

  [Export] PackedScene npcScene;
  [Export] Node2D spawnPoint;
  [Export] Vector2I spawnRate = new Vector2I(5, 10);
  public Node2D exitPoint { get; private set; }

  List<Table> _tables = new();
  Timer _spawnTimer;
  List<Npc> _activeNpcs = new();
  int _maxNpcs = 10;

  public override void _Ready()
  {
    exitPoint = GetNode<Node2D>("exitPoint");

    SetupInstance();

    InitializeTables();
    InitializeSpawnPoint();
    InitializeTimer();
  }

  private void SetupInstance()
  {
    if (Instance != null && Instance != this)
    {
      QueueFree();
      return;
    }
    Instance = this;
  }

  private void InitializeSpawnPoint()
  {
    if (spawnPoint == null)
      spawnPoint = this;
  }

  private void InitializeTimer()
  {
    _spawnTimer = GetNode<Timer>("Timer");
    int spawnTime = GD.RandRange(spawnRate.X, spawnRate.Y);
    _spawnTimer.Start(spawnTime);
  }

  private void InitializeTables()
  {
    var tables = GetTree().GetNodesInGroup("Tables");
    foreach (var table in tables)
    {
      if (table is Table tableNode)
        _tables.Add(tableNode);
    }
  }

  private void OnTimerTimeout()
  {
    Table availableTable = GetRandomTable();
    if (_activeNpcs.Count < _maxNpcs && !availableTable.IsOccupied)
      SpawnNpc(availableTable);
  }

  private void SpawnNpc(Table table)
  {
    if (npcScene == null || spawnPoint == null)
      return;

    int chairIndex = table.ReserveChair();
    var npc = npcScene.Instantiate<Npc>();
    AddChild(npc);
    npc.GlobalPosition = spawnPoint.GlobalPosition;

    _activeNpcs.Add(npc);
    npc.AssignToTable(table, chairIndex);
  }

  private Table GetRandomTable()
  {
    var availableTables = _tables.FindAll(table => !table.IsOccupied);
    if (availableTables.Count > 0)
      return availableTables[GD.RandRange(0, availableTables.Count - 1)];
    return _tables[GD.RandRange(0, _tables.Count - 1)];
  }

  public void RemoveNpc(Npc npc)
  {
    if (npc == null)
      return;

    _activeNpcs.Remove(npc);
    npc.QueueFree();
  }

  public void OnExitAreaEnteded(Node2D body){
    if (body is not Npc npc)
      return;

    RemoveNpc(npc);
  }
}