using Godot;
using System;

public partial class MainMenu : Node
{
    [Export] private Button _playButton;
    [Export] private ColorRect _transitionRect;

    private ShaderMaterial _shaderMaterial;

    public override void _Ready()
    {
        _playButton.Pressed += OnPlayPressed;
        _shaderMaterial = _transitionRect.Material as ShaderMaterial;
        _transitionRect.Visible = false;
    }

    private async void OnPlayPressed()
    {
        _transitionRect.Visible = true;
        _shaderMaterial.SetShaderParameter("progress", 0f);

        var tween = GetTree().CreateTween();
        tween.TweenProperty(_shaderMaterial, "shader_parameter/progress", 1f, 1f);

        await ToSignal(tween, Tween.SignalName.Finished);

        GetTree().ChangeSceneToFile("res://TestScene.tscn");
    }
}
