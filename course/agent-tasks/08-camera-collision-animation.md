# Agent Task: 08 Camera Collision Animation

## Task

Maintain or improve the Camera, Collision, And Animation lesson and expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/08-camera-collision-animation.md`, `course/lessons/08-camera-collision-animation.mdx`, `experiments/e06-camera-and-collision`, and `experiments/e07-animation`.

## Allowed Files

- `course/lessons/08-camera-collision-animation.mdx`
- `course/agent-tasks/08-camera-collision-animation.md`
- `course/evidence/08-camera-collision-animation/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/e06-camera-and-collision/**`
- `experiments/e06-camera-and-collision.Tests/**`
- `experiments/e07-animation/**`
- `experiments/e07-animation.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any change under `experiments/e06-camera-and-collision/**`, `experiments/e06-camera-and-collision.Tests/**`, `experiments/e07-animation/**`, or `experiments/e07-animation.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e06-camera-and-collision.Tests/E06CameraAndCollision.Tests.csproj`
- `dotnet run --project experiments/e07-animation.Tests/E07Animation.Tests.csproj`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current experiment behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run `./tools/check-course.sh` plus the non-GUI test commands.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
