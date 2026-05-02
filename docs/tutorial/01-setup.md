# Setup

Previous: [Intro](00-intro.md) | Next: [First Window](02-first-window.md)

## Goal

Prepare the machine for .NET 10 and MonoGame DesktopGL, then verify that this repo can create and run the same project shape used by the experiments.

## What You Will Run

```bash
dotnet --version
dotnet new list mgdesktopgl
./tools/check-env.sh
```

## Key Files

- [global.json](../../global.json) - pins the SDK family used by this repo.
- [tools/check-env.sh](../../tools/check-env.sh) - validates macOS, .NET, `global.json`, and the MonoGame template.
- [.editorconfig](../../.editorconfig) - keeps generated and hand-written files formatted consistently.
- [.gitignore](../../.gitignore) - keeps build output, content output, and publish output out of Git.
- [README.md](../../README.md) - source of truth for current setup instructions.

## Walkthrough

`global.json` pins the repo to .NET 10. The checked-in value is `10.0.100` with `rollForward` set to `latestFeature`, so a newer installed 10.0 feature band can be selected. On this machine, `dotnet --version` reports:

```text
10.0.107
```

The MonoGame project template used by this repo is `mgdesktopgl` from `MonoGame.Templates.CSharp`. Confirm it is visible:

```bash
dotnet new list mgdesktopgl
```

`tools/check-env.sh` is the repo-level readiness check. It verifies that the current OS is macOS, `global.json` exists and pins .NET 10, the selected SDK is .NET 10, and the `mgdesktopgl` template is installed.

The MGCB Editor is project-local in current MonoGame templates. Each MonoGame project has its own `.config/dotnet-tools.json` after scaffolding, so content tooling is restored per project with `dotnet tool restore`.

## Expected Output

On this machine, `dotnet --version` returns:

```text
10.0.107
```

A healthy environment check ends with:

```text
Environment ready.
```

## Common Problems

- Missing SDK: install the .NET 10 SDK, then rerun `dotnet --version` from the repo root.
- Wrong SDK: make sure `global.json` is at the repo root and that a .NET 10 SDK is installed.
- Missing `mgdesktopgl`: install templates with `dotnet new install MonoGame.Templates.CSharp`.
- NuGet or network failures: rerun restore/build when the network is available; commands with `--no-restore` assume packages are already present.
- Apple Silicon MGCB Editor friction: first verify `dotnet mgcb-editor` inside a scaffolded project; if arm64 launch fails, try x64 .NET through Rosetta or hand-edit `.mgcb` files.

## Checkpoint

You are ready when `./tools/check-env.sh` exits with code 0.
