using Godot;
using System;
using System.Collections.Generic;

public partial class StateMachine : Node
{
    [Export] State currentState;
    [Export] State initialState;
    Dictionary<string, State> _states = new Dictionary<string, State>();

    public override void _Ready()
    {
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                //_states.Add(child.Name, state);
                _states[child.Name] = state;
                // For each child subscribe with Transtioned, whe Transitioned is emited run OnChildTransitioned
                state.Transitioned += (newStateName) => OnChildTransitioned(state, newStateName);
            }
        }

        if (initialState != null)
        {
            initialState.Enter();
            currentState = initialState;
        }
    }

    public override void _Process(double delta)
    {
        if (currentState != null)
        {
            currentState.Update(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (currentState != null)
        {
            currentState.PhysicsUpdate(delta);
        }
    }

    void OnChildTransitioned(State state, string newStateName)
    {
        if (state != currentState) return;

        if (_states.TryGetValue(newStateName, out State newState))
        {
            TransitionTo(newState);
        }
        else
        {
            GD.PrintErr($"State {newStateName} not founded");
        }

    }

    private void TransitionTo(State newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        newState.Enter();
        currentState = newState;
    }
}
