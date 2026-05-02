# MonoGame Tutorial Design

## Purpose

Turn this repository from a research record into a usable beginner tutorial without changing the existing experiments or demo behavior.

The tutorial should teach a C# developer how to get from a fresh MonoGame DesktopGL setup to a small playable 2D demo. It should reuse the finished `experiments/` and `demo/integrated-demo/` projects as reference implementations, not introduce new game features.

## Audience

The reader:

- Knows basic C# syntax and can run terminal commands.
- Has not used MonoGame before.
- Wants a practical path to a small 2D game skeleton.
- Benefits from explicit commands, expected output, file paths, and troubleshooting notes.

The reader is not expected to understand content pipelines, game loops, sprite batching, input edge detection, audio APIs, collision math, animation state machines, or publishing before starting.

## Format

First version is Markdown-only under `docs/tutorial/`.

No tutorial website, Astro migration, screenshots, videos, or generated assets are required for the first version. The structure should make a later website migration straightforward by keeping each chapter self-contained and consistently formatted.

## Tutorial Structure

Create these files:

```text
docs/tutorial/
  00-intro.md
  01-setup.md
  02-first-window.md
  03-game-loop.md
  04-rendering.md
  05-input.md
  06-content-pipeline.md
  07-audio.md
  08-camera-collision-animation.md
  09-publishing.md
  10-integrated-demo.md
  appendix-troubleshooting.md
```

Add an index section to the root `README.md` that points to `docs/tutorial/00-intro.md`.

## Chapter Template

Every chapter must use this structure:

1. `# <chapter title>`
2. `## Goal`
3. `## What You Will Run`
4. `## Key Files`
5. `## Walkthrough`
6. `## Expected Output`
7. `## Common Problems`
8. `## Checkpoint`

The tutorial should prefer concrete command examples over prose-only explanation. Every command should be copy-pasteable from the repository root unless the chapter explicitly says to `cd` elsewhere.

## Chapter Scope

### 00 Intro

Explain what the tutorial builds and how this repository is organized. Introduce the finished demo and the experiment-per-topic structure.

### 01 Setup

Explain the target environment, `global.json`, MonoGame templates, `tools/check-env.sh`, and the expected green setup output. Mention that this project targets macOS DesktopGL and .NET 10.

### 02 First Window

Use `experiments/e01-game-loop` as the first runnable project. Explain the MonoGame template shape: `Program.cs`, `Game1.cs`, `Initialize`, `LoadContent`, `Update`, and `Draw`.

### 03 Game Loop

Explain fixed vs variable timestep using `e01-game-loop`, including the automated smoke variables and stdout frame logs.

### 04 Rendering

Explain `SpriteBatch`, batched vs unbatched draw, runtime textures, `SpriteFont`, and the 1000 sprite experiment in `e02-2d-rendering`.

### 05 Input

Explain keyboard, mouse, optional gamepad polling, edge detection, and movement logic from `e03-input`.

### 06 Content Pipeline

Explain `Content.mgcb`, `.xnb` output, asset logical names, `Content.Load<T>`, SpriteFont, PNG, WAV, and the deliberate broken MGCB example from `e05-content-pipeline`.

### 07 Audio

Explain `SoundEffect` vs `Song`/`MediaPlayer` using `e04-audio`. Call out that `SoundEffect` is suitable for short effects and `MediaPlayer` is global music state.

### 08 Camera, Collision, Animation

Combine `e06-camera-and-collision` and `e07-animation`. Explain camera transforms, AABB, circle collision, debug outlines, frame animation, and idle/walk/jump transitions.

### 09 Publishing

Explain `dotnet publish`, `osx-x64`, self-contained output, minimal PATH smoke, and the difference between a runnable publish directory and a polished `.app`/installer using `e10-publishing`.

### 10 Integrated Demo

Explain how `demo/integrated-demo` combines the earlier ideas into start/play/win/loss/restart. The chapter should describe how to run the interactive demo and the automated smoke.

### Appendix Troubleshooting

Collect practical failure modes found during the project:

- `dotnet` SDK version mismatch.
- MonoGame template missing.
- NuGet/network restore failure.
- MGCB tool restore issues.
- `.mgcb` globbing broken files.
- Missing `.xnb` at runtime.
- SpriteFont/font availability.
- macOS DesktopGL window/smoke quirks.
- Self-contained publish layout confusion.

## References To Existing Code

Each chapter should link to real files and directories:

- `experiments/e01-game-loop`
- `experiments/e02-2d-rendering`
- `experiments/e03-input`
- `experiments/e05-content-pipeline`
- `experiments/e04-audio`
- `experiments/e06-camera-and-collision`
- `experiments/e07-animation`
- `experiments/e10-publishing`
- `demo/integrated-demo`

Do not duplicate large source files into tutorial prose. Show short excerpts only when they explain a concept. Prefer path references plus the command to run the matching project.

## Verification

The tutorial implementation is complete only when:

- `docs/tutorial/00-intro.md` through `docs/tutorial/10-integrated-demo.md` exist.
- `docs/tutorial/appendix-troubleshooting.md` exists.
- Every chapter has all eight required sections from the chapter template.
- Every chapter has at least one runnable command or explicitly says why it is conceptual.
- Every experiment/demo command referenced in the tutorial is already backed by an existing project in this repository.
- Root `README.md` links to the tutorial entry point.
- `git diff --check` exits 0.
- `dotnet build GameDemo.sln --no-restore -m:1` exits 0.
- Integrated demo smoke logs `Phase: won.`, `Phase: restarted.`, and `Smoke: exit.`

## Non-Goals

The first tutorial version will not:

- Build a tutorial website.
- Add screenshots or videos.
- Add 3D or shader chapters.
- Refactor the code into a reusable engine package.
- Add new MonoGame functionality.
- Rewrite the existing research report.
- Remove or rename existing experiments.

## Rollout

Work should land in small documentation commits:

1. Tutorial skeleton and README entry point.
2. Setup and first-window chapters.
3. Game loop, rendering, input chapters.
4. Content pipeline and audio chapters.
5. Camera/collision/animation and publishing chapters.
6. Integrated demo and troubleshooting appendix.
7. Final verification pass.

Each commit should keep the repository buildable.
