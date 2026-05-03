# Agent Task: 03 Game Loop

## Task

Maintain or improve the Game Loop lesson without changing experiment behavior.

## Context

This lesson maps to `experiments/e01-game-loop` and `experiments/e01-game-loop.Tests`.

## Allowed Files

- `course/lessons/03-game-loop.mdx`
- `course/agent-tasks/03-game-loop.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `course/lessons/01-setup.mdx`
- `course/agent-tasks/01-setup.md`

## Spec Required

Any change under `experiments/e01-game-loop/**` or `experiments/e01-game-loop.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj`
- `./tools/check-course.sh`

## Acceptance

- Human lesson still contains every section required by the manifest.
- Agent task packet still contains every required operating section.
- Manifest paths still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run non-GUI checks.

## Report Format

Report changed files, verification commands, and any unverified visual evidence.
