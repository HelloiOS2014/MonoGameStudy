using System;

namespace E02Rendering;

public sealed class RenderingSmokeSettings
{
    private RenderingSmokeSettings(int toggleAfterFrames, int exitAfterFrames)
    {
        ToggleAfterFrames = toggleAfterFrames;
        ExitAfterFrames = exitAfterFrames;
    }

    public int ToggleAfterFrames { get; }

    public int ExitAfterFrames { get; }

    public static RenderingSmokeSettings? FromEnvironment()
    {
        return FromValues(
            Environment.GetEnvironmentVariable("E02_SMOKE_TOGGLE_AFTER_FRAMES"),
            Environment.GetEnvironmentVariable("E02_SMOKE_EXIT_AFTER_FRAMES"));
    }

    public static RenderingSmokeSettings? FromValues(string? toggleAfterFrames, string? exitAfterFrames)
    {
        if (string.IsNullOrWhiteSpace(toggleAfterFrames) && string.IsNullOrWhiteSpace(exitAfterFrames))
        {
            return null;
        }

        if (!int.TryParse(toggleAfterFrames, out var toggleFrame) || toggleFrame < 1)
        {
            throw new InvalidOperationException("E02_SMOKE_TOGGLE_AFTER_FRAMES must be a positive integer.");
        }

        if (!int.TryParse(exitAfterFrames, out var exitFrame) || exitFrame <= toggleFrame)
        {
            throw new InvalidOperationException("E02_SMOKE_EXIT_AFTER_FRAMES must be an integer greater than E02_SMOKE_TOGGLE_AFTER_FRAMES.");
        }

        return new RenderingSmokeSettings(toggleFrame, exitFrame);
    }

    public bool ShouldPressToggle(int updateFrame)
    {
        return updateFrame == ToggleAfterFrames;
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
