namespace E04Audio;

public enum MusicAction
{
    None,
    StartLoop,
    Stop
}

public readonly record struct AudioActions(bool PlayEffect, MusicAction MusicAction);

public sealed class AudioControlState
{
    private bool _musicStarted;

    public bool MusicEnabled { get; private set; } = true;

    public AudioActions Update(bool playEffectPressed, bool toggleMusicPressed)
    {
        var musicAction = MusicAction.None;
        if (!_musicStarted)
        {
            _musicStarted = true;
            musicAction = MusicAction.StartLoop;
        }

        if (toggleMusicPressed)
        {
            MusicEnabled = !MusicEnabled;
            musicAction = MusicEnabled ? MusicAction.StartLoop : MusicAction.Stop;
        }

        return new AudioActions(playEffectPressed, musicAction);
    }
}
