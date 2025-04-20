using Godot;
using System;

public partial class KitchenTable : StaticBody2D, IInteractable
{
  public void Interact(Node node)
  {
    if (node is not Dishes dishes)
      return;

    dishes.AddDish(1);
  }
}
