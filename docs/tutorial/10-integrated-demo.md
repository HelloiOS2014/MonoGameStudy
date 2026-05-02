# Integrated Demo

Previous: [Publishing](09-publishing.md) | Next: [Troubleshooting](appendix-troubleshooting.md)

## Goal

Connect the previous concepts into the playable collector demo.

## What You Will Run

Manual run:

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

Automated smoke:

```bash
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

## Key Files

- [demo/integrated-demo/DemoState.cs](../../demo/integrated-demo/DemoState.cs) - deterministic game model.
- [demo/integrated-demo/DemoTypes.cs](../../demo/integrated-demo/DemoTypes.cs) - phases, input, result, pickup, and hazard types.
- [demo/integrated-demo/Game1.cs](../../demo/integrated-demo/Game1.cs) - MonoGame rendering, input, content, and logging.
- [demo/integrated-demo/Content/Content.mgcb](../../demo/integrated-demo/Content/Content.mgcb) - font and collect sound content build.
- [demo/integrated-demo.Tests/Program.cs](../../demo/integrated-demo.Tests/Program.cs) - model and smoke-input tests.

## Walkthrough

The demo is a tiny 2D collector. It has four phases:

- `Start`: waits for Enter.
- `Playing`: player moves, pickups can be collected, hazards can end the run.
- `Won`: all three pickups were collected.
- `Lost`: the player touched a hazard.

`DemoState` is the deterministic model. It owns player position, score, pickups, hazards, phase transitions, circle collision checks, and restart behavior. Because it is independent from rendering, `demo/integrated-demo.Tests/Program.cs` can test win, loss, restart, and smoke input without opening a window.

`Game1` connects the model to MonoGame. It polls keyboard input, loads the `Status` font and `Audio/collect` sound, draws the arena with runtime rectangles/circles, logs phase changes, and plays the collect sound.

The smoke mode uses `DEMO_SMOKE_EXIT_AFTER_FRAMES`. In smoke mode, the demo disables vsync, uses variable timestep at the MonoGame layer, then feeds the model a fixed `1/60` simulation step. That avoids a macOS GUI smoke hang while keeping the model path deterministic.

## Expected Output

Manual mode opens the demo. Press `Enter`, collect the yellow pickups, avoid red hazards, and press `Enter` or `R` after win/loss to restart.

Smoke output includes:

```text
Phase: started.
Collect: score=3/3.
Phase: won.
Phase: restarted.
Smoke: exit.
```

## Common Problems

- GUI smoke can hang under vsync/fixed timestep on macOS; this demo disables vsync and uses fixed smoke simulation time.
- The circles are drawn as filled rectangles in this learning demo; collision still uses circle math in `DemoState`.
- If content fails to load, rerun restore/build without `--no-restore` once and inspect `Content/Content.mgcb`.

## Checkpoint

You are done when you can run the demo manually or use smoke mode to prove the full start/play/win/restart loop.
