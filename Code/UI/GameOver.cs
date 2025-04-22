using Godot;
using System;

public partial class GameOver : Control
{
    [Export] Button backButton;
    [Export] Label totalScore;

    public override void _Ready()
    {
        backButton.Pressed += OnBackPressed;
        totalScore.Text = $"Total score: {GameManager.Instance.CurrentScore}";
    }

    void OnBackPressed()
    {
        if (GameManager.Instance.CurrentScore > GameManager.Instance.RecordScore)
        {
            GameManager.Instance.RecordScore = GameManager.Instance.CurrentScore;
        }

        GameManager.Instance.ChangeScene("res://MainMenu.tscn");
    }
}
