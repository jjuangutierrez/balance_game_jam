using Godot;
using System;

public partial class WaitingChairState : Node
{
    [Export] Sprite2D emoticonSprite;
    [Export] AnimationPlayer emoticonAnimationPlayer;
    [Export] Texture2D[] emoticons;
    [Export] Timer hideEmoticonTimer;
    double _timerEmoticon = 10;
    double _currentTimerEmotiocn;
    bool _hasChoosen = false;

    public override void _Ready()
    {
        _currentTimerEmotiocn = GD.RandRange(0, _timerEmoticon);
        hideEmoticonTimer.Timeout += OnHideEmoticon;
    }


    public void Waitting(double delta)
    {
        if (_hasChoosen)
            return;

        _currentTimerEmotiocn -= delta;
        if (_currentTimerEmotiocn > 0)
            return;

        _hasChoosen = true;

        int idx = GD.RandRange(0, emoticons.Length - 1);
        emoticonSprite.Texture = emoticons[idx];
        emoticonSprite.Visible = true;

        emoticonAnimationPlayer.Play("emoticon");
        hideEmoticonTimer.Start(2);
    }

    public void StopWaitting()
    {
        _hasChoosen = false;
        emoticonSprite.Visible = false;
        hideEmoticonTimer.Stop();
    }

    public void OnHideEmoticon()
    {
        _currentTimerEmotiocn = GD.RandRange(0, _timerEmoticon);
        emoticonSprite.Visible = false;
        hideEmoticonTimer.Start(2);
        _hasChoosen = false;
    }
}
