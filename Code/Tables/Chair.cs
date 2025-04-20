using Godot;
using System;

/// <summary>
/// TODO: agregar esperar x tiempo mientras se ejecuta un emoji random (5s)
/// TODO: Si tiene plato, mostrar animacion de comer (5s)
/// TODO: despues de comer ejecutar leave()
/// TODO: si no tiene plato mostrar carita enojada (3s) despues se va
/// TODO: la animaicon de leave se ejecuta despues de comer o
/// </summary>

public partial class Chair : Node2D
{
  [Export] public Texture2D[] npcTextures;
  [Export] public Sprite2D npcSprite;
  [Export] DefaultChairState defaultState;
  [Export] WaitingChairState waitingChairState;
  [Export] EatingChairState eatingChairState;
  [Export] LeavingChairState leavingChairState;
  [Export] Timer waitTimer;
  [Export] Timer eatTimer;
  private bool _isEating = false;
  private Npc _currentNpc;
  private bool food;
  private Table _table;


  public bool IsOccupied;
  public bool IsReserved;

  public override void _Ready()
  {
    waitTimer.Timeout += WaitTimeTimeOut;
    eatTimer.Timeout += EatTimerTimeOut;
    _table = GetParent() as Table;
  }


  public override void _Process(double delta)
  {
    GD.Print(food);
    food = _table.ThereAreFood;
    if (_currentNpc != null && food && !_isEating)
    {
        _isEating = true;
        eatTimer.Start(20);
        eatingChairState.Eating();
    }
    // Si hay un NPC sentado pero no hay comida, que espere
    else if (_currentNpc != null && !food && !_isEating)
    {
        waitingChairState.Waitting(delta);
    }
  }


  public void SetNpc(Npc npc)
  {
    if (npc == null)
      throw new ArgumentNullException(nameof(npc));

    _currentNpc = npc;

    defaultState.SetNpc(npc, this);
    // Start Wait timer
    waitTimer.Start(30);
  }

  public void WaitTimeTimeOut()
  {
    leavingChairState.Leave(_currentNpc, this);
    _currentNpc.AngryEmoticonSprite.Visible = true;
    waitingChairState.StopWaitting();
    _isEating = false;
    _currentNpc = null;
    _table.ThereAreFood = false;
    food = false;
  }

  public void EatTimerTimeOut()
  {
    leavingChairState.Leave(_currentNpc, this);
    _currentNpc.HappyEmoticonSprite.Visible = true;
    eatingChairState.StopEating();
    _isEating = false;
    _currentNpc = null;
    _table.ThereAreFood = false;
    food = false;
  }
}