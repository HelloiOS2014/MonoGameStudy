# Agent Task: 01 Setup

## Task

Maintain or improve the setup lesson without changing repository toolchain behavior.

## Context

This lesson maps to repository bootstrap files: `global.json`, `tools/check-env.sh`, and `GameDemo.sln`.

## Allowed Files

- `course/lessons/01-setup.mdx`
- `course/agent-tasks/01-setup.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/**`

## Spec Required

Any change to `tools/check-env.sh` or `global.json` requires an approved spec because it changes bootstrap behavior.

## Commands

- `./tools/check-env.sh`
- `./tools/check-course.sh`

## Acceptance

- Human lesson still contains every section required by the manifest.
- Agent task packet still contains every required operating section.
- Manifest paths still resolve.
- Verification commands are reported with results.

## Failure Handling

If `./tools/check-env.sh` fails because the local machine is missing a tool, report the missing tool and do not edit unrelated files.

## Report Format

Report changed files, verification commands, and any setup checks that could not be completed locally.
