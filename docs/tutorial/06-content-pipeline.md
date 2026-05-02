# Content Pipeline

Previous: [Input](05-input.md) | Next: [Audio](07-audio.md)

## Goal

Load a PNG, `SpriteFont`, and WAV through MGCB, then understand the path rules that make content builds succeed or fail.

## What You Will Run

Run the working content experiment:

```bash
env E05_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj --no-restore
```

Run the deliberate failure from the experiment root:

```bash
cd experiments/e05-content-pipeline
dotnet mgcb /quiet /@:docs/broken-content.mgcb
```

## Key Files

- [experiments/e05-content-pipeline/Content/Content.mgcb](../../experiments/e05-content-pipeline/Content/Content.mgcb) - working MGCB file.
- [experiments/e05-content-pipeline/ContentAssets.cs](../../experiments/e05-content-pipeline/ContentAssets.cs) - logical asset names used by code.
- [experiments/e05-content-pipeline/docs/broken-content.mgcb](../../experiments/e05-content-pipeline/docs/broken-content.mgcb) - deliberately broken MGCB file.
- [docs/03-content-pipeline.md](../03-content-pipeline.md) - research note on MGCB workflow and failure modes.

## Walkthrough

MGCB compiles source assets into runtime `.xnb` files. Game code loads those compiled files through `Content.Load<T>` using logical names, not source filenames.

This experiment loads three assets:

```csharp
Content.Load<Texture2D>("Images/pipeline_tile");
Content.Load<SpriteFont>("PipelineStatus");
Content.Load<SoundEffect>("Audio/beep");
```

Those names come from `ContentAssets.cs` and match entries in `Content/Content.mgcb`. The source files include extensions, but the runtime names omit them.

`Content.RootDirectory = "Content"` tells MonoGame where to look for compiled content relative to the built executable. During build, MGCB writes `.xnb` output under the content build output directory; at runtime, `Content.Load<T>` reads those compiled files.

The broken MGCB file is intentional. It references `Images/missing_texture.png` from the wrong location, so `dotnet mgcb` reports a missing source file. That failure is part of the lesson: path mistakes should be reproduced in isolation before you blame game code.

The project sets `EnableMGCBItems=false` and explicitly includes only the real `Content/Content.mgcb`. Without that, MonoGame build targets may glob every `.mgcb` file and try to build the deliberately broken docs example.

## Expected Output

The working smoke run includes:

```text
LoadContent: texture=Images/pipeline_tile 64x64; font=PipelineStatus; sound=Audio/beep duration=250ms.
Smoke: played Content-loaded sound.
Smoke: exit.
```

The broken MGCB command should fail with a missing source file error similar to:

```text
docs/Images/missing_texture.png: error: The source file ... does not exist!
```

## Common Problems

- Relative paths are resolved from the `.mgcb` file's directory, not always from your shell's current directory.
- Filename casing matters, especially when moving between case-insensitive and case-sensitive filesystems.
- MGCB globbing can accidentally include docs or failure examples; use `EnableMGCBItems=false` when you need explicit includes.
- Missing `.xnb` files usually mean the content build failed or the logical name passed to `Content.Load<T>` is wrong.

## Checkpoint

You are ready when you can predict the logical name used by `Content.Load<Texture2D>("Images/pipeline_tile")`.
