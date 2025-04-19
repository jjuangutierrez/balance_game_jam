using Godot;
using System;
using System.Collections.Generic;

public partial class NpcSpawnManager : Node2D
{
  public static NpcSpawnManager Instance { get; private set; }

  [Export] public PackedScene npcScene;
  [Export] public Node2D spawnPoint;

  private List<Table> _tables = new();
  private Timer _spawnTimer;
  private List<Npc> _activeNpcs = new();
  private int _maxNpcs = 10;
  private readonly float _minSpawnTime = 5f;
  private readonly float _maxSpawnTime = 10f;

  public override void _Ready()
  {
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
    _spawnTimer.WaitTime = GD.RandRange(_minSpawnTime, _maxSpawnTime);
    _spawnTimer.Start();
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

    var npc = npcScene.Instantiate<Npc>();
    AddChild(npc);
    npc.GlobalPosition = spawnPoint.GlobalPosition;

    _activeNpcs.Add(npc);

    int chairIndex = table.ReserveChair();
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
}