# Game Loop

Previous: [First Window](02-first-window.md) | Next: [Rendering](04-rendering.md)

## Goal

Understand fixed vs variable timestep in MonoGame and why the repo uses smoke variables to make a GUI loop testable.

## What You Will Run

```bash
env E01_SMOKE_TOGGLE_AFTER_FRAMES=30 E01_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj --no-restore
```

## Key Files

- [experiments/e01-game-loop/GameLoopState.cs](../../experiments/e01-game-loop/GameLoopState.cs) - tracks fixed/variable mode and edge-detects toggle input.
- [experiments/e01-game-loop/LoopSmokeSettings.cs](../../experiments/e01-game-loop/LoopSmokeSettings.cs) - parses smoke-test environment variables.
- [experiments/e01-game-loop.Tests/Program.cs](../../experiments/e01-game-loop.Tests/Program.cs) - tests loop state without opening a window.

## Walkthrough

MonoGame exposes timestep behavior through `IsFixedTimeStep`.

When `IsFixedTimeStep` is `true`, the experiment targets 60 updates per second with:

```csharp
TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
```

When `IsFixedTimeStep` is `false`, MonoGame runs in variable timestep mode. The experiment changes that flag at runtime when `F1` is pressed.

`GameLoopState` keeps the mode outside `Game1` so the behavior can be tested without graphics. It also remembers whether the toggle was already pressed; this prevents one held `F1` key from flipping modes every frame.

The smoke command does not synthesize a real key event. Instead, `LoopSmokeSettings` reads `E01_SMOKE_TOGGLE_AFTER_FRAMES` and `E01_SMOKE_EXIT_AFTER_FRAMES`, then `Update` treats the configured frame as if the toggle was pressed. This is more reliable on macOS than depending on synthetic keyboard events for a GUI app.

## Expected Output

The smoke run should open briefly, toggle once, and exit. Stdout includes:

```text
Update: timestep mode changed to Variable.
Smoke: exit.
```

If you run manually, press `F1` and watch the window title switch between `Fixed 60 Hz` and `Variable`.

## Common Problems

- Synthetic key events can be unreliable on macOS, so automated smoke uses environment variables.
- `E01_SMOKE_EXIT_AFTER_FRAMES` must be greater than `E01_SMOKE_TOGGLE_AFTER_FRAMES`.
- `--no-restore` assumes the project has already restored. If it fails due to missing packages, rerun without `--no-restore` once.

## Checkpoint

You are ready when you can describe why fixed 60 Hz is useful for deterministic learning experiments and why variable timestep is still worth seeing.
