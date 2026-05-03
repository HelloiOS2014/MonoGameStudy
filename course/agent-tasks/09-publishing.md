# Agent Task: 09 Publishing

## Task

Maintain or improve the Publishing lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/09-publishing.md`, `course/lessons/09-publishing.mdx`, and `experiments/e10-publishing`.

## Allowed Files

- `course/lessons/09-publishing.mdx`
- `course/agent-tasks/09-publishing.md`
- `course/evidence/09-publishing/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/e10-publishing/**`
- `experiments/e10-publishing.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, `experiments/e10-publishing/**`, or `experiments/e10-publishing.Tests/**` requires an approved spec because it changes tutorial source behavior. This packet does not authorize runtime edits.

## Commands

- `./tools/check-course.sh`
- `dotnet run --project experiments/e10-publishing.Tests/E10Publishing.Tests.csproj`
- `dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current publish smoke behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If publish or published smoke cannot run in the current environment, report the skipped command and run `./tools/check-course.sh` plus the non-GUI test command.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
