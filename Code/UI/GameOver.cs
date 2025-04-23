using Godot;
using System;

public partial class GameOver : Control
{
    [Export] Button backButton;
    [Export] Label totalTime;
    GameManager _gameManager;
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");

        totalTime.Text = $"Total Time: {_gameManager.CurrentTime.ToString("0.0")}";

        backButton.Pressed += OnBackPressed;
    }

    void OnBackPressed()
    {
        _gameManager.ChangeScene("res://MainMenu.tscn");
    }
}
