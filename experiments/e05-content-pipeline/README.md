# e05 Content Pipeline

## How to run

```bash
dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj
```

To run the asset-name and smoke-setting tests:

```bash
dotnet run --project experiments/e05-content-pipeline.Tests/E05ContentPipeline.Tests.csproj
```

To run the automated content smoke check:

```bash
E05_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj
```

## What you should see

A 960x540 window opens showing a green PNG tile and text rendered by a Content-loaded `SpriteFont`.

- Press `Space` to play the Content-loaded beep.
- Press `Escape` to quit.
- The automated smoke check loads the PNG, `SpriteFont`, and WAV, plays the sound once, then exits.

Expected smoke output includes:

```text
LoadContent: texture=Images/pipeline_tile 64x64; font=PipelineStatus; sound=Audio/beep duration=250ms.
Smoke: played Content-loaded sound.
Smoke: exit.
```

## What was learned

MGCB paths are relative to the `.mgcb` file. The normal build uses:

- `Images/pipeline_tile.png` through `TextureImporter` / `TextureProcessor`
- `PipelineStatus.spritefont` through `FontDescriptionImporter` / `FontDescriptionProcessor`
- `Audio/beep.wav` through `WavImporter` / `SoundEffectProcessor`

A deliberate broken file lives at `docs/broken-content.mgcb`. Running this from the experiment root:

```bash
dotnet mgcb /quiet /@:docs/broken-content.mgcb
```

fails with:

```text
docs/Images/missing_texture.png: error: The source file ... does not exist!
```

That failure is useful: MGCB reports the resolved absolute path, making bad relative paths easy to diagnose.

Important project-file detail: this experiment sets `EnableMGCBItems=false` and explicitly includes only `Content/Content.mgcb`, because the MonoGame build targets otherwise glob every `**/*.mgcb` file and would try to build the deliberate broken file too.
