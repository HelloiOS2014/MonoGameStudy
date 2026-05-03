# Agent Task: 05 Input

## Task

Maintain or improve the Input lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/05-input.md`, `course/lessons/05-input.mdx`, and `experiments/e03-input`.

## Allowed Files

- `course/lessons/05-input.mdx`
- `course/agent-tasks/05-input.md`
- `course/evidence/05-input/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/e03-input/**`
- `experiments/e03-input.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any change under `experiments/e03-input/**` or `experiments/e03-input.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e03-input.Tests/E03Input.Tests.csproj`

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
