# Agent Task: 06 Content Pipeline

## Task

Maintain or improve the Content Pipeline lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/06-content-pipeline.md`, `course/lessons/06-content-pipeline.mdx`, and `experiments/e05-content-pipeline`.

## Allowed Files

- `course/lessons/06-content-pipeline.mdx`
- `course/agent-tasks/06-content-pipeline.md`
- `course/evidence/06-content-pipeline/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/e05-content-pipeline/**`
- `experiments/e05-content-pipeline.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any change under `experiments/e05-content-pipeline/**` or `experiments/e05-content-pipeline.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e05-content-pipeline.Tests/E05ContentPipeline.Tests.csproj`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current experiment behavior and deliberate MGCB failure.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run `./tools/check-course.sh` plus the non-GUI test command.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
