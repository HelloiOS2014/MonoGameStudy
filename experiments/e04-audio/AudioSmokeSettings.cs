using System;

namespace E04Audio;

public readonly record struct AudioInputFrame(bool PlayEffectPressed, bool ToggleMusicPressed);

public sealed class AudioSmokeSettings
{
    public const string ExitAfterFramesVariable = "E04_SMOKE_EXIT_AFTER_FRAMES";

    public int ExitAfterFrames { get; }

    private AudioSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public static AudioSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable(ExitAfterFramesVariable));
    }

    public static AudioSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (!int.TryParse(exitAfterFrames, out var parsed) || parsed <= 0)
        {
            return null;
        }

        return new AudioSmokeSettings(parsed);
    }

    public AudioInputFrame InputForFrame(int updateFrame)
    {
        return updateFrame switch
        {
            30 => new AudioInputFrame(PlayEffectPressed: true, ToggleMusicPressed: false),
            60 => new AudioInputFrame(PlayEffectPressed: false, ToggleMusicPressed: true),
            90 => new AudioInputFrame(PlayEffectPressed: true, ToggleMusicPressed: false),
            120 => new AudioInputFrame(PlayEffectPressed: true, ToggleMusicPressed: true),
            _ => default
        };
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
