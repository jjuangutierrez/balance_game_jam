using Godot;
using System;

public partial class GameOver : Control
{
    [Export] Button backButton;
    [Export] Label totalScore;
    GameManager _gameManager;
    public override void _Ready()
    {
        backButton.Pressed += OnBackPressed;
        _gameManager = GetNode<GameManager>("/root/GameManager");
    }

    void OnBackPressed()
    {
        _gameManager.ChangeScene("res://MainMenu.tscn");
    }
}
