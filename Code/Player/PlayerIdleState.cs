using Godot;

public partial class PlayerIdleState : State
{
    PlayerController _player;

    public override void Enter()
    {
        _player = GetParent().GetParent() as PlayerController;
    }

    public override void Update(double delta)
    {
        if (_player.inputDirection != Vector2.Zero)
            EmitTransitioned("Run");
    }

}
