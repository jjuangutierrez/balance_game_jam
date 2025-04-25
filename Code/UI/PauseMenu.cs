using Godot;
using System;

public partial class PauseMenu : Control
{
    [Export] private HSlider Master;
    [Export] private HSlider Effects;
    [Export] private HSlider Music;

    string busName;
    int busIndex;

    private GameManager _gameManager;

    private bool _isPaused = false;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        ProcessMode = ProcessModeEnum.Always;
        Visible = false;

        // Sound settings
        SetupSlider(Master, "Master", 0);
        SetupSlider(Music, "Music", 0);
        SetupSlider(Effects, "SFX", 0);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            GetTree().Paused = true;
            Visible = true;
        }
        else
        {
            GetTree().Paused = false;
            Visible = false;
        }
    }

    private void OnContinuePressed()
    {
        TogglePause();
    }


    private void SetupSlider(HSlider slider, string bus, float defaultDb)
    {
        slider.MinValue = -80;
        slider.MaxValue = 20;

        int index = AudioServer.GetBusIndex(bus);

        float volumeDb = AudioServer.GetBusVolumeDb(index);

        if (volumeDb <= -79.0f)
        {
            volumeDb = defaultDb;
            AudioServer.SetBusVolumeDb(index, defaultDb);
        }

        slider.Value = volumeDb;

        slider.ValueChanged += (value) =>
        {
            AudioServer.SetBusVolumeDb(index, (float)value);
        };
    }

}