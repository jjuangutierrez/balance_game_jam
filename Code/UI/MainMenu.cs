using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button playButton;
    [Export] private Label recordLabel;
    GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        playButton.Pressed += OnPlayPressed;
    }

    private void OnPlayPressed()
    {
        _gameManager.ChangeScene("res://TestScene.tscn");
    }
}
