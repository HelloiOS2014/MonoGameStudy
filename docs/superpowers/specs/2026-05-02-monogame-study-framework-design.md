# MonoGame Study Framework Design

Date: 2026-05-02
Status: Draft for review

## Purpose

Turn this repository from a completed MonoGame research record plus Markdown tutorial into a reusable **MonoGame Study Framework**.

The framework must serve two audiences at the same time:

- **Humans** use the tutorial to understand the MonoGame baseline, run experiments, inspect expected output, and learn the shape of the framework.
- **Agents** use a development protocol to continue work without the user writing long setup prompts every time.

The important correction from the previous work is that a tutorial is not enough. The repository needs a human-facing learning path and an agent-facing operating contract that share the same code, boundaries, and verification commands.

## Current State

Already present:

- `experiments/e01-*` through `experiments/e07-*` and `experiments/e10-publishing` as focused MonoGame reference implementations.
- `demo/integrated-demo` as a capstone demo.
- `docs/tutorial/` as a human-readable Markdown tutorial.
- `tools/check-env.sh` for toolchain readiness.
- `tools/check-tutorial.sh` for full tutorial dry-run verification.
- `docs/tutorial/ROADMAP.md` and `docs/tutorial/validation-log.md`.

Missing:

- Root-level `AGENTS.md`.
- Agent-facing task type definitions.
- Agent-facing short task template.
- Development protocol explaining how an agent starts work.
- Verification matrix by change type.
- Explicit boundaries for what agents may and may not change.
- A README that presents this repo as a dual-entry framework rather than only a research project or tutorial.

Known cleanup and correction:

- A static-site prototype was started in the wrong direction before this spec. This framework pass must not recreate `docs/tutorial-site/` or `tools/build-tutorial-site.mjs`.

## Non-Goals

This pass will not:

- Build a tutorial website.
- Add screenshots, videos, or generated visual assets.
- Add new MonoGame experiments.
- Add new game features.
- Expand `demo/integrated-demo` into a production game.
- Introduce Godot or another engine track.
- Add GitHub issue templates, PR templates, or CI unless a separately approved task explicitly scopes them.

## Information Architecture

The repository should expose these entry points:

```text
README.md
  Framework overview
  Human path -> docs/tutorial/README.md
  Agent path -> AGENTS.md
  Shared verification -> tools/check-env.sh, tools/check-tutorial.sh
  Boundaries -> docs/agents/boundaries.md

AGENTS.md
  Required first-read file for agents
  Default agent role
  Task type table
  Required reading order
  Verification and git rules
  Links into docs/agents/

docs/tutorial/
  Human learning path
  Existing chapter structure remains the human source of truth

docs/agents/
  README.md
  task-types.md
  task-template.md
  development-protocol.md
  verification.md
  boundaries.md
```

## Documentation Language

Repository documentation remains English by default because the existing tutorial, reports, and code comments are English.

Agent task intake must support both:

```text
验收: <observable result>
```

and:

```text
Acceptance: <observable result>
```

The agent-facing docs should mention both forms so the user can issue short Chinese or English tasks without writing a long prompt.

## README Role

`README.md` becomes the framework front door.

It should answer:

- What is this repository now?
- Is it a research record, a tutorial, a framework, or a game?
- Where should a human start?
- Where should an agent start?
- What commands prove the framework is healthy?
- What should not be expanded casually?

The README should not duplicate all tutorial or agent protocol content. It should route readers to the correct entry point.

## Human-Facing Tutorial Role

`docs/tutorial/` remains the human learning path.

It should answer:

- How do I set up MonoGame on this repo's target environment?
- Which experiment teaches each concept?
- What command do I run?
- What output should I see?
- What common failure should I diagnose?
- What checkpoint proves I understood the chapter?

The tutorial should not become the agent protocol. It may link to the agent docs, but it remains optimized for a human learner.

## Agent-Facing Protocol Role

Agents should be able to enter the repository and work from a short task without requiring a long user prompt.

The default agent identity is:

> A development agent maintaining and extending a MonoGame study framework with strict scope control.

The agent should first read `AGENTS.md`, then follow links based on task type.

The protocol must make these decisions explicit:

