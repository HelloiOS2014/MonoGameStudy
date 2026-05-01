# e01 Game Loop

## How to run

```bash
dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj
```

To run the loop-state tests:

```bash
dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj
```

To run the automated smoke check without manual input:

```bash
E01_SMOKE_TOGGLE_AFTER_FRAMES=30 E01_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj
```

## What you should see

A 960x540 CornflowerBlue window opens with the title `E01 Game Loop - Fixed 60 Hz`.

- Press `F1` to toggle between fixed 60 Hz and variable timestep mode.
- Press `Escape` to quit.
- Once per second, stdout prints the total frame count, frames drawn in the last second, and the current mode.

## What was learned

MonoGame owns the main loop through `Game.Run()`. Initialization and content setup happen before repeated `Update` and `Draw` calls. The `IsFixedTimeStep` property can be changed at runtime, and keyboard polling needs edge detection so a held key does not toggle every frame.

The first build also restored the template-local MGCB tools with `dotnet tool restore`, even though this experiment does not load content yet.

The smoke-test environment variables are intentionally narrow: they exercise the same toggle path as F1, then exit the game so the behavior can be verified from stdout in automation.
