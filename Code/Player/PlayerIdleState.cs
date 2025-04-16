using Godot;
using System;

public partial class PlayerIdleState : State
{
    [Export] PlayerController Player;


    public override void Update(double delta)
    {
        if (Player.InputDirection != Vector2.Zero)
            EmitTransitioned("Run");
    }

}
