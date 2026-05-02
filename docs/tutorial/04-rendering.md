# Rendering

Previous: [Game Loop](03-game-loop.md) | Next: [Input](05-input.md)

## Goal

Draw sprites in 2D and understand why `SpriteBatch` batching matters for performance.

## What You Will Run

```bash
env E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj --no-restore
```

## Key Files

- [experiments/e02-2d-rendering/Game1.cs](../../experiments/e02-2d-rendering/Game1.cs) - loads content and draws sprites/text.
- [experiments/e02-2d-rendering/SpriteField.cs](../../experiments/e02-2d-rendering/SpriteField.cs) - creates the 1000 sprite positions.
- [experiments/e02-2d-rendering/RenderModeState.cs](../../experiments/e02-2d-rendering/RenderModeState.cs) - toggles between `Batched` and `Unbatched`.
- [experiments/e02-2d-rendering/Content/Status.spritefont](../../experiments/e02-2d-rendering/Content/Status.spritefont) - font used for the on-screen overlay.

## Walkthrough

MonoGame's common 2D drawing path is `SpriteBatch`:

- `SpriteBatch.Begin` starts a batch.
- `SpriteBatch.Draw` queues a texture draw.
- `SpriteBatch.DrawString` queues text using a loaded `SpriteFont`.
- `SpriteBatch.End` flushes the queued work to the GPU.

The experiment draws 1000 sprites in two modes. `Batched` wraps all sprite draws in one `Begin`/`End` pair. `Unbatched` intentionally uses one `Begin`/`End` pair per sprite. That is not how you would normally write a game, but it makes the cost of broken batching visible.

This chapter also shows two asset styles. The sprite texture comes from content built by MGCB, while the input experiment in the next chapter creates a 1x1 runtime texture directly in code. Both are useful: content files are for real assets; runtime textures are fine for debug shapes and tiny experiments.

The overlay uses `SpriteBatch.DrawString` to print mode, sprite count, FPS, and frame time. Those numbers are teaching signals, not a benchmark suite.

## Expected Output

The window shows 1000 sprites and overlay text. The smoke run starts in:

```text
mode=Batched
```

Then it toggles:

```text
Update: render mode changed to Unbatched.
Smoke: exit.
```

## Common Problems

- Missing `Status.spritefont` or built content usually means restore/build did not finish cleanly.
- `Content.Load<SpriteFont>("Status")` uses the logical asset name, not the file extension.
- Very early FPS samples can include startup/content-load cost; do not treat the first line as a stable measurement.

## Checkpoint

You are ready when you can explain why grouping draw calls inside fewer `SpriteBatch.Begin`/`End` pairs matters.
