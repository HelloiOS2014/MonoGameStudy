# Platforms And Publishing

Week 2 publishing target: macOS DesktopGL, `osx-x64`, self-contained.

## Command Used

```bash
dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true
```

The publish command provides these publish-relevant choices:

- `RuntimeIdentifier=osx-x64`
- `SelfContained=true`
- `PublishReadyToRun=false` from `E10Publishing.csproj`

`PublishReadyToRun=false` keeps the experiment aligned with the other MonoGame projects and avoids adding another moving part to the first publishing pass.

## Output Layout

Expected output directory:

```text
experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/
```

Important files in that directory:

- `E10Publishing` — native launcher executable.
- `E10Publishing.dll` and `.deps.json` / `.runtimeconfig.json` — managed app metadata.
- `libSDL2-2.0.0.dylib` and `libopenal.dylib` — MonoGame DesktopGL native dependencies.
- .NET runtime libraries — present because this is self-contained.

## Smoke Verification

Run the published executable without `dotnet` on `PATH`:

```bash
env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing
```

Expected stdout includes:

- `Initialize: publishing smoke app for osx-x64.`
- `Smoke: rendered 30 update frames.`
- `Smoke: exit.`

This verification passed for `experiments/e10-publishing` with `E10_SMOKE_EXIT_AFTER_FRAMES=60`.

## Current Takeaway

Publishing is straightforward for a minimal DesktopGL app, but the output is a directory, not a polished `.app` bundle or installer. A later production pass would need packaging, signing/notarization, and a clearer asset/runtime layout.
