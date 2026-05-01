using System;
using Microsoft.Xna.Framework;

namespace E06CameraAndCollision;

public readonly record struct CameraCollisionInput(CameraInput Camera, Vector2 ShapeDelta);

public sealed class CameraCollisionSmokeSettings
{
    public const string ExitAfterFramesVariable = "E06_SMOKE_EXIT_AFTER_FRAMES";

    public int ExitAfterFrames { get; }

    private CameraCollisionSmokeSettings(int exitAfterFrames)
    {
        ExitAfterFrames = exitAfterFrames;
    }

    public static CameraCollisionSmokeSettings? FromEnvironment()
    {
        return FromValues(Environment.GetEnvironmentVariable(ExitAfterFramesVariable));
    }

    public static CameraCollisionSmokeSettings? FromValues(string? exitAfterFrames)
    {
        if (!int.TryParse(exitAfterFrames, out var parsed) || parsed <= 0)
        {
            return null;
        }

        return new CameraCollisionSmokeSettings(parsed);
    }

    public CameraCollisionInput InputForFrame(int updateFrame)
    {
        if (updateFrame is >= 1 and <= 40)
        {
            return new CameraCollisionInput(new CameraInput(new Vector2(1f, 0f), 0f), Vector2.Zero);
        }

        if (updateFrame is >= 60 and <= 90)
        {
            return new CameraCollisionInput(new CameraInput(Vector2.Zero, 1f), Vector2.Zero);
        }

        if (updateFrame is >= 100 and <= 125)
        {
            return new CameraCollisionInput(new CameraInput(Vector2.Zero, 0f), new Vector2(10f, 0f));
        }

        return default;
    }

    public bool ShouldExit(int updateFrame)
    {
        return updateFrame >= ExitAfterFrames;
    }
}
