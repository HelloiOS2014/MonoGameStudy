# e02 2D Rendering

## How to run

```bash
dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj
```

To run the behavior tests:

```bash
dotnet run --project experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj
```

To run the automated rendering smoke check:

```bash
E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj
```

## What you should see

A 960x540 window opens with a dark background, 1000 small sprites, and a text overlay in the upper-left corner showing render mode, sprite count, FPS, and frame time.

- Press `F1` to toggle between `Batched` and `Unbatched`.
- Press `Escape` to quit.
- The console prints one performance line per second.

The smoke check should print a stable batched line around 60fps, then `Update: render mode changed to Unbatched.`, then unbatched performance lines before exiting.

## What was learned

`SpriteBatch.Begin`/`End` boundaries matter. Drawing all sprites inside one batch is the normal path; forcing one `Begin`/`End` per sprite is intentionally inefficient and makes the batching cost visible.

This experiment also loads a texture through MGCB (`SpriteTile`, sourced from the template icon bitmap) and a `SpriteFont` (`Status`) for on-screen metrics. The first logged FPS sample can include startup/content-load cost, so the smoke test waits for a second batched sample before toggling.
