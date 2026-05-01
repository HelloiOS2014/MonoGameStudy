# MonoGame Research Project Design

Date: 2026-04-26
Status: Approved for planning

## Purpose

This repository is a structured research project for learning MonoGame from zero to the point where a small, complete game demo can be built independently.

The project is not intended to claim full MonoGame mastery in the first pass. The intended first milestone is practical competence:

- Understand what MonoGame is and how it differs from editor-driven engines such as Unity and Godot.
- Build and run MonoGame projects from scratch.
- Understand the game lifecycle: `Initialize`, `LoadContent`, `Update`, and `Draw`.
- Use core systems for rendering, input, audio, resources, camera behavior, collision, basic 3D, shaders, and publishing.
- Produce a playable integrated demo.
- Write a technical evaluation explaining when MonoGame is a good fit and when it is not.

## Scope

The project will cover both 2D and basic 3D. It will not assume MonoGame is only for 2D, but the first practical path will lean on 2D because it is the fastest way to build correct intuition for the framework.

Target platform for Phase 1 is macOS DesktopGL only. Windows and Linux desktop validation are nice-to-have; mobile and console targets are explicitly deferred.

The comparison set used in the final evaluation is fixed in advance: Unity, Godot, raylib, and hand-rolled engines on top of SDL or similar. This is the same list referenced by the evaluation report, and no additional engines should be added mid-cycle.

### Out Of Scope (this phase)

- Deep MonoGame source-code analysis.
- Commercial-grade engine architecture.
- Advanced 3D rendering pipelines (PBR, deferred, post-processing stacks).
- Publishing to Windows, Linux, mobile, or consoles.
- Custom editor tooling, asset pipelines beyond stock MGCB.
- Networking, save systems, localization.

These belong to follow-up phases, listed at the end of this document.

## Environment And Toolchain

This is a macOS-first project. The following toolchain is required before Week 1 can start:

- .NET SDK 10.0 LTS (`dotnet --version` must report 10.x). .NET 10 became LTS in November 2025 and is supported through November 2028. .NET 8 still works for MonoGame but exits support in November 2026, so this project pins .NET 10. The pin is enforced by a `global.json` at the repo root.
- MonoGame project templates: `dotnet new install MonoGame.Templates.CSharp`. Use the latest stable package unless a specific experiment needs preview behavior. MonoGame 3.8.4.1 targets .NET 8 but is compatible with newer .NET target frameworks, including .NET 10; avoid requiring 3.8.5 preview packages for Phase 1.
- Default project template: `mgdesktopgl` (cross-platform OpenGL backend).
- IDE: JetBrains Rider or VS Code with the C# Dev Kit. Visual Studio for Mac is not assumed.
- MGCB Editor: project-local `dotnet` tool. The MonoGame templates already include a `.config/dotnet-tools.json` manifest that lists `dotnet-mgcb-editor`. After scaffolding a project, run `dotnet tool restore` once and then launch with `dotnet mgcb-editor` from the project root. The older `dotnet tool install -g dotnet-mgcb-editor` + `mgcb-editor --register` flow is no longer the documented path and should not be used.
- Apple Silicon caveat (not relevant to the current dev machine, which is Intel `x86_64`, but kept for portability): verify on the first M-series Mac. If MGCB Editor fails under arm64 .NET, try the x64 .NET runtime via Rosetta; if that still blocks work, hand-edit `.mgcb` files and document the friction.

A `tools/check-env.sh` script verifies these prerequisites and prints actionable errors. Spec compliance is checked by running this script, not by inspection.

## Recommended Learning Strategy

The project will use a course-like structure:

1. Learn the framework concepts.
2. Build small focused experiments.
3. Combine the useful pieces into a complete demo.
4. Evaluate MonoGame as a technology choice.

This approach is preferred over starting with one large game immediately. MonoGame is a framework rather than a complete editor engine, so isolated experiments will expose the framework's real workflow and friction points faster.

## Repository Structure

