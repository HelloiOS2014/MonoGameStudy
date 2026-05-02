# Agent Guide

## Start Here

Start at root `AGENTS.md` for the session role, startup checklist, verification rule, and stop conditions.

This directory holds detailed operating rules for agents maintaining the MonoGame Study Framework.

## Task Flow

1. Run `git status --short --untracked-files=all`.
2. Classify the request with `docs/agents/task-types.md`.
3. Match or normalize the prompt with `docs/agents/task-template.md`.
4. Follow the work loop in `docs/agents/development-protocol.md`.
5. Select commands from `docs/agents/verification.md`.
6. Check scope walls in `docs/agents/boundaries.md`.

## Files

- `task-types.md`: task classification, planning gates, allowed file areas, gray areas, experiment IDs.
- `task-template.md`: short user task format, `验收:` and `Acceptance:` forms, examples.
- `development-protocol.md`: startup, short-task handling, vague continuation prompts, reporting, stopping.
- `verification.md`: fresh and warmed workspace commands plus task-specific command matrix.
- `boundaries.md`: allowed work, blocked work, spec-required work, and violation response.

## Fast Checks

```bash
git status --short --untracked-files=all
git diff --check
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
```

These checks do not replace task-specific verification. Use `verification.md` before claiming completion.
