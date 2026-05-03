# Agent Guide

## Start Here

Start at root `AGENTS.md` for the session role, startup checklist, verification rule, and stop conditions.

This directory holds detailed operating rules for agents maintaining the MonoGame Study Framework.

Per-lesson task packets live in `course/agent-tasks/`. Use this directory only for general agent operating rules.

Current v1 lesson work is driven by `course/manifest.json`. Treat `docs/tutorial/` as legacy migration source, not the primary course source.

## Task Flow

1. Run `git status --short --untracked-files=all`.
2. Check `course/manifest.json` to decide whether the request maps to a lesson.
3. For lesson-scoped work, open `course/agent-tasks/<lesson-id>.md`.
4. Classify non-lesson work with `docs/agents/task-types.md`.
5. Match or normalize the prompt with `docs/agents/task-template.md`.
6. Follow the work loop in `docs/agents/development-protocol.md`.
7. Select commands from `docs/agents/verification.md`.
8. Check scope walls in `docs/agents/boundaries.md`.

## Files

- `task-types.md`: task classification, planning gates, allowed file areas, gray areas, experiment IDs.
- `task-template.md`: short user task format, `验收:` and `Acceptance:` forms, examples.
- `development-protocol.md`: startup, short-task handling, vague continuation prompts, reporting, stopping.
- `verification.md`: fresh and warmed workspace commands plus task-specific command matrix.
- `boundaries.md`: allowed work, blocked work, spec-required work, and violation response.
- `../../course/agent-tasks/`: lesson-specific task packets with allowed files, blocked files, acceptance, and report format.

## Fast Checks

```bash
git status --short --untracked-files=all
git diff --check
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
bash -n tools/check-course.sh
./tools/check-course.sh
```

These checks do not replace task-specific verification. Use `verification.md` before claiming completion.
