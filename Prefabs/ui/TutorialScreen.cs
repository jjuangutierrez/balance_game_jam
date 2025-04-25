using Godot;
using System;

public partial class TutorialScreen : Control
{
    [Export] Button playButton;

     GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        playButton.Pressed += OnPlayPressed;
    }

    private void OnPlayPressed() => _gameManager.ChangeScene("res://MainScene.tscn");
}
