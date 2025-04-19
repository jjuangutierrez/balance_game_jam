using Godot;

public partial class PlayerRunState : State
{
    PlayerController _player;

    public override void _Ready()
    {
      _player = GetParent().GetParent() as PlayerController;
    }

    public override void PhysicsUpdate(double delta)
    {
        Vector2 velocity = _player.Velocity;

        velocity.X = _player.inputDirection.X * _player.MoveSpeed;
        velocity.Y = _player.inputDirection.Y * _player.MoveSpeed;

        if (_player.inputDirection == Vector2.Zero)
            EmitTransitioned("Idle");

        _player.Velocity = velocity * _player.MoveSpeed;

        _player.MoveAndSlide();
    }



}
