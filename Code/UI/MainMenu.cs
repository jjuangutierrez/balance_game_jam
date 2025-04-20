using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button playButton;

    public override void _Ready()
    {
        playButton.Pressed += OnPlayPressed;
    }

    private void OnPlayPressed()
    {
        var gameManager = GetNode<GameManager>("/root/GameManager");
        gameManager.ChangeScene("res://TestScene.tscn");
    }
}
