using Microsoft.Xna.Framework;

namespace E06CameraAndCollision;

public readonly record struct CameraInput(Vector2 Pan, float ZoomDelta);

public sealed class Camera2D
{
    public const float PanSpeed = 240f;
    public const float ZoomSpeed = 1f;
    public const float MinZoom = 0.25f;
    public const float MaxZoom = 4f;

    public Vector2 Position { get; private set; }
    public float Zoom { get; private set; }

    public Camera2D(Vector2 position, float zoom)
    {
        Position = position;
        Zoom = MathHelper.Clamp(zoom, MinZoom, MaxZoom);
    }

    public void Apply(CameraInput input, float elapsedSeconds)
    {
        Position += input.Pan * PanSpeed * elapsedSeconds;
        Zoom = MathHelper.Clamp(Zoom + input.ZoomDelta * ZoomSpeed * elapsedSeconds, MinZoom, MaxZoom);
    }

    public Vector2 WorldToScreen(Vector2 worldPosition, Point viewportSize)
    {
        return (worldPosition - Position) * Zoom + viewportSize.ToVector2() / 2f;
    }

    public Matrix GetViewMatrix(Point viewportSize)
    {
        return Matrix.CreateTranslation(new Vector3(-Position, 0f))
            * Matrix.CreateScale(Zoom, Zoom, 1f)
            * Matrix.CreateTranslation(new Vector3(viewportSize.ToVector2() / 2f, 0f));
    }
}