```text
game_demo/
  README.md
  GameDemo.sln
  tools/
    check-env.sh
  docs/
    00-roadmap.md
    01-monogame-overview.md
    02-engine-comparison.md
    03-content-pipeline.md
    04-platforms-and-publishing.md
    reports/
      monogame-technical-evaluation.md

  experiments/
    e01-game-loop/
    e02-2d-rendering/
    e03-input/
    e04-audio/
    e05-content-pipeline/
    e06-camera-and-collision/
    e07-animation/
    e08-basic-3d/
    e09-shader/
    e10-publishing/

  demo/
    integrated-demo/
```

The `docs` directory holds learning notes and final evaluation material.

The `experiments` directory holds focused MonoGame experiments. Each experiment is its own `.csproj`, grouped under a single top-level `GameDemo.sln` solution. Folder names are prefixed with `e` (rather than starting with a digit) so that the C# `RootNamespace` derived from the folder is a legal identifier.

Scaffold experiments with a C#-safe project name and then add them to the matching solution folder, for example:

```bash
dotnet new mgdesktopgl -o experiments/e01-game-loop -n E01GameLoop
dotnet sln GameDemo.sln add experiments/e01-game-loop/E01GameLoop.csproj --solution-folder experiments
```

For content experiments, run `dotnet tool restore` from the experiment directory before launching `dotnet mgcb-editor`. Keep `Content.mgcb` paths project-relative and case-exact so the same assets behave on both default macOS volumes and case-sensitive filesystems.

Each experiment must contain a `README.md` with three sections:

- **How to run** — exact `dotnet run` invocation and any required content build step.
- **What you should see** — the observable behavior, including which keys do what.
- **What was learned** — friction points, reusable takeaways, and references to docs notes.

The `demo/integrated-demo` directory holds the final small playable project, also part of `GameDemo.sln`.

## Three-Week Research Plan

### Week 1: Framework Entry

Goal: build a working mental model of MonoGame and run basic projects.

Experiments owned by Week 1:

- `e01-game-loop` — minimal `Game1` project, lifecycle prints, fixed vs variable timestep toggle.
- `e02-2d-rendering` — `SpriteBatch` draw, texture and `SpriteFont` loading, simple batched-vs-unbatched performance comparison.
- `e03-input` — keyboard, mouse, controller polling, edge-detection helpers.
- `e05-content-pipeline` — adding assets via MGCB, build-time errors, runtime asset reload friction.

Notes to write under `docs/`:

- `01-monogame-overview.md`
- `02-engine-comparison.md` (initial draft, comparison set fixed in Scope)
- `03-content-pipeline.md`

Exit criteria for Week 1:

- I can scaffold a new MonoGame project from the template without consulting external docs.
- I can load a PNG and a font and render them at a known position.
- I can articulate, in writing, what `Initialize` / `LoadContent` / `Update` / `Draw` each own and what runs on which thread.

### Week 2: Capability Experiments

Goal: test important MonoGame subsystems in isolation.

Week 2 runs **four required experiments** plus **two optional stretch experiments** (3D and shader). The earlier draft made all six required, which is too aggressive for one solo week. The stretch pair is pulled in only if the four required experiments finish ahead of schedule; otherwise they move to follow-up.

Required experiments:

- `e04-audio` — `SoundEffect` vs `Song`, mixing, latency observations.
- `e06-camera-and-collision` — translating/zooming camera, AABB and circle collision, 2D viewport behavior.
- `e07-animation` — frame-based sprite animation and a small state machine (idle/walk/jump).
- `e10-publishing` — `dotnet publish` for macOS DesktopGL, app bundle layout, smoke test of the published binary.

Optional stretch experiments (do only after the required four pass their acceptance checks):

- `e08-basic-3d` — load a model, basic camera, `BasicEffect` lighting.
- `e09-shader` — minimal custom HLSL → MGFX shader, parameter passing.

