using E06CameraAndCollision;
using Microsoft.Xna.Framework;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

static void AssertNear(float actual, float expected, string message)
{
    if (Math.Abs(actual - expected) > 0.001f)
    {
        throw new InvalidOperationException($"{message} Expected {expected}, got {actual}.");
    }
}

var camera = new Camera2D(position: new Vector2(100, 50), zoom: 1f);
camera.Apply(new CameraInput(new Vector2(1, -1), ZoomDelta: 1), elapsedSeconds: 0.5f);
AssertNear(camera.Position.X, 220f, "Camera should pan right by speed * time.");
AssertNear(camera.Position.Y, -70f, "Camera should pan up by speed * time.");
AssertTrue(camera.Zoom > 1f, "Positive zoom input should zoom in.");

camera.Apply(new CameraInput(Vector2.Zero, ZoomDelta: -100), elapsedSeconds: 1f);
AssertNear(camera.Zoom, Camera2D.MinZoom, "Camera zoom should clamp to the minimum.");

var screen = camera.WorldToScreen(worldPosition: camera.Position, viewportSize: new Point(960, 540));
AssertTrue(screen == new Vector2(480, 270), "Camera position should project to the viewport center.");

AssertTrue(Collision2D.AabbIntersects(new Rectangle(0, 0, 32, 32), new Rectangle(31, 31, 32, 32)), "Overlapping AABBs should collide.");
AssertTrue(!Collision2D.AabbIntersects(new Rectangle(0, 0, 32, 32), new Rectangle(40, 0, 32, 32)), "Separated AABBs should not collide.");

var movingCircle = new Circle(new Vector2(0, 0), Radius: 10f);
var staticCircle = new Circle(new Vector2(18, 0), Radius: 10f);
AssertTrue(Collision2D.CircleIntersects(movingCircle, staticCircle), "Overlapping circles should collide.");
AssertTrue(!Collision2D.CircleIntersects(movingCircle, new Circle(new Vector2(25, 0), 10f)), "Separated circles should not collide.");

var scene = CollisionScene.CreateDefault();
AssertTrue(!scene.Evaluate().AabbHit, "Default moving AABB should start outside the static AABB.");
AssertTrue(!scene.Evaluate().CircleHit, "Default moving circle should start outside the static circle.");
scene.MoveShapes(new Vector2(260, 0));
var collision = scene.Evaluate();
AssertTrue(collision.AabbHit, "Moving AABB should collide after moving into the static AABB.");
AssertTrue(collision.CircleHit, "Moving circle should collide after moving into the static circle.");

var disabledSmoke = CameraCollisionSmokeSettings.FromValues(null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled without frame value.");

var smoke = CameraCollisionSmokeSettings.FromValues("180");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}

AssertTrue(smoke.InputForFrame(30).Camera.Pan.X > 0, "Smoke should pan the camera right.");
AssertTrue(smoke.InputForFrame(70).Camera.ZoomDelta > 0, "Smoke should zoom in.");
AssertTrue(smoke.InputForFrame(110).ShapeDelta.X > 0, "Smoke should move shapes toward collisions.");
AssertTrue(!smoke.ShouldExit(179), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(180), "Smoke should exit at the configured frame.");

Console.WriteLine("E06 tests passed.");
