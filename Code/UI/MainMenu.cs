using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button playButton;
    [Export] private Label recordLabel;


    public override void _Ready()
    {

        recordLabel.Text = $"Record score: {GameManager.Instance.RecordScore}";


        playButton.Pressed += OnPlayPressed;

    }

    private void OnPlayPressed()
    {
        GameManager.Instance.ChangeScene("res://TestScene.tscn");
    }
}
