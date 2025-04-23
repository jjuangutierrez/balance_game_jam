using Godot;
using System;

public partial class KitchenTable : StaticBody2D, IInteractable
{
  AudioManager _audioManager;
  public override void _Ready()
  {
    _audioManager = GetNode<AudioManager>("/root/AudioManager");
  }

  public void Interact(Node node)
  {
    if (node is not Dishes dishes)
      return;

    if (dishes.DishCount < dishes.maxDishes)
      _audioManager.PlaySound("pickUpDish");

    dishes.AddDish(1);
  }
}
