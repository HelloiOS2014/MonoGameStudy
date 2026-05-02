# Development Protocol

## Startup

1. Read `AGENTS.md`.
2. Run `git status --short --untracked-files=all`.
3. Classify the task.
4. Read `docs/agents/task-types.md`.
5. Read `docs/agents/boundaries.md` when scope is unclear.
6. Read `docs/agents/verification.md` before claiming completion.

Do not begin from conversation mood alone. The repo state and the relevant spec or roadmap define the current work.

## Classification

Classify every request as `Docs`, `Fix`, `Experiment`, `Demo`, `Tooling`, or `Research` before editing.

If more than one type applies, use the priority from `task-types.md`. If classification changes the planning gate, use the stricter gate.

State the assumed task type before editing when the user gives free-form input.

## Short Task To Plan Contract

Short tasks are enough to start classification and planning.

- `Docs`, narrow `Fix`, and typo-only `Tooling` may proceed when acceptance is observable.
- `Experiment`, `Demo`, broad `Tooling`, boundary changes, architecture changes, and project-direction changes require a spec or plan first.
- In gated cases, do not edit runtime code before the user approves the drafted spec or plan.
- The user does not need a long setup prompt; derive context from repo docs and local files.
- Subjective acceptance such as `feels better` must be converted to observable acceptance first.

## Planning Gate

Proceed directly only when:

- task type is clear,
- allowed file area is clear,
- acceptance is observable,
- verification command is known,
- no boundary is violated.

Draft a spec or plan first when the task adds an experiment, changes the integrated demo, changes tool behavior broadly, changes architecture, or changes project direction.

## Implementation Loop

1. Inspect current files before editing.
2. Make the smallest scoped change.
3. Run task-specific verification.
4. Fix verification failures inside the task scope.
5. Report files changed, commands run, exit codes, and remaining risk.

Do not include unrelated cleanup in the same task.

## Vague Continuation Prompts

For `continue`, `go`, `干`, `继续`, and terse equivalents:

1. Run `git status --short --untracked-files=all`.
2. Inspect the relevant roadmap, spec, or implementation plan.
3. State the concrete artifact and task type being continued.
4. Proceed only if the assumption is low risk.

If the active artifact cannot be identified, ask one concise question.

## Reporting

Final reports must include:

- task type,
- files changed,
- verification commands run,
- evidence from command output,
- skipped commands and reasons,
- remaining risk.

Do not claim completion without evidence from the current task.

## Stopping

Stop before editing when:

- the task violates `docs/agents/boundaries.md`,
- the task type is ambiguous and the wrong type changes scope,
- acceptance is subjective or not observable,
- verification repeatedly fails,
- required GUI, network, or desktop access is unavailable,
- a fix would delete intentional functionality.
