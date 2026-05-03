# Agent Task: 03 Game Loop

## Task

Maintain or improve the Game Loop lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/03-game-loop.md`, `course/lessons/03-game-loop.mdx`, `experiments/e01-game-loop`, and `experiments/e01-game-loop.Tests`.

## Allowed Files

- `course/lessons/03-game-loop.mdx`
- `course/agent-tasks/03-game-loop.md`
- `course/evidence/03-game-loop/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/e01-game-loop/**`
- `experiments/e01-game-loop.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, `experiments/e01-game-loop/**`, or `experiments/e01-game-loop.Tests/**` requires an approved spec because it changes tutorial source behavior. This packet does not authorize runtime edits.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current experiment behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run `./tools/check-course.sh` plus the non-GUI test command.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
