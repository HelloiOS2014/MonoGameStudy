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

## Final File Skeleton

The implementation plan must produce these file shapes, not only these filenames.

### Root `README.md`

Target sections:

- `# MonoGame Study Framework`
- `What This Is`
- `Current Status`
- `For Humans`
- `For Agents`
- `Shared Verification`
- `Boundaries`
- `Project Map`
- `Phase 1 Record`

`README.md` must be a router. It introduces the framework and sends each audience to the right file. It must not copy the full tutorial chapter list, task-type matrix, or verification matrix.

### Root `AGENTS.md`

Target sections:

- `Default Role`
- `First Actions`
- `Short Task Format`
- `Task Types`
- `Required Reading`
- `Verification Rule`
- `Git Rules`
- `Stop Conditions`

`AGENTS.md` must fit the first-read role. It gives agents enough rules to begin safely, then links to `docs/agents/` for the detailed tables.

### `docs/agents/README.md`

Target sections:

- `Start Here`
- `Task Flow`
- `Files`
- `Fast Checks`

This file is the directory map for agent docs. It must not restate the detailed contents of every child file.

### `docs/agents/task-types.md`

Target sections:

- `Summary Table`
- `Docs`
- `Fix`
- `Experiment`
- `Demo`
- `Tooling`
- `Research`
- `Gray Areas`

Each task type section must include when to use it, required planning level, allowed file areas, required verification, and one example.

### `docs/agents/task-template.md`

Target sections:

- `Minimal Format`
- `Chinese Acceptance`
- `English Acceptance`
- `Examples`
- `When Free-Form Input Is Allowed`

This file carries the short prompt contract. It must let the user issue compact tasks without restating repo context.

### `docs/agents/development-protocol.md`

Target sections:

- `Startup`
- `Classification`
- `Short Task To Plan Contract`
- `Planning Gate`
- `Implementation Loop`
- `Vague Continuation Prompts`
- `Reporting`
- `Stopping`

This file defines agent behavior, not human tutorial prose.

### `docs/agents/verification.md`

Target sections:

- `Rule`
- `Fresh Workspace Commands`
- `Warmed Workspace Commands`
- `Command Matrix`
- `GUI Smoke Notes`
- `Failure Handling`
- `Evidence To Report`

The command matrix must give exact commands by task type.

### `docs/agents/boundaries.md`

Target sections:

- `Purpose`
- `Allowed`
- `Not Allowed`
- `Requires A Spec`
- `Rationale`
- `Boundary Violation Response`

This file explains the scope walls that keep the repo a study framework instead of a general game project.

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

## README Rewrite Strategy

The rewrite must preserve the existing history while changing the front-door framing.

Required strategy:

- Reframe the repo as a framework built from completed Phase 1 MonoGame research.
- Keep links to the Phase 1 closeout, technical evaluation, tutorial, validation log, and roadmap.
- Put the Human path and Agent path near the top.
- Keep shared verification commands visible without turning the README into a command manual.
- Move detailed research narrative below the routing sections or link to the existing docs.
- Move the old "archive this repository and start a separate prototype" recommendation into `Phase 1 Record` as a historical note dated before this framework pass.
- The current top-level next step must be framework usage: humans start with `docs/tutorial/README.md`; agents start with `AGENTS.md`.
- Do not delete project history to make the repo look newly created.
- Do not present `demo/integrated-demo` as a production-ready game.

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

It must make short tasks operational:

- A short task is enough for the agent to start classification and planning.
- For `Docs`, narrow `Fix`, and typo-only `Tooling`, the agent may proceed after classification when the acceptance signal is observable.
- For `Experiment`, `Demo`, broad `Tooling`, boundary changes, architecture changes, and project-direction changes, the short task triggers the agent to draft the required spec or implementation plan first.
- In those gated cases, the agent must not edit runtime code until the user approves the drafted spec or plan.
- The user must not need to write a long setup prompt; the agent derives repo context from `AGENTS.md`, `docs/agents/`, the current spec, and local files.
- If acceptance is subjective or not observable, the agent must stop and convert it into an observable acceptance proposal before implementation.

It must also cover vague continuation prompts. If the user says only "continue", "go", "干", "继续", or similar, the agent must inspect current repo state and the relevant roadmap/spec before choosing the next action. The agent should state the assumed task type and milestone before making changes.

