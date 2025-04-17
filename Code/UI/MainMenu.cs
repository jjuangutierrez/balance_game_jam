using Godot;
using System;

public partial class MainMenu : Control
{
    [Export] private Button playButton;
    [Export] private ColorRect _spiralRect;

    private ShaderMaterial _spiralMaterial;

    public override void _Ready()
    {
        playButton.Pressed += OnPlayButtonPressed;
        _spiralMaterial = _spiralRect.Material as ShaderMaterial;
        _spiralRect.Visible = false;
    }

    public async void OnPlayButtonPressed()
    {
        _spiralRect.Visible = true;
        _spiralMaterial.SetShaderParameter("progress", 0f);

        var tween = GetTree().CreateTween();
        tween.TweenProperty(_spiralMaterial, "shader_parameter/progress", 1f, 1.5f);

        await ToSignal(tween, Tween.SignalName.Finished);

        GetTree().ChangeSceneToFile("res://TestScene.tscn");
    }
}
