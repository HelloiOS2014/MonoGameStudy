# Content Pipeline

MonoGame's Content Pipeline compiles source assets into runtime `.xnb` files at build time. The project code then loads assets by logical name through `Content.Load<T>()`.

## Workflow Used In e05

The experiment root contains `Content/Content.mgcb`. It builds:

- `Images/pipeline_tile.png` as a `Texture2D`.
- `PipelineStatus.spritefont` as a `SpriteFont`.
- `Audio/beep.wav` as a `SoundEffect`.

Runtime code loads them with:

```csharp
Content.Load<Texture2D>("Images/pipeline_tile");
Content.Load<SpriteFont>("PipelineStatus");
Content.Load<SoundEffect>("Audio/beep");
```

Asset names are path-like logical names without the source extension.

## MGCB Details

MGCB paths are relative to the `.mgcb` file's directory. The build output goes under `Content/bin/DesktopGL/Content`; intermediate files go under `Content/obj/DesktopGL/...`.

The MonoGame build task automatically runs `dotnet tool restore` and `dotnet mgcb` during build. Running `dotnet tool restore` manually inside the project directory makes first-build behavior easier to diagnose.

## Failure Mode

The deliberate broken file `experiments/e05-content-pipeline/docs/broken-content.mgcb` references `Images/missing_texture.png`. Running it manually:

```bash
cd experiments/e05-content-pipeline
dotnet mgcb /quiet /@:docs/broken-content.mgcb
```

fails with a missing source file error and prints the resolved absolute path. That makes relative path mistakes visible.

One important trap: `MonoGame.Content.Builder.Task` globs every `**/*.mgcb` by default. `e05` sets `EnableMGCBItems=false` and explicitly includes only `Content/Content.mgcb` so the deliberately broken docs file is not built during normal `dotnet build`.

## Current Takeaway

The pipeline is understandable and scriptable, but it is less forgiving than editor import workflows. It rewards keeping asset paths simple, content files near the project root, and build-only failure examples excluded from normal MSBuild content references.
