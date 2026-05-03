# Troubleshooting

Previous: [Integrated Demo](10-integrated-demo.md) | Next: [Tutorial Index](README.md)

## Goal

Diagnose common failures from this repo without guessing.

## What You Will Run

```bash
./tools/check-env.sh
dotnet build GameDemo.sln --no-restore -m:1
git diff --check
```

## Key Files

- [tools/check-env.sh](../../tools/check-env.sh) - environment diagnostic.
- [global.json](../../global.json) - SDK selection.
- [.gitignore](../../.gitignore) - ignored build/content/publish outputs.
- [docs/03-content-pipeline.md](../03-content-pipeline.md) - MGCB workflow notes.
- [docs/04-platforms-and-publishing.md](../04-platforms-and-publishing.md) - Publish layout notes.
- [docs/reports/phase1-closeout.md](../reports/phase1-closeout.md) - final verification commands.

## Walkthrough

Use the symptom to choose the smallest diagnostic path below.

### SDK mismatch

Run `dotnet --version` from the repo root. This repo expects .NET 10 selected through `global.json`. If another major version appears, install the .NET 10 SDK and rerun from the repo root.

### Missing template

Run `dotnet new list mgdesktopgl`. If `mgdesktopgl` is missing, install templates:

```bash
dotnet new install MonoGame.Templates.CSharp
```

Then rerun `./tools/check-env.sh`.

If `dotnet new list mgdesktopgl` prints a template-cache permission warning but still lists `mgdesktopgl` and exits 0, the template is usable. Fix `~/.templateengine` permissions only if the warning becomes a blocking error.

### NuGet or network restore

Commands with `--no-restore` assume packages are already restored. On a fresh machine, run the same `dotnet build` or `dotnet run` command once without `--no-restore`.

### MGCB restore

MonoGame projects use project-local tools. If content build tooling is missing, run from the project directory:

```bash
dotnet tool restore
```

Then build again.

If MGCB or `dotnet tool restore` prints a cache-related warning but exits 0, treat it as non-blocking and continue to the build step. If the next content build fails, delete the affected project's `bin/` and `obj/` directories, rerun `dotnet tool restore` from that project directory, then rebuild without `--no-restore` once.

### `.mgcb` globbing

MonoGame build targets can pick up extra `.mgcb` files. `e05` sets `EnableMGCBItems=false` so the deliberate failure file under `docs/` is not built during normal project builds.

### missing `.xnb`

If `Content.Load<T>` fails with a missing `.xnb`, first check whether the source asset is listed in `Content.mgcb`, then check the logical name passed to `Content.Load<T>`. Runtime names omit source extensions.

### SpriteFont and font availability

If a `SpriteFont` fails, inspect the `.spritefont` file and rerun the content build. Font availability can differ between machines, so keep tutorial fonts simple and verify on the target macOS machine.

### macOS DesktopGL smoke quirks

GUI smoke runs still open a DesktopGL window. If a smoke command hangs, prefer the env-var smoke path documented in each chapter and avoid synthetic keyboard events.

macOS may also print native messages such as `TSM AdjustCapsLockLEDForKeyTransitionHandling`, `IMKCFRunLoopWakeUpReliable`, or `CSSM_ModuleLoad()`. Treat them as noise when the command exits 0 and the expected smoke lines appear.

### Full tutorial dry run

Run `./tools/check-tutorial.sh` from the repo root to verify the tutorial path end-to-end. The script intentionally includes one expected MGCB failure and treats it as success only when the output mentions `missing_texture.png`.

### Publish layout confusion

`dotnet publish` outputs a directory, not a signed `.app`. For `e10`, the executable is under:

```text
experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing
```

## Expected Output

Healthy checks look like:

```text
Environment ready.
```

and:

```text
0 warnings
0 errors
```

`git diff --check` should print no output.

## Common Problems

- Rerun restore without `--no-restore` when using a fresh machine, a new SDK install, or cleared NuGet caches.
- Use `./tools/check-env.sh` before debugging MonoGame code; it separates toolchain failures from project failures.
- If a publish smoke fails, confirm you are running the executable from the current RID's publish directory.

## Checkpoint

You are ready when you can pick the right diagnostic command for a setup, build, content, smoke, or publish failure.
