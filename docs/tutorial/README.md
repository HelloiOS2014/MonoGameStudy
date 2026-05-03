# MonoGame Tutorial

> Legacy migration source: this Markdown path remains readable during v1 migration, but the canonical v1 course source is `course/` once the quality gate is implemented.

This tutorial teaches the existing MonoGame research repo as a finished learning path. Start with setup, move through one concept per experiment, then finish with the integrated 2D collector demo.

## How To Use This

Read the chapters in order. Each chapter has the same shape: goal, command to run, key files, walkthrough, expected output, common problems, and checkpoint.

Do not expand the demo while reading the tutorial. The point is to understand the finished baseline first, then decide whether the next milestone is editorial hardening, visual evidence, a static site, or a separate tiny game.

## Path

| Order | Chapter | What It Proves |
| --- | --- | --- |
| 0 | [Intro](00-intro.md) | Repo shape and integrated demo goal |
| 1 | [Setup](01-setup.md) | macOS, .NET 10, MonoGame template readiness |
| 2 | [First Window](02-first-window.md) | MonoGame project lifecycle |
| 3 | [Game Loop](03-game-loop.md) | Fixed vs variable timestep |
| 4 | [Rendering](04-rendering.md) | SpriteBatch drawing and batching |
| 5 | [Input](05-input.md) | Polling and edge detection |
| 6 | [Content Pipeline](06-content-pipeline.md) | MGCB asset names and failure modes |
| 7 | [Audio](07-audio.md) | SoundEffect vs Song/MediaPlayer |
| 8 | [Camera, Collision, And Animation](08-camera-collision-animation.md) | Common game-layer systems |
| 9 | [Publishing](09-publishing.md) | Self-contained macOS DesktopGL publish |
| 10 | [Integrated Demo](10-integrated-demo.md) | Complete start/play/win/restart loop |
| Appendix | [Troubleshooting](appendix-troubleshooting.md) | Diagnosis paths for common failures |

## Verify Everything

Run the dry-run script from the repo root:

```bash
./tools/check-tutorial.sh
```

The script opens short-lived MonoGame smoke windows, verifies the deliberate MGCB failure, publishes `e10`, and runs the published executable without `dotnet` on `PATH`.

Use this script as the tutorial acceptance check. If it fails, fix the repo or tutorial before adding any new milestone work.

## Current Validation

Latest recorded validation: [validation-log.md](validation-log.md).

## Planning Boundary

Current completion state and allowed future milestones are defined in [ROADMAP.md](ROADMAP.md).
