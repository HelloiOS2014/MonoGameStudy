using System;
using Microsoft.Xna.Framework;

namespace IntegratedDemo;

public sealed class DemoSmokeSettings
{
    public const string ExitAfterFramesVariable = "DEMO_SMOKE_EXIT_AFTER_FRAMES";

    public int ExitAfterFrames { get; }

    private DemoSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public static DemoSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable(ExitAfterFramesVariable));
    }

    public static DemoSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (!int.TryParse(exitAfterFrames, out var parsed) || parsed <= 0)
        {
            return null;
        }

        return new DemoSmokeSettings(parsed);
    }

    public DemoInput InputForFrame(int updateFrame)
    {
        if (updateFrame == 1)
        {
            return new DemoInput(Vector2.Zero, StartPressed: true, RestartPressed: false);
        }

        if (updateFrame is >= 2 and <= 130)
        {
            return new DemoInput(new Vector2(1f, 0f), StartPressed: false, RestartPressed: false);
        }

        if (updateFrame == 140)
        {
            return new DemoInput(Vector2.Zero, StartPressed: false, RestartPressed: true);
        }

        return new DemoInput(Vector2.Zero, StartPressed: false, RestartPressed: false);
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
