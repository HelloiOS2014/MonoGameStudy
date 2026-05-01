using Microsoft.Xna.Framework;

namespace E03Input;

public static class PlayerMotion
{
    public static Vector2 Apply(Vector2 current, InputSnapshot snapshot, float speed, float elapsedSeconds)
    {
        if (snapshot.MouseTarget is { } mouseTarget)
        {
            return mouseTarget;
        }

        var axis = snapshot.MoveAxis;
        if (axis.LengthSquared() > 1f)
        {
            axis.Normalize();
        }

        return current + axis * speed * elapsedSeconds;
    }
}
