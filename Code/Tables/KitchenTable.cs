using Godot;
using System;

public partial class KitchenTable : StaticBody2D, IInteractable
{
  public void Interact(Node node)
  {
    if (node is not Dishes dishes)
      return;

    dishes.AddDish(1);
    if (dishes.DishCount < 10)
      GameManager.Instance.PlaySound("pickup");
  }
}
