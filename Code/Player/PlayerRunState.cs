using Godot;
using System;

public partial class PlayerRunState : State
{
    [Export] PlayerController Player;

    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = Player.Velocity;

        velocity.X = Player.InputDirection.X * Player.MoveSpeed;
        velocity.Y = Player.InputDirection.Y * Player.MoveSpeed;

        if (Player.InputDirection == Vector2.Zero)
        {
            EmitTransitioned("Idle");
        }

        Player.Velocity = velocity * Player.MoveSpeed;

        Player.MoveAndSlide();
    }



}
