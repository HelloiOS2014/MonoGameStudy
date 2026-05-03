# Agent Task: 04 Rendering

## Task

Maintain or improve the Rendering lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/04-rendering.md`, `course/lessons/04-rendering.mdx`, and `experiments/e02-2d-rendering`.

## Allowed Files

- `course/lessons/04-rendering.mdx`
- `course/agent-tasks/04-rendering.md`
- `course/evidence/04-rendering/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/e02-2d-rendering/**`
- `experiments/e02-2d-rendering.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, `experiments/e02-2d-rendering/**`, or `experiments/e02-2d-rendering.Tests/**` requires an approved spec because it changes tutorial source behavior. This packet does not authorize runtime edits.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj`

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
