using Microsoft.Xna.Framework;

namespace E06CameraAndCollision;

public readonly record struct Circle(Vector2 Center, float Radius);

public static class Collision2D
{
    public static bool AabbIntersects(Rectangle first, Rectangle second)
    {
        return first.Left < second.Right
            && first.Right > second.Left
            && first.Top < second.Bottom
            && first.Bottom > second.Top;
    }

    public static bool CircleIntersects(Circle first, Circle second)
    {
        var radii = first.Radius + second.Radius;
        return Vector2.DistanceSquared(first.Center, second.Center) <= radii * radii;
    }
}