- classify the task,
- decide whether a spec/plan is required,
- identify allowed files,
- identify required verification,
- decide when to stop and ask the user,
- avoid expanding the integrated demo into a larger game,
- avoid introducing unrelated engines or toolchains.

## Agent Document Responsibilities

### `AGENTS.md`

Root `AGENTS.md` is the required first-read file for agents.

It must contain:

- default role: development agent for a MonoGame study framework,
- startup checklist,
- short task format,
- six task types,
- hard boundaries,
- verification rule: no completion claim without fresh evidence,
- git rule: stage only files changed for the current task,
- links to `docs/agents/`.

It should be short enough for agents to read every session.

### `docs/agents/README.md`

This is the table of contents for detailed agent guidance.

It must answer:

- which file to read for task classification,
- which file to read before implementation,
- which file maps task types to verification commands,
- which file defines boundaries.

### `docs/agents/task-types.md`

This file defines `Docs`, `Fix`, `Experiment`, `Demo`, `Tooling`, and `Research`.

For each type it must include:

- when to use it,
- whether a spec is required,
- whether an implementation plan is required,
- allowed file areas,
- required verification category,
- one concrete example task.

### `docs/agents/task-template.md`

This file defines the short task format:

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

It must also document the English equivalent:

```text
<Type>: <goal>
Acceptance: <observable result, command, or output>
```

It must include examples for all six task types so the user does not need to write long setup prompts.

### `docs/agents/development-protocol.md`

This file defines the work loop:

1. read `AGENTS.md`,
2. inspect `git status --short --untracked-files=all`,
3. classify the task,
4. read the task-type guidance,
5. decide whether spec/plan is required,
6. implement only the scoped change,
7. run required verification,
8. report evidence and remaining risk.

It must also cover vague continuation prompts. If the user says only "continue", "go", "干", "继续", or similar, the agent must inspect current repo state and the relevant roadmap/spec before choosing the next action. The agent should state the assumed task type and milestone before making changes.

### `docs/agents/verification.md`

This file is the verification matrix.

It must map each task type to exact commands, including:

- `git diff --check`,
- `bash -n tools/check-env.sh`,
- `bash -n tools/check-tutorial.sh`,
- `./tools/check-env.sh`,
- `dotnet build GameDemo.sln --no-restore -m:1`,
- targeted test projects,
- GUI smoke commands,
- `./tools/check-tutorial.sh` for broad tutorial/framework changes.

It must explicitly say that GUI smoke commands open DesktopGL windows and may require local desktop access or tool escalation.

### `docs/agents/boundaries.md`

This file explains boundaries and their rationale.

It must cover:

- why `demo/integrated-demo` is not a production game,
- why Godot is not part of this repo's future path,
- when new experiments are allowed,
- when new tools are allowed,
- what to do if a task violates boundaries.

## Task Format

Preferred user task format:

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

Example:

```text
Experiment: Add a mouse-drag input variant to e03
验收: New smoke simulates a drag path and exits; GameDemo.sln builds with 0 errors.
```

If the user gives only a free-form request, the agent must classify it into one of the task types before doing implementation work. If classification is ambiguous, the agent states the assumed type and proceeds only when the assumption is low risk; otherwise it asks one concise question.

If the user gives a terse continuation request, the agent must not guess from conversation mood alone. It must inspect the current repo state and the relevant roadmap/spec, then state what it is continuing before making changes.

## Task Types

The framework uses six task types.

### Docs

Used for tutorial chapters, README, reports, agent docs, and roadmap text.

Spec/plan requirement:

- Small corrections do not need a spec.
- Structural documentation changes need a short plan.
- Changes that redefine project direction need a spec.

Default verification:

- `git diff --check`
- Markdown structure checks when touching `docs/tutorial/`
- `bash -n tools/check-tutorial.sh` if command references change

### Fix

Used for broken builds, failing smoke tests, incorrect docs, or wrong scripts.

Spec/plan requirement:

- Not required for a narrow bug.
- Required when the root cause changes architecture or project boundaries.

Default verification:

- reproduce the failure first when feasible,
- run the smallest command that proves the fix,
- run broader build/smoke if shared behavior changed.

### Experiment

Used for adding or extending `experiments/eNN-*`.

Spec/plan requirement:

- Required.

Required deliverables:

