# Input

Previous: [Rendering](04-rendering.md) | Next: [Content Pipeline](06-content-pipeline.md)

## Goal

Poll keyboard, mouse, and optional gamepad state, then separate held input from input that was pressed this frame.

## What You Will Run

```bash
env E03_SMOKE_EXIT_AFTER_FRAMES=120 dotnet run --project experiments/e03-input/E03Input.csproj --no-restore
```

## Key Files

- [experiments/e03-input/InputSnapshot.cs](../../experiments/e03-input/InputSnapshot.cs) - compact input model used by movement code.
- [experiments/e03-input/ButtonEdgeState.cs](../../experiments/e03-input/ButtonEdgeState.cs) - tracks `PressedThisFrame`, held, and released state.
- [experiments/e03-input/PlayerMotion.cs](../../experiments/e03-input/PlayerMotion.cs) - applies axis or mouse input to the player position.
- [experiments/e03-input/InputSmokeSettings.cs](../../experiments/e03-input/InputSmokeSettings.cs) - produces deterministic smoke input.
- [experiments/e03-input.Tests/Program.cs](../../experiments/e03-input.Tests/Program.cs) - tests input state and motion without a window.

## Walkthrough

MonoGame input is polled during `Update`. This experiment reads:

- `Keyboard.GetState()` for WASD, arrows, `Space`, and `Escape`.
- `Mouse.GetState()` for left-click placement.
- `GamePad.GetState(PlayerIndex.One)` for an optional left-stick movement axis.

`InputSnapshot` turns those sources into two values: a movement axis and an optional mouse target. `PlayerMotion.Apply` then moves the square from the snapshot. Keyboard and gamepad-style movement share the same axis path; mouse input directly places the square at a target.

`ButtonEdgeState` compares previous and current button state. `PressedThisFrame` is true only on the first down frame. `Held` stays true while the button remains down. That distinction matters for actions like jump, fire, menu select, or toggle, where holding a button should not repeat every frame.

The smoke path does not require a real gamepad. `InputSmokeSettings` produces axis input for frames 1-40, mouse input for frames 41-80, and gamepad-style axis input for frames 81-120.

## Expected Output

The smoke run prints frame lines for each input source:

```text
Smoke: frame=40, position=..., input=axis.
Smoke: frame=80, position=..., input=mouse.
Smoke: frame=120, position=..., input=axis.
Smoke: exit.
```

Manual mode opens a window with a green square. Use WASD/arrows to move, left-click to place, `Space` to log button edges, and `Escape` to exit.

## Common Problems

- No gamepad connected is acceptable; the smoke path simulates gamepad-style axis input.
- `PressedThisFrame` should not stay true while a key is held. If it does, previous-frame state is not being stored correctly.
- `--no-restore` assumes packages are present. Rerun without it once if restore has not happened.

## Checkpoint

You are ready when you can distinguish `PressedThisFrame` from held input and explain why games usually need both.
