using System;

namespace E07Animation;

public sealed class AnimationSmokeSettings
{
    public const string ExitAfterFramesVariable = "E07_SMOKE_EXIT_AFTER_FRAMES";

    public int ExitAfterFrames { get; }

    private AnimationSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public static AnimationSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable(ExitAfterFramesVariable));
    }

    public static AnimationSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (!int.TryParse(exitAfterFrames, out var parsed) || parsed <= 0)
        {
            return null;
        }

        return new AnimationSmokeSettings(parsed);
    }

    public CharacterInput InputForFrame(int updateFrame)
    {
        return updateFrame switch
        {
            >= 40 and <= 90 => new CharacterInput(horizontalAxis: 1f, jumpPressed: false),
            95 => new CharacterInput(horizontalAxis: 1f, jumpPressed: true),
            >= 96 and <= 145 => new CharacterInput(horizontalAxis: 1f, jumpPressed: false),
            _ => new CharacterInput(horizontalAxis: 0f, jumpPressed: false)
        };
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