- project directory under `experiments/`,
- matching test project when logic can be tested without graphics,
- smoke mode when the experiment opens a GUI,
- README with how to run, what to see, and what was learned,
- project added to `GameDemo.sln`.

Default verification:

- experiment test project,
- experiment smoke command,
- `dotnet build GameDemo.sln --no-restore -m:1`,
- `git diff --check`.

### Demo

Used for changes to `demo/integrated-demo`.

Spec/plan requirement:

- Required.

Default stance:

- The integrated demo is a capstone and validation harness, not a production game.
- Do not expand it into a larger game without a separate approved spec.

Default verification:

- `dotnet run --project demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj --no-restore`
- `env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore`
- `dotnet build GameDemo.sln --no-restore -m:1`

### Tooling

Used for `tools/check-env.sh`, `tools/check-tutorial.sh`, future verification scripts, or build helpers.

Spec/plan requirement:

- Required for new scripts or behavior-changing edits.
- Not required for typo-only fixes.

Default verification:

- shell syntax checks for shell scripts,
- script dry-run where practical,
- commands that prove failure handling when the script expects failures.

### Research

Used for investigation, comparison, evaluation, spec writing, and implementation planning.

Spec/plan requirement:

- Research may produce a spec or plan but must not silently implement runtime changes.

Default verification:

- cite local files or primary sources,
- list assumptions,
- identify remaining unknowns,
- do not claim implementation completion.

## Boundaries

Agents must preserve these boundaries:

- No Godot track in this repository.
- No casual expansion of `demo/integrated-demo`.
- No new MonoGame experiment without a spec/plan.
- No new tutorial website in this pass.
- No destructive git commands.
- No committing unrelated or untracked files.
- No claiming completion without fresh verification output.
- No changing target platform away from macOS DesktopGL without a spec.
- No changing `.NET 10` pinning without a spec.

## Verification Baseline

Shared health commands:

```bash
./tools/check-env.sh
dotnet build GameDemo.sln --no-restore -m:1
./tools/check-tutorial.sh
git diff --check
git status --short --untracked-files=all
```

Agents should not always run the full tutorial dry-run for small docs edits, because it opens GUI smoke windows and publishes `e10`. The verification matrix in `docs/agents/verification.md` will define which subset is required for each task type.

## Stop Conditions

An agent must stop and ask the user when:

- the task type is ambiguous and the wrong type changes scope,
- requested work violates boundaries,
- verification fails repeatedly,
- a fix requires deleting intentional functionality,
- a change touches files outside the task's expected ownership,
- network or GUI access is required and not available,
- adding a new toolchain would be necessary.

## Implementation Deliverables

The implementation should create or modify:

```text
AGENTS.md
docs/agents/README.md
docs/agents/task-types.md
docs/agents/task-template.md
docs/agents/development-protocol.md
docs/agents/verification.md
docs/agents/boundaries.md
README.md
```

The implementation must ensure these out-of-scope static-site files do not exist:

```text
docs/tutorial-site/index.html
tools/build-tutorial-site.mjs
```

## Acceptance Criteria

The framework pass is complete when:

- `README.md` clearly presents the repo as a MonoGame Study Framework with Human and Agent entry points.
- `README.md` preserves links to the Phase 1 closeout, technical evaluation, human tutorial, and shared verification commands.
- Root `AGENTS.md` exists and gives agents enough first-read instructions to start safely.
- `docs/agents/` contains task types, task template, development protocol, verification matrix, and boundaries.
- The short task format is documented with both `验收:` and `Acceptance:`.
- The six task types are documented with required spec/plan behavior and verification.
- Human tutorial docs remain linked and unchanged in purpose.
- Out-of-scope static-site prototype files are absent.
- `git diff --check` exits 0.
- `bash -n tools/check-env.sh` exits 0.
- `bash -n tools/check-tutorial.sh` exits 0.
- `git status --short --untracked-files=all` shows only intended files before commit, and is clean after commit.

## Spec Self-Review Notes

This spec intentionally does not implement the agent docs. It defines the contract that the next implementation plan must satisfy.

The highest-risk ambiguity is keeping human tutorial content and agent protocol content separate while sharing verification commands. The implementation plan must not merge these into one long document; root `README.md` routes audiences, `docs/tutorial/` teaches humans, and `docs/agents/` instructs agents.
