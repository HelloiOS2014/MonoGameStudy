# Intro

## Goal

This tutorial turns the existing research repo into a path from zero MonoGame knowledge to a tiny 2D collector demo. You will not build a new game from a blank folder. Instead, you will walk through the finished experiments, understand why each one exists, then run the integrated demo that combines the useful parts.

## What You Will Run

Run the playable demo manually:

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

Run the same demo in automated smoke mode:

```bash
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

## Key Files

- [README.md](../../README.md) - project status, setup, and the high-level map.
- [docs/00-roadmap.md](../00-roadmap.md) - the original research checklist and completion record.
- [docs/reports/monogame-technical-evaluation.md](../reports/monogame-technical-evaluation.md) - final Phase 1 evaluation.
- [demo/integrated-demo/README.md](../../demo/integrated-demo/README.md) - controls, demo genre, and limitations.

## Walkthrough

The repo has two main learning areas:

- `experiments/` contains topic-focused finished states. Each directory isolates one MonoGame concept, such as the game loop, rendering, input, audio, content loading, collision, animation, or publishing.
- `demo/integrated-demo/` is the capstone. It is a small 2D collector that uses the parts that were useful enough to combine: game loop, rendering, input, content-loaded font/audio, collision, simple presentation state, and restart flow.

Use the experiments when you want to understand one subsystem without extra game code around it. Use the integrated demo when you want to see how the pieces fit together in a playable loop.

## Expected Output

Manual mode opens a MonoGame window. Press `Enter` to start, collect the yellow pickups, avoid the red hazards, then restart from the win or loss screen.

Smoke mode should exit by itself. Near the end of stdout, look for:

```text
Phase: won.
Phase: restarted.
Smoke: exit.
```

## Common Problems

- This tutorial assumes macOS DesktopGL. Windows and Linux may work, but they were not the Phase 1 target.
- The first restore or build can take minutes because NuGet packages, MonoGame tools, and generated content may need to be prepared.
- A normal `dotnet run` opens a GUI window. Use the smoke command when you want a non-interactive confidence check.

## Checkpoint

You are ready for the next chapter when you can explain the difference between `experiments/` and `demo/` in this repo.
