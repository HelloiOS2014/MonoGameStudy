# e03 Input

## How to run

```bash
dotnet run --project experiments/e03-input/E03Input.csproj
```

To run the behavior tests:

```bash
dotnet run --project experiments/e03-input.Tests/E03Input.Tests.csproj
```

To run the automated input smoke check:

```bash
E03_SMOKE_EXIT_AFTER_FRAMES=120 dotnet run --project experiments/e03-input/E03Input.csproj
```

## What you should see

A 960x540 window opens with a small green square.

- `WASD` or arrow keys move the square.
- Left mouse click places the square at the cursor.
- If a gamepad is connected, the left stick moves the square.
- Pressing and releasing `Space` prints edge-detection logs to stdout.
- Press `Escape` to quit.

The window title shows the current input source and square position. The smoke check automatically exercises keyboard-style movement, mouse placement, and gamepad-style movement, then exits.

## What was learned

MonoGame input is polled each `Update`; it does not send event callbacks for normal keyboard/gamepad state. Edge detection is a small state comparison between the previous frame and current frame. Keyboard and gamepad axes can share the same movement path, while mouse clicks are more naturally treated as a target position.
