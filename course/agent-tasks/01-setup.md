# Agent Task: 01 Setup

## Task

Maintain or improve the Setup lesson and expected state without changing repository toolchain behavior.

## Context

This lesson maps to `docs/tutorial/01-setup.md`, `course/lessons/01-setup.mdx`, `global.json`, `tools/check-env.sh`, and `GameDemo.sln`.

## Allowed Files

- `course/lessons/01-setup.mdx`
- `course/agent-tasks/01-setup.md`
- `course/evidence/01-setup/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `demo/integrated-demo.Tests/**`
- `experiments/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any runtime change under `demo/integrated-demo/**`, `demo/integrated-demo.Tests/**`, or `experiments/**` requires an approved spec. This setup packet does not authorize runtime edits.

Any change to `tools/check-env.sh`, `global.json`, or solution-level toolchain behavior requires an approved spec and must also be explicitly allowed by this packet or the manifest because it changes bootstrap behavior.

## Commands

- `./tools/check-course.sh`
- `./tools/check-env.sh`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected state matches setup command behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If `./tools/check-env.sh` fails because the local machine is missing a tool, report the missing tool and do not edit unrelated files.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any setup checks that could not be completed locally.
