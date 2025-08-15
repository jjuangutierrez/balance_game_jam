using Godot;

public partial class UIManager : CanvasLayer
{
    [Export] private Label timer;
    [Export] private TextureProgressBar progressBar;
    [Export] private AnimationPlayer animationPlayer;

    GameManager _gameManager;

    public override void _Ready()
    {
        if (GetTree().Root.HasNode("GameManager"))
        {
            _gameManager = GetNode<GameManager>("/root/GameManager");
        }

        ValidateUINodes();
    }

    private void ValidateUINodes()
    {
        if (timer == null)
            GD.PrintErr("Error: Timer Label not found");
        if (progressBar == null)
            GD.PrintErr("Error: ProgressBar not found");
        if (animationPlayer == null)
            GD.PrintErr("Warning: AnimationPlayer not found");
    }

    public void UpdateTimer(double delta)
    {
        if (timer != null && _gameManager != null)
        {
            timer.Text = _gameManager.CurrentTime.ToString("0");
        }
    }

    public void UpdateProgressBar()
    {
        if (progressBar != null && _gameManager != null)
        {
            progressBar.Value = _gameManager.CurrentSatisfaction;
        }
    }
}