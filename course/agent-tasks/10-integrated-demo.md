# Agent Task: 10 Integrated Demo

## Task

Maintain or improve the Integrated Demo lesson and expected state without changing capstone behavior.

## Context

This lesson maps to `docs/tutorial/10-integrated-demo.md`, `course/lessons/10-integrated-demo.mdx`, and `demo/integrated-demo`.

## Allowed Files

- `course/lessons/10-integrated-demo.mdx`
- `course/agent-tasks/10-integrated-demo.md`
- `course/evidence/10-integrated-demo/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, or `experiments/**` requires an approved spec because the integrated demo is capstone evidence, not a product game expansion target. This packet does not authorize runtime edits.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
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