### `docs/agents/verification.md`

This file is the verification matrix.

It must map each task type to exact commands, including:

- `dotnet restore GameDemo.sln` for fresh workspaces before `--no-restore` commands,
- `git diff --check`,
- `git grep -n -E "UNRESOLVED|NEEDS_DECISION|FIXME" -- <changed-md-files>` with no matches before docs completion claims,
- `bash -n tools/check-env.sh`,
- `bash -n tools/check-tutorial.sh`,
- `./tools/check-env.sh`,
- `dotnet build GameDemo.sln -m:1` for fresh workspaces,
- `dotnet build GameDemo.sln --no-restore -m:1`,
- targeted test projects,
- GUI smoke commands,
- `./tools/check-tutorial.sh` for broad tutorial/framework changes.

It must explicitly separate fresh workspace commands from warmed workspace commands. `--no-restore` commands are valid only after restore has succeeded in the same workspace or the agent has evidence that dependencies are already restored.

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

## Short Task To Plan Contract

The framework must let the user issue short tasks without also writing a long agent brief.

Contract:

- Short tasks are accepted as task intake.
- Agent docs supply the missing context: repo purpose, file map, boundaries, task types, planning gates, and verification commands.
- For tasks that do not require a spec or plan, the agent classifies, states the scope, edits the allowed files, verifies, and reports evidence.
- For tasks that require a spec or plan, the agent uses the short task to draft the missing spec or plan, then waits for user approval before runtime or broad documentation edits.
- For `Experiment` and `Demo`, a short task never authorizes immediate runtime implementation.
- For vague tasks such as `继续`, the agent must identify the active artifact from git state and the relevant spec or roadmap before changing files.
- For subjective acceptance such as "feels better", the agent must stop and propose observable acceptance before implementation.

## Short Task Examples

These examples must appear in `docs/agents/task-template.md` and be consistent with `docs/agents/task-types.md`.

```text
Docs: Tighten the setup tutorial wording for the MGCB restore step
验收: The changed chapter names the exact command and git diff has no trailing whitespace.
```

```text
Fix: Repair the e05 content smoke after an asset path rename
验收: The failing smoke is reproduced first, then the targeted smoke and GameDemo.sln build pass.
```

```text
Experiment: Add a mouse-drag input variant to e03
验收: New smoke simulates a drag path and exits; GameDemo.sln builds with 0 errors.
```

```text
Demo: Fix the restart prompt copy after win state
Acceptance: Integrated demo tests pass and the smoke exits after the configured frame limit.
```

```text
Tooling: Add link checks to tutorial verification
验收: The script reports a broken internal tutorial link with a non-zero exit code.
```

```text
Research: Evaluate whether an e08 shader experiment belongs in v2
Acceptance: The report lists local evidence, tradeoffs, and a recommendation without runtime changes.
```

## Gray-Area Classification Rules

When a request fits more than one task type, classify by the highest-risk affected area:

```text
Demo > Experiment > Tooling > Fix > Docs > Research
```

Rules:

- Tutorial wording only is `Docs`.
- Tutorial command text plus script behavior is `Tooling`.
- Any new `experiments/eNN-*` directory is `Experiment`.
- Any runtime change under `demo/integrated-demo` is `Demo`.
- Docs-only changes under `demo/integrated-demo` are `Docs`.
- A broken smoke caused by existing code is `Fix`.
- New behavior added while fixing a smoke is `Experiment` or `Demo`, based on the affected area.
- New validation scripts or build helpers are `Tooling`.
- Study notes, comparison, evaluation, and planning are `Research`.
- If classification changes the planning gate, use the stricter planning gate.

## Experiment ID Rules

Agents must not assign experiment IDs by guessing.

Rules:

- Existing implemented experiments are `e01` through `e07` and `e10`.
- Original roadmap entries `e08` and `e09` refer to optional 3D and shader topics unless a new spec redefines them.
- A new experiment requires an approved spec or plan that names the ID and directory.
- If the request names a concept but not an ID, the agent must propose the ID in the spec or plan before creating files.
- The agent must not skip to `e11` merely because `e08` and `e09` were not implemented.
- The agent must not reuse an existing ID for a different concept.

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

