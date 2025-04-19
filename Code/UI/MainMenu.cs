using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button _playButton;

    public override void _Ready()
    {
        _playButton.Pressed += OnPlayPressed;
    }

    private void OnPlayPressed()
    {
        var transition = GetTree().Root.GetNode<TransitionLayer>("Transition");

        // await transition.PlayOutTransition();

        GetTree().ChangeSceneToFile("res://TestScene.tscn");
    }
}
