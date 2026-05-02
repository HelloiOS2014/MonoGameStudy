# Publishing

Previous: [Camera, Collision, And Animation](08-camera-collision-animation.md) | Next: [Integrated Demo](10-integrated-demo.md)

## Goal

Publish a self-contained macOS DesktopGL build and verify that the executable does not depend on `dotnet` being on `PATH`.

## What You Will Run

```bash
dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false
env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing
```

## Key Files

- [experiments/e10-publishing/E10Publishing.csproj](../../experiments/e10-publishing/E10Publishing.csproj) - publish-related project settings.
- [experiments/e10-publishing/README.md](../../experiments/e10-publishing/README.md) - experiment run notes.
- [docs/04-platforms-and-publishing.md](../04-platforms-and-publishing.md) - Phase 1 publishing findings.

## Walkthrough

The publish command chooses a runtime identifier:

```text
osx-x64
```

It also uses `--self-contained true`, so the output includes the .NET runtime files needed by the app. The experiment keeps `PublishReadyToRun=false` to reduce variables in this first publishing pass.

Expected output directory:

```text
experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/
```

That directory contains the native launcher `E10Publishing`, managed metadata files, MonoGame DesktopGL native dependencies such as SDL/OpenAL dylibs, and .NET runtime libraries.

The smoke command uses a minimal `PATH`:

```bash
env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" ...
```

That proves the published executable can start without finding `dotnet` from the SDK. It is still just a publish directory, not a signed `.app` bundle, installer, or notarized macOS release.

## Expected Output

The published smoke should print:

```text
Smoke: rendered 30 update frames.
Smoke: exit.
```

## Common Problems

- The publish directory is large because self-contained output includes runtime files.
- Native dependency files are expected for DesktopGL.
- Signing, notarization, installer creation, and a polished `.app` bundle are out of scope for this tutorial.
- Publishing for `osx-arm64` is a separate validation step; this repo's Phase 1 publish check used `osx-x64`.

## Checkpoint

You are ready when you can find the published executable and run it without `dotnet` on `PATH`.
