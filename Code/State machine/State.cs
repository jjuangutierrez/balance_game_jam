using Godot;
using System;

public partial class State : Node
{
    // Custom signal Transitioned to check if a state is changed.
    [Signal] public delegate void TransitionedEventHandler(string newStateName);
    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update(double delta) {}
    public virtual void PhysicsUpdate(double delta) {}

    // Emit the signal
    protected void EmitTransitioned(string newStateName)
    {
        EmitSignal(SignalName.Transitioned, newStateName);
    }
}
