using Godot;
using System;

public partial class EatingChairState : Node
{
    [Export] Sprite2D plate;
    public void Eating()
    {
        plate.Visible = true;
    }

    public void StopEating()
    {
        plate.Visible = false;
    }
}
