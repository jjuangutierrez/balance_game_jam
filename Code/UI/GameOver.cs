using Godot;
using System;

public partial class GameOver : Control
{
    [Export] Button backButton;

    public override void _Ready()
    {
        backButton.Pressed += OnBackPressed;
    }

    void OnBackPressed()
    {
        var gameManager = GetNode<GameManager>("/root/GameManager");
        gameManager.ChangeScene("res://MainMenu.tscn");
    }
}
