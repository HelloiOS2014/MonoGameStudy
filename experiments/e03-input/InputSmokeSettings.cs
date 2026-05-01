using System;
using Microsoft.Xna.Framework;

namespace E03Input;

public sealed class InputSmokeSettings
{
    private InputSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public int ExitAfterFrames { get; }

    public static InputSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable("E03_SMOKE_EXIT_AFTER_FRAMES"));
    }

    public static InputSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (string.IsNullOrWhiteSpace(exitAfterFrames))
        {
            return null;
        }

        if (!int.TryParse(exitAfterFrames, out var exitFrame) || exitFrame < 90)
        {
            throw new InvalidOperationException("E03_SMOKE_EXIT_AFTER_FRAMES must be an integer greater than or equal to 90.");
        }

        return new InputSmokeSettings(exitFrame);
    }

    public InputSnapshot SnapshotForFrame(int updateFrame)
    {
        return updateFrame switch
        {
            <= 40 => new InputSnapshot(new Vector2(1, 0), null),
            <= 80 => new InputSnapshot(Vector2.Zero, new Vector2(480, 270)),
            <= 120 => new InputSnapshot(new Vector2(0, -1), null),
            _ => new InputSnapshot(Vector2.Zero, null)
        };
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
