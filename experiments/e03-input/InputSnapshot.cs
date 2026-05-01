using Microsoft.Xna.Framework;

namespace E03Input;

public readonly record struct InputSnapshot(Vector2 MoveAxis, Vector2? MouseTarget);