Each experiment's README must include the three required sections (How to run / What you should see / What was learned) and one acceptance check that is observable from running the program — for example, `e02-2d-rendering` should render at least 1000 sprites at 60fps with a key to toggle batching.

Exit criteria for Week 2:

- The four required experiments build and run on macOS DesktopGL.
- Each completed experiment's README is non-empty with the three required sections.
- A draft "what's painful" list exists in `docs/02-engine-comparison.md` with concrete examples drawn from these experiments.
- If the optional 3D/shader experiments were not done, that decision is recorded in `docs/00-roadmap.md` (deferred vs dropped).

### Week 3: Integrated Demo And Evaluation

Goal: combine the useful parts into a playable demo and write a practical evaluation.

The integrated demo should be small. Its exact genre is chosen after the experiments reveal MonoGame's development feel. The default recommendation is a 2D or 2.5D game. A basic 3D or shader scene as a validation harness is **encouraged but not mandatory** — include it if the optional Week 2 stretch experiments succeeded and the cost is low; otherwise the demo can stay 2D and the report can note that 3D/shader were deferred.

Expected output:

- `demo/integrated-demo` is runnable via `dotnet run --project demo/integrated-demo`. The `.csproj` is registered in `GameDemo.sln` for IDE convenience, but `dotnet run` is always invoked with `--project` because solution-level run is not reliable.
- Top-level `README.md` explains setup, the env check script, and how to run each experiment and the demo.
- `docs/reports/monogame-technical-evaluation.md` explains strengths, weaknesses, risks, and fit compared with the comparison set fixed in Scope (Unity, Godot, raylib, hand-rolled engines).

Exit criteria for Week 3:

- The integrated demo can be played start-to-finish through its core loop.
- The evaluation report cites specific experiments as evidence for each claim.
- I can answer, in one paragraph, "should I keep using MonoGame for my next project?"

## Evaluation Criteria

The research is successful when the repository can answer these questions with working evidence:

- How do you create, run, and structure a MonoGame project?
- How does the MonoGame game loop work in practice?
- How are content assets loaded and managed?
- How much code is required for common rendering, input, audio, and camera tasks?
- What is straightforward in MonoGame?
- What becomes manual or costly compared with editor-driven engines?
- What kind of small game can be built comfortably after this learning cycle?
- Is MonoGame worth deeper study for the user's future game-development goals?

## Testing And Verification

Each code milestone is verified by running the relevant project locally on macOS DesktopGL.

For experiments, verification has three concrete requirements:

1. `dotnet build` succeeds with no warnings other than known-MonoGame-template noise.
2. `dotnet run` launches a window and the experiment's README "What you should see" is observably true.
3. The README's stated acceptance check passes (e.g. a frame-time number, a key toggling visible behavior).

For the integrated demo, verification means the game can be launched from the command line via `dotnet run --project demo/integrated-demo`, played through its core loop, and exited cleanly.

Documentation must be checked for consistency with the current code before the final evaluation is written. The evaluation report must not make claims that are not backed by a specific experiment or demo behavior.

## Abort And Pivot Conditions

This is a research project. The following conditions trigger early termination of Phase 1, with the remaining time redirected to writing a negative evaluation report:

- The MonoGame templates do not run on the target macOS / .NET 10 environment after a reasonable bootstrap effort.
- MGCB Editor is unusable on Apple Silicon and editing `.mgcb` by hand is also blocked (rather than merely awkward).
- A blocking bug in MonoGame itself prevents Week 1 experiments from completing and there is no near-term workaround.
- After Week 2, the friction list is severe enough that Week 3's integrated demo would not be honest evidence of competence.

In any of these cases, the research is still considered successful: a clear "do not adopt, here is why" report is a valid outcome and must be written with the same rigor as the positive case.

## Follow-Up Phase Candidates

After this phase, possible deeper tracks include:

- MonoGame source-code reading.
- Advanced 3D rendering and shader work.
- ECS or custom game architecture on top of MonoGame.
- Asset tooling and editor workflow.
- Cross-platform publishing.
- Performance profiling.
