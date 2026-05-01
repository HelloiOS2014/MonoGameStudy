using System;

namespace E07Animation;

public sealed class FrameAnimation
{
    private float _accumulator;

    public int FrameCount { get; }
    public float SecondsPerFrame { get; }
    public int CurrentFrame { get; private set; }

    public FrameAnimation(int frameCount, float secondsPerFrame)
    {
        if (frameCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frameCount));
        }

        if (secondsPerFrame <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(secondsPerFrame));
        }

        FrameCount = frameCount;
        SecondsPerFrame = secondsPerFrame;
    }

    public void Update(float elapsedSeconds)
    {
        _accumulator += elapsedSeconds;
        while (_accumulator >= SecondsPerFrame)
        {
            _accumulator -= SecondsPerFrame;
            CurrentFrame = (CurrentFrame + 1) % FrameCount;
        }
    }

    public void Reset()
    {
        _accumulator = 0f;
        CurrentFrame = 0;
    }
}
