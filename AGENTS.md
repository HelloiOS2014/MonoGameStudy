# AGENTS.md

## Default Role

A development agent maintaining and extending a MonoGame study framework with strict scope control.

This repository is a learning framework built from completed Phase 1 MonoGame research. It is not a production game project.

## Active Quality Gate

Before claiming v1 tutorial completion, read:

- `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md`

Completion requires evidence-backed scores of at least 95/100 for both content quality and mainline alignment.

## First Actions

1. Inspect `git status --short --untracked-files=all`.
2. Read `course/manifest.json` for lesson-scoped work.
3. For lesson-scoped work, open the matching `course/agent-tasks/<lesson-id>.md`.
4. Use `docs/agents/task-types.md` only after deciding whether the request is lesson-scoped.
5. Read the matching guidance under `docs/agents/`.
6. Identify the files allowed for the task.
7. Identify verification before editing.

For terse prompts such as `continue`, `go`, `干`, or `继续`, inspect git state plus the relevant roadmap or spec before choosing the next action.

## Short Task Format

Preferred Chinese format:

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

English equivalent:

```text
<Type>: <goal>
Acceptance: <observable result, command, or output>
```

If the user gives free-form input, classify it before editing. If acceptance is subjective, stop and propose observable acceptance first.

## Task Types

- Lesson-scoped work: course lessons, course metadata, and matching task packets under `course/`.
- `Docs`: tutorial, README, reports, roadmap, agent docs.
- `Fix`: broken build, smoke test, script, or incorrect docs.
- `Experiment`: add or extend `experiments/eNN-*`.
- `Demo`: runtime work under `demo/integrated-demo`.
- `Tooling`: verification scripts and build helpers.
- `Research`: investigation, comparison, spec writing, and planning.

`Experiment` and `Demo` require a spec or plan before runtime edits.

## Required Reading

- `course/manifest.json` for lesson metadata, code mapping, commands, and evidence state.
- `course/agent-tasks/<lesson-id>.md` for lesson-scoped allowed files and acceptance.
- `docs/agents/task-types.md` for classification.
- `docs/agents/task-template.md` for prompt shape.
- `docs/agents/development-protocol.md` for the work loop.
- `docs/agents/verification.md` for commands.
- `docs/agents/boundaries.md` for scope walls.

## Verification Rule

Do not claim completion without fresh verification evidence from this task. Report the command, exit code, and result.

Run `./tools/check-course.sh` for lesson, manifest, task packet, or tutorial-site navigation changes.

## Git Rules

- Stage only files changed for the current task.
- Do not use destructive git commands.
- Do not commit unrelated or untracked files.
- Keep commits scoped to the current task.

## Stop Conditions

Stop and ask before editing when:

- the task type is ambiguous and the wrong type changes scope,
- the request violates repository boundaries,
- acceptance is subjective or not observable,
- verification fails repeatedly,
- required GUI or network access is unavailable,
- the fix would delete intentional functionality.
