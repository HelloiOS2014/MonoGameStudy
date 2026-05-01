using E03Input;
using Microsoft.Xna.Framework;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var button = new ButtonEdgeState();
button.Update(isDown: false);
AssertTrue(!button.PressedThisFrame, "Button should not be pressed on an initial up frame.");
AssertTrue(!button.Held, "Button should not be held on an initial up frame.");

button.Update(isDown: true);
AssertTrue(button.PressedThisFrame, "Button should report pressed on the first down frame.");
AssertTrue(button.Held, "Button should report held while down.");

button.Update(isDown: true);
AssertTrue(!button.PressedThisFrame, "Button should not repeat pressed while held.");
AssertTrue(button.Held, "Button should remain held on repeated down frames.");

button.Update(isDown: false);
AssertTrue(button.ReleasedThisFrame, "Button should report released on the first up frame.");

var start = new Vector2(100, 100);
var movedByKeyboard = PlayerMotion.Apply(start, new InputSnapshot(new Vector2(1, 0), null), speed: 120, elapsedSeconds: 0.5f);
AssertTrue(movedByKeyboard == new Vector2(160, 100), "Keyboard right input should move the player right.");

var movedByGamepad = PlayerMotion.Apply(start, new InputSnapshot(new Vector2(0, -1), null), speed: 120, elapsedSeconds: 0.5f);
AssertTrue(movedByGamepad == new Vector2(100, 40), "Gamepad up input should move the player up.");

var movedByMouse = PlayerMotion.Apply(start, new InputSnapshot(Vector2.Zero, new Vector2(320, 240)), speed: 120, elapsedSeconds: 0.5f);
AssertTrue(movedByMouse == new Vector2(320, 240), "Mouse press should place the player at the mouse target.");

var smoke = InputSmokeSettings.FromValues(exitAfterFrames: "120");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse valid frame values.");
}
AssertTrue(smoke.SnapshotForFrame(10).MoveAxis.X > 0, "Smoke should exercise keyboard-style right movement first.");
AssertTrue(smoke.SnapshotForFrame(50).MouseTarget is not null, "Smoke should exercise mouse movement second.");
AssertTrue(smoke.SnapshotForFrame(90).MoveAxis.Y < 0, "Smoke should exercise gamepad-style up movement third.");
AssertTrue(smoke.ShouldExit(updateFrame: 120), "Smoke should exit on the configured frame.");

Console.WriteLine("E03 tests passed.");
