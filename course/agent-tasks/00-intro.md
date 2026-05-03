# Agent Task: 00 Intro

## Task

Maintain or improve the Intro lesson and expected state without changing runtime behavior.

## Context

This lesson maps to `docs/tutorial/00-intro.md`, `course/lessons/00-intro.mdx`, and the completed experiment/demo structure.

## Allowed Files

- `course/lessons/00-intro.mdx`
- `course/agent-tasks/00-intro.md`
- `course/evidence/00-intro/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, or `experiments/**` requires an approved spec because this lesson only orients learners to existing runtime artifacts. This packet does not authorize runtime edits.

## Commands

- `./tools/check-course.sh`
- `dotnet run --project demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current integrated demo behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run `./tools/check-course.sh` plus the non-GUI test command.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
