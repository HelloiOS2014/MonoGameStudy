using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace IntegratedDemo;

public sealed class DemoState
{
    public const int TargetScore = 3;
    public const float PlayerRadius = 18f;
    public const float PlayerSpeed = 160f;

    private readonly Vector2 _startPosition = new(90f, 120f);
    private readonly Hazard[] _hazards =
    {
        new(new Vector2(90f, 240f), 26f),
        new(new Vector2(430f, 120f), 30f)
    };
    private Collectible[] _collectibles =
    {
        new(new Vector2(170f, 120f), 16f, Collected: false),
        new(new Vector2(250f, 120f), 16f, Collected: false),
        new(new Vector2(330f, 120f), 16f, Collected: false)
    };

    public DemoPhase Phase { get; private set; } = DemoPhase.Start;
    public Vector2 PlayerCenter { get; private set; }
    public int Score { get; private set; }
    public IReadOnlyList<Collectible> Collectibles => _collectibles;
    public IReadOnlyList<Hazard> Hazards => _hazards;

    private DemoState()
    {
        PlayerCenter = _startPosition;
    }

    public static DemoState CreateDefault()
    {
        return new DemoState();
    }

    public DemoUpdateResult Update(DemoInput input, float elapsedSeconds)
    {
        if (Phase == DemoPhase.Start)
        {
            if (!input.StartPressed)
            {
                return default;
            }

            ResetRun();
            return new DemoUpdateResult(Started: true, Restarted: false, Collected: false, Won: false, Lost: false);
        }

        if (Phase is DemoPhase.Won or DemoPhase.Lost)
        {
            if (!input.RestartPressed)
            {
                return default;
            }

            ResetRun();
            return new DemoUpdateResult(Started: false, Restarted: true, Collected: false, Won: false, Lost: false);
        }

        var axis = input.MoveAxis;
        if (axis.LengthSquared() > 1f)
        {
            axis.Normalize();
        }

        PlayerCenter += axis * PlayerSpeed * elapsedSeconds;
        PlayerCenter = Vector2.Clamp(PlayerCenter, new Vector2(40f, 70f), new Vector2(880f, 430f));

        var collected = CollectOverlappingPickups();
        if (Score >= TargetScore)
        {
            Phase = DemoPhase.Won;
            return new DemoUpdateResult(Started: false, Restarted: false, collected, Won: true, Lost: false);
        }

        if (HitsHazard())
        {
            Phase = DemoPhase.Lost;
            return new DemoUpdateResult(Started: false, Restarted: false, collected, Won: false, Lost: true);
        }

        return new DemoUpdateResult(Started: false, Restarted: false, collected, Won: false, Lost: false);
    }

    private void ResetRun()
    {
        Phase = DemoPhase.Playing;
        PlayerCenter = _startPosition;
        Score = 0;
        for (var i = 0; i < _collectibles.Length; i++)
        {
            _collectibles[i] = _collectibles[i] with { Collected = false };
        }
    }

    private bool CollectOverlappingPickups()
    {
        var collected = false;
        for (var i = 0; i < _collectibles.Length; i++)
        {
            var pickup = _collectibles[i];
            if (pickup.Collected || !CircleIntersects(PlayerCenter, PlayerRadius, pickup.Center, pickup.Radius))
            {
                continue;
            }

            _collectibles[i] = pickup with { Collected = true };
            Score++;
            collected = true;
        }

        return collected;
    }

    private bool HitsHazard()
    {
        foreach (var hazard in _hazards)
        {
            if (CircleIntersects(PlayerCenter, PlayerRadius, hazard.Center, hazard.Radius))
            {
                return true;
            }
        }

        return false;
    }

    private static bool CircleIntersects(Vector2 firstCenter, float firstRadius, Vector2 secondCenter, float secondRadius)
    {
        var radii = firstRadius + secondRadius;
        return Vector2.DistanceSquared(firstCenter, secondCenter) <= radii * radii;
    }
}
