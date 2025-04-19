using Godot;
using System;
using System.Threading.Tasks;

public partial class TransitionLayer : CanvasLayer
{
/*     [Export] private ColorRect transitionRect;
    [Export] private float transitionDuration = 1.0f;
    [Export] private int transitionType = 0;

    private ShaderMaterial shaderMaterial;

    public override void _Ready()
    {
        if (transitionRect == null)
        {
            GD.PrintErr("TransitionRect no asignado en TransitionLayer");
            return;
        }

        shaderMaterial = transitionRect.Material as ShaderMaterial;

        if (shaderMaterial == null)
        {
            GD.PrintErr("El ColorRect no tiene un ShaderMaterial asignado");
            return;
        }

        shaderMaterial.SetShaderParameter("transition_type", transitionType);

        transitionRect.Visible = true;
        shaderMaterial.SetShaderParameter("progress", 1.0f);

        PlayInTransition();
    }

    public async Task PlayInTransition()
    {
        transitionRect.Visible = true;
        shaderMaterial.SetShaderParameter("progress", 1.0f);

        var tween = GetTree().CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(shaderMaterial, "shader_parameter/progress", 0.0f, transitionDuration);

        await ToSignal(tween, Tween.SignalName.Finished);
    }

    public async Task PlayOutTransition()
    {
        transitionRect.Visible = true;
        shaderMaterial.SetShaderParameter("progress", 0.0f);

        var tween = GetTree().CreateTween();
        tween.SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(shaderMaterial, "shader_parameter/progress", 1.0f, transitionDuration);

        await ToSignal(tween, Tween.SignalName.Finished);
    }

    public void SetTransitionType(int type)
    {
        transitionType = type;
        shaderMaterial.SetShaderParameter("transition_type", type);
    } */
}