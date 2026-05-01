using Microsoft.Xna.Framework;

namespace E06CameraAndCollision;

public readonly record struct CollisionFlags(bool AabbHit, bool CircleHit);

public sealed class CollisionScene
{
    public Rectangle MovingAabb { get; private set; }
    public Rectangle StaticAabb { get; }
    public Circle MovingCircle { get; private set; }
    public Circle StaticCircle { get; }

    private CollisionScene(Rectangle movingAabb, Rectangle staticAabb, Circle movingCircle, Circle staticCircle)
    {
        MovingAabb = movingAabb;
        StaticAabb = staticAabb;
        MovingCircle = movingCircle;
        StaticCircle = staticCircle;
    }

    public static CollisionScene CreateDefault()
    {
        return new CollisionScene(
            movingAabb: new Rectangle(40, 120, 70, 70),
            staticAabb: new Rectangle(300, 120, 90, 90),
            movingCircle: new Circle(new Vector2(80, 330), 35f),
            staticCircle: new Circle(new Vector2(340, 330), 45f));
    }

    public void MoveShapes(Vector2 delta)
    {
        MovingAabb = new Rectangle(
            MovingAabb.X + (int)delta.X,
            MovingAabb.Y + (int)delta.Y,
            MovingAabb.Width,
            MovingAabb.Height);
        MovingCircle = MovingCircle with { Center = MovingCircle.Center + delta };
    }

    public CollisionFlags Evaluate()
    {
        return new CollisionFlags(
            Collision2D.AabbIntersects(MovingAabb, StaticAabb),
            Collision2D.CircleIntersects(MovingCircle, StaticCircle));
    }
}
