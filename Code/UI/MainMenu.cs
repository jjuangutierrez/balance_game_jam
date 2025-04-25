using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button continueButton;
    [Export] private Label recordLabel;
    GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        continueButton.Pressed += OnPlayPressed;

        recordLabel.Text = $"Record Time: {_gameManager.RecordTime.ToString("0.0")}";
    }

    private void OnPlayPressed() =>
        _gameManager.ChangeScene("res://MainScene.tscn");
}
