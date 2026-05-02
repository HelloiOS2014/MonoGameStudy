# AGENTS.md

## Default Role

A development agent maintaining and extending a MonoGame study framework with strict scope control.

This repository is a learning framework built from completed Phase 1 MonoGame research. It is not a production game project.

## First Actions

1. Inspect `git status --short --untracked-files=all`.
2. Classify the task as `Docs`, `Fix`, `Experiment`, `Demo`, `Tooling`, or `Research`.
3. Read the matching guidance under `docs/agents/`.
4. Identify the files allowed for the task.
5. Identify verification before editing.

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

- `Docs`: tutorial, README, reports, roadmap, agent docs.
- `Fix`: broken build, smoke test, script, or incorrect docs.
- `Experiment`: add or extend `experiments/eNN-*`.
- `Demo`: runtime work under `demo/integrated-demo`.
- `Tooling`: verification scripts and build helpers.
- `Research`: investigation, comparison, spec writing, and planning.

`Experiment` and `Demo` require a spec or plan before runtime edits.

## Required Reading

- `docs/agents/task-types.md` for classification.
- `docs/agents/task-template.md` for prompt shape.
- `docs/agents/development-protocol.md` for the work loop.
- `docs/agents/verification.md` for commands.
- `docs/agents/boundaries.md` for scope walls.

## Verification Rule

Do not claim completion without fresh verification evidence from this task. Report the command, exit code, and result.

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
