using Godot;
using System;

public partial class UIManager : CanvasLayer
{
    [Export] private Label timer;
    [Export] private Label score;
    [Export] private AnimationPlayer animationPlayer;
    [Export] private GameManager gameManager;

    public override void _Ready()
    {
        gameManager = GetTree().Root.GetNode<GameManager>("GameManager");
    }


    public override void _PhysicsProcess(double delta)
    {

        timer.Text = gameManager.CurrentTime.ToString("0");
        TimerAnimationHandler();
    }

    void TimerAnimationHandler()
    {
        if (gameManager.CurrentTime < 10)
        {
            animationPlayer.Play("finishing_timer");
        } 
    }
}
