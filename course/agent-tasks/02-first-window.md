# Agent Task: 02 First Window

## Task

Maintain or improve the First Window lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/02-first-window.md`, `course/lessons/02-first-window.mdx`, and `experiments/e01-game-loop`.

## Allowed Files

- `course/lessons/02-first-window.mdx`
- `course/agent-tasks/02-first-window.md`
- `course/evidence/02-first-window/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/e01-game-loop/**`
- `experiments/e01-game-loop.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any change under `experiments/e01-game-loop/**` or `experiments/e01-game-loop.Tests/**` requires an approved spec because it changes tutorial source behavior.

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
