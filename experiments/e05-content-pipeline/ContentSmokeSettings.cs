using System;

namespace E05ContentPipeline;

public sealed class ContentSmokeSettings
{
    private ContentSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public int ExitAfterFrames { get; }

    public static ContentSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable("E05_SMOKE_EXIT_AFTER_FRAMES"));
    }

    public static ContentSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (string.IsNullOrWhiteSpace(exitAfterFrames))
        {
            return null;
        }

        if (!int.TryParse(exitAfterFrames, out var exitFrame) || exitFrame < 30)
        {
            throw new InvalidOperationException("E05_SMOKE_EXIT_AFTER_FRAMES must be an integer greater than or equal to 30.");
        }

        return new ContentSmokeSettings(exitFrame);
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
