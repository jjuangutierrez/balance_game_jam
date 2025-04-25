using Godot;
using System.Linq;

public partial class Table : StaticBody2D, IInteractable
{
  [Export] Area2D tableArea;
  [Export] NodePath[] chairPaths;
  [Export] public Sprite2D[] DishSprites { get; private set; }

  Chair[] _chairs;
  AudioManager _audioManager;
  public bool IsOccupied => _chairs.All(chair => chair.IsOccupied || chair.IsReserved);

  public override void _Ready()
  {
    _audioManager = GetNode<AudioManager>("/root/AudioManager");

    _chairs = chairPaths.Select(path => GetNode<Chair>(path)).ToArray();
    tableArea.BodyEntered += OnArea2DBodyEntered;
  }

  public bool IsDishServed(int chairIndex) => DishSprites[chairIndex].Visible;
  public void ClearDish(int chairIndex) => DishSprites[chairIndex].Visible = false;

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

  private int ServeDish(int availableDishes)
  {
    int servedDishes = 0;

    for (int i = 0; i < _chairs.Length; i++)
    {
      Chair chair = _chairs[i];
      if (chair.IsOccupied && chair.IsReserved && !DishSprites[i].Visible && servedDishes < availableDishes)
      {
        DishSprites[i].Visible = true;
        servedDishes++;
      }
    }

    return servedDishes;
  }

  private void OnArea2DBodyEntered(Node body)
  {
    if (body is not Npc npc)
      return;

    if (npc.AssignedTable != this || npc.ChairIndex == -1)
      return;

    SeatNpc(npc, _chairs[npc.ChairIndex]);
  }

  public void Interact(Node node)
  {
    if (node is not Dishes dishes)
      return;

    bool hasOccupiedChairWithoutDish = _chairs
      .Zip(DishSprites, (chair, dish) => chair.IsOccupied && !dish.Visible)
      .Any(x => x);

    if (dishes.DishCount > 0 && hasOccupiedChairWithoutDish)
    {
      _audioManager.PlaySound("serveDish", (float)GD.RandRange(1f, 1.1f));
      dishes.RemoveDish(ServeDish(dishes.DishCount));
    }
  }
}