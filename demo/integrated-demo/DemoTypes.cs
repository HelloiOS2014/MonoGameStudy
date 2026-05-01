using Microsoft.Xna.Framework;

namespace IntegratedDemo;

public enum DemoPhase
{
    Start,
    Playing,
    Won,
    Lost
}

public readonly record struct DemoInput(Vector2 MoveAxis, bool StartPressed, bool RestartPressed);

public readonly record struct DemoUpdateResult(
    bool Started,
    bool Restarted,
    bool Collected,
    bool Won,
    bool Lost);

public readonly record struct Collectible(Vector2 Center, float Radius, bool Collected);

public readonly record struct Hazard(Vector2 Center, float Radius);
