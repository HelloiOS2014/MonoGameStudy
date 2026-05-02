# MonoGame Research Project

A structured, time-boxed research project for learning MonoGame from zero to a small playable demo.

The full design lives in [`docs/superpowers/specs/2026-04-26-monogame-research-design.md`](docs/superpowers/specs/2026-04-26-monogame-research-design.md). Read it first — it defines the scope, the three-week plan, exit criteria, and the conditions under which this research is allowed to abort with a negative report.

## Status

Phase 1 is complete through the integrated demo and technical evaluation. This repo is now a MonoGame learning and evaluation record, not an active game project.

Closeout: [`docs/reports/phase1-closeout.md`](docs/reports/phase1-closeout.md)

Recommended next step: run the separate Godot parity plan in [`docs/superpowers/plans/2026-05-02-godot-parity-demo.md`](docs/superpowers/plans/2026-05-02-godot-parity-demo.md).

## Target environment

- macOS (Intel or Apple Silicon) — Phase 1 platform; the dev machine is Intel `x86_64`.
- .NET SDK 10.0 LTS, pinned by `global.json`.
- MonoGame `mgdesktopgl` template from the latest stable `MonoGame.Templates.CSharp` package.
- IDE: JetBrains Rider or VS Code + C# Dev Kit.

Windows and Linux desktop are nice-to-have. Mobile and consoles are out of scope.

## Setup

```bash
# 1. Install .NET 10 SDK from https://dotnet.microsoft.com/download/dotnet/10.0
# 2. Install the MonoGame project templates
dotnet new install MonoGame.Templates.CSharp
# 3. Verify the toolchain
./tools/check-env.sh
```

The MGCB Editor is **not** installed globally. Each MonoGame project scaffolded from the template ships a `.config/dotnet-tools.json` manifest. Inside any experiment's directory, run `dotnet tool restore` once and then launch the editor with `dotnet mgcb-editor`. Apple Silicon users should verify the editor on the first M-series machine; if arm64 launch fails, try the x64 .NET runtime via Rosetta or hand-edit `.mgcb` files and document the friction.

`tools/check-env.sh` is the source of truth for "is this machine ready". If it exits non-zero, do not start an experiment.

## Repository layout

```
game_demo/
  README.md                  ← you are here
  GameDemo.sln               ← top-level solution; every experiment + the demo joins it
  tools/check-env.sh         ← toolchain sanity check
  docs/                      ← learning notes + final evaluation report
  experiments/eNN-name/      ← one focused experiment per directory
  demo/integrated-demo/      ← Week 3 playable demo
```

Experiment folders are prefixed with `e` so the C# `RootNamespace` derived from the folder is a legal identifier. Each experiment is its own `.csproj`, added to `GameDemo.sln`.

## Creating an experiment

Use a C#-safe project name even though the folder name is kebab-case:

```bash
dotnet new mgdesktopgl -o experiments/e01-game-loop -n E01GameLoop
dotnet sln GameDemo.sln add experiments/e01-game-loop/E01GameLoop.csproj --solution-folder experiments
```

After scaffolding, run `dotnet tool restore` inside the experiment directory before opening MGCB Editor. Keep content paths project-relative, keep asset filename casing exact, and invoke the editor from the experiment root with `dotnet mgcb-editor`.

## Running an experiment

Once an experiment exists:

```bash
dotnet run --project experiments/e01-game-loop
```

Every experiment ships a `README.md` with three sections:

1. **How to run** — the exact command and any content build step.
2. **What you should see** — the observable behavior, including which keys do what.
3. **What was learned** — friction points and reusable takeaways.

If those three sections are not filled in, the experiment is not done.

## Running the integrated demo

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

The evaluation report is in [`docs/reports/monogame-technical-evaluation.md`](docs/reports/monogame-technical-evaluation.md).

Automated smoke:

```bash
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

## Roadmap

| Week | Focus                          | Experiments                                                         |
| ---- | ------------------------------ | ------------------------------------------------------------------- |
| 1    | Framework entry                | `e01-game-loop`, `e02-2d-rendering`, `e03-input`, `e05-content-pipeline` |
| 2    | Capability experiments         | required: `e04-audio`, `e06-camera-and-collision`, `e07-animation`, `e10-publishing`; optional stretch: `e08-basic-3d`, `e09-shader` |
| 3    | Integrated demo + evaluation   | `demo/integrated-demo`, `docs/reports/monogame-technical-evaluation.md` |

Detailed exit criteria for each week are in the design spec.

## Abort conditions

The spec allows Phase 1 to terminate early and write a negative evaluation report if the toolchain or the framework itself blocks progress (see *Abort And Pivot Conditions* in the spec). A clear "do not adopt, here is why" outcome is a valid result.
