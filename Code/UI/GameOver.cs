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
        GetTree().ChangeSceneToFile("res://MainMenu.tscn");
    }
}