## Document Sprawl Guardrails

The agent docs must be operational instructions, not essays.

Rules:

- No new `docs/agents/` file unless this spec or a new spec names it.
- Each `docs/agents/` file owns one responsibility from the final file skeleton.
- Target maximum size: `AGENTS.md` 120 lines, `docs/agents/README.md` 80 lines, `task-types.md` 220 lines, `task-template.md` 160 lines, `development-protocol.md` 180 lines, `verification.md` 180 lines, and `boundaries.md` 140 lines.
- If a target size is exceeded, the implementation must remove duplication or tighten wording before adding new files.
- Do not duplicate the task-type matrix in multiple files.
- Do not duplicate the verification matrix in multiple files.
- `AGENTS.md` stays concise and links to detailed agent docs.
- Every rule must map to a behavior, a stop condition, or a verification command.
- Human tutorial chapters must not absorb agent protocol details.
- Agent docs must not retell tutorial lessons except to link to the human tutorial.

## Verification Baseline

Shared health commands:

```bash
./tools/check-env.sh
dotnet restore GameDemo.sln
dotnet build GameDemo.sln -m:1
./tools/check-tutorial.sh
git diff --check
git status --short --untracked-files=all
```

Agents should not always run the full tutorial dry-run for small docs edits, because it opens GUI smoke windows and publishes `e10`. The verification matrix in `docs/agents/verification.md` will define which subset is required for each task type.

For warmed workspaces, `docs/agents/verification.md` may use `--no-restore` commands after documenting the restore precondition. For fresh workspaces, the first build path must include restore.

## Agent Usability Acceptance Tests

The implementation is acceptable only if the written agent docs support these task intake cases without extra user prompting.

Case 1:

```text
Docs: Add a note about MGCB cache warnings
验收: The relevant tutorial chapter mentions the warning and git diff has no trailing whitespace.
```

Expected agent behavior: classify as `Docs`, read `AGENTS.md` plus `docs/agents/task-types.md`, edit only docs, and run documentation-level verification.

Case 2:

```text
e03 鼠标拖拽
```

Expected agent behavior: classify as `Experiment` unless the user narrows the request to docs or research, state the assumption, require a spec/plan before runtime implementation, and avoid editing demo code.

Case 3:

```text
继续
```

Expected agent behavior: inspect git status and the relevant roadmap/spec, state the concrete task it is continuing, then proceed only if the assumption is low risk.

Case 4:

```text
修一下 check-tutorial 里 e10 失败
```

Expected agent behavior: classify as `Fix` with `Tooling` risk, reproduce the failure when feasible, inspect `tools/check-tutorial.sh`, and run shell syntax plus the smallest command proving the fix.

Case 5:

```text
Demo: Add three new enemy types
验收: The demo feels more complete.
```

Expected agent behavior: stop before implementation because this expands the capstone demo beyond framework validation and the acceptance signal is not observable.

The implementation plan must include a final intake-case verification table with these columns:

- input,
- expected task type,
- required reads,
- allowed file areas,
- required verification,
- expected stop or proceed decision,
- pass or fail evidence.

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
- `docs/agents/task-template.md` includes working examples for all six task types.
- Gray-area task classification rules are documented.
- Experiment ID assignment rules are documented.
- The six task types are documented with required spec/plan behavior and verification.
- Agent usability acceptance tests are represented in the docs and checked through a pass/fail intake table.
- Document sprawl guardrails are represented in the docs.
- Fresh workspace verification and warmed workspace verification are separated.
- Human tutorial docs remain linked and unchanged in purpose.
- Out-of-scope static-site prototype files are absent.
- `git diff --check` exits 0.
- `bash -n tools/check-env.sh` exits 0.
- `bash -n tools/check-tutorial.sh` exits 0.
- `git status --short --untracked-files=all` shows only intended files before commit, and is clean after commit.

## Spec Self-Review Notes

This spec intentionally does not implement the agent docs. It defines the contract that the next implementation plan must satisfy.

The highest-risk ambiguity is keeping human tutorial content and agent protocol content separate while sharing verification commands. The implementation plan must not merge these into one long document; root `README.md` routes audiences, `docs/tutorial/` teaches humans, and `docs/agents/` instructs agents.
