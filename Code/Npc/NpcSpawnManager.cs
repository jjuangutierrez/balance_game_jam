using Godot;
using System;
using System.Collections.Generic;

public partial class NpcSpawnManager : Node2D
{
  public static NpcSpawnManager Instance { get; private set; }

  [Export] PackedScene npcScene;
  [Export] Node2D spawnPoint;
  [Export] Vector2I spawnRateRange = new Vector2I(1, 5);
  [Export] public Node2D ExitPoint { get; private set; }

  List<Table> _tables = new();
  Timer _spawnTimer;
  List<Npc> _activeNpcs = new();
  int _maxNpcs = 14;
  AudioManager _audioManager;
  public override void _Ready()
  {
    _audioManager = GetNode<AudioManager>("/root/AudioManager");

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
    _spawnTimer = new Timer();
    AddChild(_spawnTimer);
    _spawnTimer.Timeout += OnTimerTimeout;
    int spawnTime = GD.RandRange(spawnRateRange.X, spawnRateRange.Y);
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
    Table availableTable = GetRandomAvailableTable();
    if (_activeNpcs.Count < _maxNpcs && !availableTable.IsOccupied){
      // Set a new random spawn time for the next NPC
      int newSpawnTime = GD.RandRange(spawnRateRange.X, spawnRateRange.Y);
      _spawnTimer.Start(newSpawnTime);

      SpawnNpc(availableTable);
    }
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

    _audioManager.PlaySound("doorBell", (float)GD.RandRange(1, 1.1));
  }

  private Table GetRandomAvailableTable()
  {
    var availableTables = _tables.FindAll(table => !table.IsOccupied);
    if (availableTables.Count > 0)
      return availableTables[GD.RandRange(0, availableTables.Count - 1)];
    return null;
  }

  public void OnExitAreaEnteded(Node2D body)
  {
    if (body is not Npc npc || npc == null)
      return;

    _activeNpcs.Remove(npc);
    npc.QueueFree();
  }

  // reset singleton
  public override void _ExitTree()
  {
    if (Instance == this)
      Instance = null;
  }
}