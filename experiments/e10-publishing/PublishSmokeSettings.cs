using System;

namespace E10Publishing;

public sealed class PublishSmokeSettings
{
    public const string ExitAfterFramesVariable = "E10_SMOKE_EXIT_AFTER_FRAMES";

    public int ExitAfterFrames { get; }

    private PublishSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public static PublishSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable(ExitAfterFramesVariable));
    }

    public static PublishSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (!int.TryParse(exitAfterFrames, out var parsed) || parsed <= 0)
        {
            return null;
        }

        return new PublishSmokeSettings(parsed);
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
