# MonoGame Study Framework Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Turn the repository front door and agent docs into a usable MonoGame Study Framework for humans and development agents.

**Architecture:** `README.md` routes humans and agents. `AGENTS.md` is the required first-read file for agents. `docs/agents/` holds detailed operational rules, split by responsibility so agents can classify short tasks, obey planning gates, and run the right verification commands.

**Tech Stack:** Markdown, Bash verification commands, existing .NET 10 MonoGame solution, existing tutorial and report documents.

---

## Source Spec

Implement this spec:

- `docs/superpowers/specs/2026-05-02-monogame-study-framework-design.md`

Do not implement runtime game changes. Do not create a tutorial website. Do not create new experiments.

## File Structure

Create:

- `AGENTS.md`: concise root agent entrypoint, target maximum 120 lines.
- `docs/agents/README.md`: agent docs directory map, target maximum 80 lines.
- `docs/agents/task-types.md`: six task types, gray areas, experiment ID rules, target maximum 220 lines.
- `docs/agents/task-template.md`: short task format and examples, target maximum 160 lines.
- `docs/agents/development-protocol.md`: agent work loop, planning gates, continuation behavior, target maximum 180 lines.
- `docs/agents/verification.md`: exact verification commands by task type, target maximum 180 lines.
- `docs/agents/boundaries.md`: allowed work, blocked work, spec-required work, target maximum 140 lines.

Modify:

- `README.md`: reframe as `MonoGame Study Framework`, preserve Phase 1 record links, move old archive guidance to historical note.

Must remain absent:

- `docs/tutorial-site/index.html`
- `tools/build-tutorial-site.mjs`

## Task 1: Add Root Agent Entrypoint

**Files:**

- Create: `AGENTS.md`

- [ ] **Step 1: Inspect source spec and current tree**

Run:

```bash
sed -n '87,197p' docs/superpowers/specs/2026-05-02-monogame-study-framework-design.md
find docs/agents -maxdepth 1 -type f -print 2>/dev/null | sort
```

Expected:

- Spec output includes the final file skeleton.
- `docs/agents` may be missing before this task starts.

- [ ] **Step 2: Create `AGENTS.md`**

Write `AGENTS.md` with these exact top-level headings:

```markdown
# AGENTS.md

## Default Role

## First Actions

## Short Task Format

## Task Types

## Required Reading

## Verification Rule

## Git Rules

## Stop Conditions
```

Required content under those headings:

- Default role: `A development agent maintaining and extending a MonoGame study framework with strict scope control.`
- First actions:
  - inspect `git status --short --untracked-files=all`,
  - classify the task,
  - read the matching file under `docs/agents/`,
  - identify allowed files,
  - identify required verification.
- Short task format:
  - `<Type>: <goal>`
  - `验收: <observable result, command, or output>`
  - `Acceptance: <observable result, command, or output>`
- Task types:
  - `Docs`
  - `Fix`
  - `Experiment`
  - `Demo`
  - `Tooling`
  - `Research`
- Required reading:
  - `docs/agents/task-types.md` for classification,
  - `docs/agents/task-template.md` for prompt shape,
  - `docs/agents/development-protocol.md` for work loop,
  - `docs/agents/verification.md` for commands,
  - `docs/agents/boundaries.md` for scope.
- Verification rule: no completion claim without fresh evidence.
- Git rules:
  - do not stage unrelated files,
  - do not use destructive git commands,
  - commit only the current task scope.
- Stop conditions:
  - ambiguous scope that changes task type,
  - boundary violation,
  - subjective acceptance,
  - repeated verification failure,
  - required GUI or network access unavailable.

- [ ] **Step 3: Verify root entrypoint**

Run:

```bash
rg -n "^## (Default Role|First Actions|Short Task Format|Task Types|Required Reading|Verification Rule|Git Rules|Stop Conditions)$" AGENTS.md
wc -l AGENTS.md
git diff --check
```

Expected:

- All eight headings are found.
- `AGENTS.md` has 120 lines or fewer.
- `git diff --check` exits 0.

- [ ] **Step 4: Commit Task 1**

Run:

```bash
git add AGENTS.md
git commit -m "docs: add agent entrypoint"
```

Expected: commit succeeds with only `AGENTS.md`.

## Task 2: Add Agent Task Guidance

**Files:**

- Create: `docs/agents/README.md`
- Create: `docs/agents/task-types.md`
- Create: `docs/agents/task-template.md`

- [ ] **Step 1: Create `docs/agents/README.md`**

Use these headings:

```markdown
# Agent Guide

## Start Here

## Task Flow

## Files

## Fast Checks
```

Required content:

- Tell agents to start at root `AGENTS.md`.
- Route task classification to `docs/agents/task-types.md`.
- Route short prompt format to `docs/agents/task-template.md`.
- Route work loop and continuation prompts to `docs/agents/development-protocol.md`.
- Route command selection to `docs/agents/verification.md`.
- Route scope questions to `docs/agents/boundaries.md`.
- Include the fast checks:
  - `git status --short --untracked-files=all`
  - `git diff --check`
  - `bash -n tools/check-env.sh`
  - `bash -n tools/check-tutorial.sh`

- [ ] **Step 2: Create `docs/agents/task-types.md`**

Use these headings:

```markdown
# Task Types

## Summary Table

## Docs

## Fix

## Experiment

## Demo

## Tooling

## Research

## Gray Areas

## Experiment ID Rules
```

Required summary table columns:

- Type
- Use For
- Spec Required
- Plan Required
- Allowed Files
- Default Verification

Required rows:

- `Docs`: tutorial, README, reports, roadmap, agent docs; small corrections no spec; structural docs need a short plan; project-direction docs need a spec; allowed files are `README.md`, `docs/**`, `AGENTS.md`; verification is docs checks plus `git diff --check`.
- `Fix`: broken build, smoke, script, incorrect docs; spec only when architecture or boundary changes; allowed files are failing surface plus minimal support files; verification reproduces failure when feasible and then proves fix.
- `Experiment`: add or extend `experiments/eNN-*`; spec required; plan required; allowed files are named experiment, matching test project, `GameDemo.sln`, experiment README; verification includes targeted tests, smoke, solution build.
- `Demo`: runtime changes under `demo/integrated-demo`; spec required; plan required; allowed files are integrated demo and matching tests; verification includes demo tests, demo smoke, solution build.
- `Tooling`: `tools/check-env.sh`, `tools/check-tutorial.sh`, verification helpers; spec required for new scripts or behavior-changing edits; verification includes shell syntax and focused command behavior.
- `Research`: investigation, comparison, evaluation, spec writing, implementation planning; implementation changes blocked; verification is local evidence, assumptions, unknowns, and no runtime edits.

Required gray-area order:

```text
Demo > Experiment > Tooling > Fix > Docs > Research
```

Required gray-area rules:

- Tutorial wording only is `Docs`.
- Tutorial command text plus script behavior is `Tooling`.
- New `experiments/eNN-*` directory is `Experiment`.
- Runtime change under `demo/integrated-demo` is `Demo`.
- Docs-only change under `demo/integrated-demo` is `Docs`.
- Broken smoke caused by existing code is `Fix`.
- New behavior while fixing a smoke is `Experiment` or `Demo`.
- New validation scripts or build helpers are `Tooling`.
- Study notes, comparison, evaluation, and planning are `Research`.
- If classification changes the planning gate, use the stricter gate.

Required experiment ID rules:

- Existing implemented experiments are `e01` through `e07` and `e10`.
- Roadmap entries `e08` and `e09` refer to optional 3D and shader topics unless a new spec redefines them.
- A new experiment requires an approved spec or plan that names the ID and directory.
- If the request names a concept but not an ID, propose the ID in the spec or plan before creating files.
- Do not skip to `e11` only because `e08` and `e09` are not implemented.
- Do not reuse an existing ID for a different concept.

- [ ] **Step 3: Create `docs/agents/task-template.md`**

Use these headings:

```markdown
# Task Template

## Minimal Format

## Chinese Acceptance

## English Acceptance

## Examples

## When Free-Form Input Is Allowed
```

Required minimal format:

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

Required English equivalent:

```text
<Type>: <goal>
Acceptance: <observable result, command, or output>
```

Required examples:

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

Free-form input rules:

- Agent must classify before editing.
- If the classification is low risk, agent states the assumed type and proceeds.
- If the classification changes planning gate or scope, agent asks one concise question or drafts the required spec or plan.
- Subjective acceptance such as `feels better` must be converted to observable acceptance before implementation.

- [ ] **Step 4: Verify Task 2**

Run:

```bash
rg -n "^## (Start Here|Task Flow|Files|Fast Checks)$" docs/agents/README.md
rg -n "^## (Summary Table|Docs|Fix|Experiment|Demo|Tooling|Research|Gray Areas|Experiment ID Rules)$" docs/agents/task-types.md
rg -n "^## (Minimal Format|Chinese Acceptance|English Acceptance|Examples|When Free-Form Input Is Allowed)$" docs/agents/task-template.md
rg -n "Demo > Experiment > Tooling > Fix > Docs > Research|e01|e07|e10|e08|e09|验收:|Acceptance:" docs/agents/task-types.md docs/agents/task-template.md
wc -l docs/agents/README.md docs/agents/task-types.md docs/agents/task-template.md
git diff --check
```

Expected:

- All listed headings are found.
- The priority order, experiment IDs, and both acceptance labels are found.
- Line counts are within the target maximums from the spec.
- `git diff --check` exits 0.

- [ ] **Step 5: Commit Task 2**

Run:

```bash
git add docs/agents/README.md docs/agents/task-types.md docs/agents/task-template.md
git commit -m "docs: add agent task guidance"
```

Expected: commit succeeds with only the three task guidance files.

## Task 3: Add Development Protocol And Verification Matrix

**Files:**

- Create: `docs/agents/development-protocol.md`
- Create: `docs/agents/verification.md`

- [ ] **Step 1: Create `docs/agents/development-protocol.md`**

Use these headings:

```markdown
# Development Protocol

## Startup

## Classification

## Short Task To Plan Contract

## Planning Gate

## Implementation Loop

## Vague Continuation Prompts

## Reporting

## Stopping
```

Required startup loop:

1. Read `AGENTS.md`.
2. Run `git status --short --untracked-files=all`.
3. Classify the task.
4. Read `docs/agents/task-types.md`.
5. Read `docs/agents/boundaries.md` when scope is unclear.
6. Read `docs/agents/verification.md` before claiming completion.

Required short task contract:

- Short task is enough to start classification and planning.
- `Docs`, narrow `Fix`, and typo-only `Tooling` may proceed when acceptance is observable.
- `Experiment`, `Demo`, broad `Tooling`, boundary changes, architecture changes, and project-direction changes require spec or plan first.
- In gated cases, do not edit runtime code before user approves the drafted spec or plan.
- User does not need a long setup prompt.
- Subjective acceptance must be converted to observable acceptance first.

Required vague continuation rule:

- For `continue`, `go`, `干`, `继续`, and terse equivalents, inspect git status plus the relevant roadmap or spec.
- State the concrete artifact and task type before changing files.
- If the active artifact cannot be identified, ask one concise question.

Required reporting format:

- task type,
- files changed,
- verification commands run,
- evidence from output,
- remaining risk.

- [ ] **Step 2: Create `docs/agents/verification.md`**

Use these headings:

```markdown
# Verification

## Rule

## Fresh Workspace Commands

## Warmed Workspace Commands

## Command Matrix

## GUI Smoke Notes

## Failure Handling

## Evidence To Report
```

Required rule:

- No completion claim without fresh verification evidence.

Required fresh workspace commands:

```bash
./tools/check-env.sh
dotnet restore GameDemo.sln
dotnet build GameDemo.sln -m:1
git diff --check
```

Required warmed workspace commands:

```bash
dotnet build GameDemo.sln --no-restore -m:1
git diff --check
```

Required docs verification:

```bash
git diff --check
git grep -n -E "UNRESOLVED|NEEDS_DECISION|FIXME" -- README.md AGENTS.md docs/agents docs/tutorial
```

Expected for the `git grep` command: exit 1 with no matches when docs are complete.

Required shell verification:

```bash
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
```

Required command matrix rows:

- `Docs`: `git diff --check`; unresolved-marker grep on changed docs; `bash -n tools/check-tutorial.sh` when tutorial commands change.
- `Fix`: reproduce failure when feasible; run the smallest proving command; run broader command if shared behavior changed.
- `Experiment`: targeted test project; experiment smoke; `dotnet build GameDemo.sln --no-restore -m:1`; `git diff --check`.
- `Demo`: `dotnet run --project demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj --no-restore`; `env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore`; `dotnet build GameDemo.sln --no-restore -m:1`.
- `Tooling`: shell syntax; focused script behavior; failure-path command when script handles failures.
- `Research`: local file evidence or primary-source citations; assumptions; unknowns; no implementation completion claim.

Required GUI note:

- DesktopGL smoke commands open short-lived windows.
- GUI smoke may require local desktop access or escalated tool permission.
- If GUI access is unavailable, report the blocker and do not claim the smoke passed.

Required evidence:

- command,
- exit code,
- pass/fail reading,
- skipped command and reason when a command could not run.

- [ ] **Step 3: Verify Task 3**

Run:

```bash
rg -n "^## (Startup|Classification|Short Task To Plan Contract|Planning Gate|Implementation Loop|Vague Continuation Prompts|Reporting|Stopping)$" docs/agents/development-protocol.md
rg -n "^## (Rule|Fresh Workspace Commands|Warmed Workspace Commands|Command Matrix|GUI Smoke Notes|Failure Handling|Evidence To Report)$" docs/agents/verification.md
rg -n "dotnet restore GameDemo.sln|dotnet build GameDemo.sln -m:1|--no-restore|DEMO_SMOKE_EXIT_AFTER_FRAMES|UNRESOLVED|NEEDS_DECISION|FIXME|exit 1" docs/agents/verification.md
wc -l docs/agents/development-protocol.md docs/agents/verification.md
git diff --check
```

Expected:

- Required headings are found.
- Fresh and warmed workspace command paths are both present.
- GUI smoke and unresolved-marker expectations are present.
- Line counts are within target maximums.
- `git diff --check` exits 0.

- [ ] **Step 4: Commit Task 3**

Run:

```bash
git add docs/agents/development-protocol.md docs/agents/verification.md
git commit -m "docs: add agent workflow and verification"
```

Expected: commit succeeds with only the two protocol files.

## Task 4: Add Boundaries

**Files:**

- Create: `docs/agents/boundaries.md`

- [ ] **Step 1: Create `docs/agents/boundaries.md`**

Use these headings:

```markdown
# Boundaries

## Purpose

## Allowed

## Not Allowed

## Requires A Spec

## Rationale

## Boundary Violation Response
```

Required allowed list:

- improve human tutorial docs,
- improve agent docs,
- fix broken verification scripts,
- fix narrow build or smoke failures,
- update README routing,
- write research notes, specs, and implementation plans.

Required blocked list:

- no Godot track,
- no tutorial website in this pass,
- no casual expansion of `demo/integrated-demo`,
- no new game features without spec,
- no new MonoGame experiment without approved spec or plan,
- no target platform change away from macOS DesktopGL without spec,
- no `.NET 10` pinning change without spec,
- no destructive git commands,
- no unrelated commits.

Required spec list:

- `Experiment`,
- `Demo`,
- broad `Tooling`,
- boundary changes,
- architecture changes,
- project-direction changes.

Required response:

- stop before implementation,
- state the violated boundary,
- propose a narrower safe task or request an approved spec,
- do not edit runtime code while blocked.

- [ ] **Step 2: Verify Task 4**

Run:

```bash
rg -n "^## (Purpose|Allowed|Not Allowed|Requires A Spec|Rationale|Boundary Violation Response)$" docs/agents/boundaries.md
rg -n "Godot|tutorial website|integrated-demo|macOS DesktopGL|\\.NET 10|destructive git|approved spec" docs/agents/boundaries.md
wc -l docs/agents/boundaries.md
git diff --check
```

Expected:

- Required headings and scope walls are found.
- `docs/agents/boundaries.md` has 140 lines or fewer.
- `git diff --check` exits 0.

- [ ] **Step 3: Commit Task 4**

Run:

```bash
git add docs/agents/boundaries.md
git commit -m "docs: add agent boundaries"
```

Expected: commit succeeds with only `docs/agents/boundaries.md`.

## Task 5: Rewrite Root README As Framework Front Door

**Files:**

- Modify: `README.md`

- [ ] **Step 1: Rewrite top-level sections**

Replace the current README structure with these sections:

```markdown
# MonoGame Study Framework

## What This Is

## Current Status

## For Humans

## For Agents

## Shared Verification

## Boundaries

## Project Map

## Phase 1 Record
```

Required content:

- `What This Is`: states this is a MonoGame Study Framework built from Phase 1 research, not a production game.
- `Current Status`: states Phase 1 research is complete and the framework path is now the current usage path.
- `For Humans`: links to `docs/tutorial/README.md`.
- `For Agents`: links to `AGENTS.md` and explains short task intake.
- `Shared Verification`: includes:
  - `./tools/check-env.sh`
  - `dotnet restore GameDemo.sln`
  - `dotnet build GameDemo.sln -m:1`
  - `./tools/check-tutorial.sh`
- `Boundaries`: links to `docs/agents/boundaries.md` and states no Godot, no tutorial site, no casual integrated demo expansion.
- `Project Map`: lists `experiments/`, `demo/integrated-demo/`, `docs/tutorial/`, `docs/agents/`, `docs/reports/`, `tools/`.
- `Phase 1 Record`: preserves links to:
  - `docs/superpowers/specs/2026-04-26-monogame-research-design.md`
  - `docs/reports/phase1-closeout.md`
  - `docs/reports/monogame-technical-evaluation.md`
  - `docs/tutorial/validation-log.md`
  - `docs/tutorial/ROADMAP.md`
  - `docs/00-roadmap.md`
- Move the old separate-prototype recommendation into this section as a historical Phase 1 recommendation, not current top-level guidance.

- [ ] **Step 2: Verify README routing**

Run:

```bash
rg -n "^## (What This Is|Current Status|For Humans|For Agents|Shared Verification|Boundaries|Project Map|Phase 1 Record)$" README.md
rg -n "docs/tutorial/README.md|AGENTS.md|docs/agents/boundaries.md|phase1-closeout|monogame-technical-evaluation|validation-log|ROADMAP|docs/00-roadmap.md" README.md
rg -n "Recommended next step: keep this repository archived" README.md
git diff --check
```

Expected:

- All target headings are found.
- Human, agent, boundary, closeout, evaluation, validation, roadmap links are found.
- The final `rg` command exits 1, proving the old top-level archive recommendation is gone.
- `git diff --check` exits 0.

- [ ] **Step 3: Commit Task 5**

Run:

```bash
git add README.md
git commit -m "docs: reframe repository as study framework"
```

Expected: commit succeeds with only `README.md`.

## Task 6: Final Framework Verification

**Files:**

- Verify: `AGENTS.md`
- Verify: `README.md`
- Verify: `docs/agents/*.md`
- Verify: absence of static-site files

- [ ] **Step 1: Verify required files and blocked files**

Run:

```bash
test -f AGENTS.md
test -f docs/agents/README.md
test -f docs/agents/task-types.md
test -f docs/agents/task-template.md
test -f docs/agents/development-protocol.md
test -f docs/agents/verification.md
test -f docs/agents/boundaries.md
test ! -e docs/tutorial-site/index.html
test ! -e tools/build-tutorial-site.mjs
```

Expected: every command exits 0.

- [ ] **Step 2: Verify headings**

Run:

```bash
rg -n "^# MonoGame Study Framework$|^## For Humans$|^## For Agents$" README.md
rg -n "^# AGENTS.md$|^## Default Role$|^## First Actions$|^## Stop Conditions$" AGENTS.md
rg -n "^# (Agent Guide|Task Types|Task Template|Development Protocol|Verification|Boundaries)$" docs/agents/*.md
```

Expected: all target headings are found.

- [ ] **Step 3: Verify line limits**

Run:

```bash
wc -l AGENTS.md docs/agents/README.md docs/agents/task-types.md docs/agents/task-template.md docs/agents/development-protocol.md docs/agents/verification.md docs/agents/boundaries.md
```

Expected maximums:

- `AGENTS.md`: 120
- `docs/agents/README.md`: 80
- `docs/agents/task-types.md`: 220
- `docs/agents/task-template.md`: 160
- `docs/agents/development-protocol.md`: 180
- `docs/agents/verification.md`: 180
- `docs/agents/boundaries.md`: 140

- [ ] **Step 4: Verify docs and shell syntax**

Run:

```bash
git diff --check
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
```

Expected: each command exits 0.

- [ ] **Step 5: Verify unresolved markers**

Run:

```bash
git grep -n -E "UNRESOLVED|NEEDS_DECISION|FIXME" -- README.md AGENTS.md docs/agents
```

Expected: command exits 1 with no matches.

- [ ] **Step 6: Run intake-case verification**

Use this table and mark each row from the implemented docs:

| input | expected task type | required reads | allowed file areas | required verification | expected stop or proceed decision | pass or fail evidence |
| --- | --- | --- | --- | --- | --- | --- |
| `Docs: Add a note about MGCB cache warnings` plus `验收: The relevant tutorial chapter mentions the warning and git diff has no trailing whitespace.` | Docs | `AGENTS.md`, `docs/agents/task-types.md`, `docs/agents/task-template.md`, `docs/agents/verification.md` | docs only | `git diff --check`; unresolved-marker grep on changed docs | proceed after classification | PASS only if docs route this as Docs with observable acceptance |
| `e03 鼠标拖拽` | Experiment | `AGENTS.md`, `docs/agents/task-types.md`, `docs/agents/development-protocol.md`, `docs/agents/boundaries.md` | no runtime files before spec or plan approval | no implementation verification before gate | stop to draft spec or plan | PASS only if docs block immediate runtime edit |
| `继续` | depends on repo state | `AGENTS.md`, `docs/agents/development-protocol.md` | active artifact only after inspection | selected after task type is stated | inspect state, state assumption, proceed only if low risk | PASS only if docs require git status plus roadmap or spec inspection |
| `修一下 check-tutorial 里 e10 失败` | Fix with Tooling risk | `AGENTS.md`, `docs/agents/task-types.md`, `docs/agents/verification.md` | `tools/check-tutorial.sh`, focused e10 files only if reproduced root cause requires them | reproduce when feasible; `bash -n tools/check-tutorial.sh`; smallest proving command | proceed after focused reproduction or state blocker | PASS only if docs require failure-first behavior when feasible |
| `Demo: Add three new enemy types` plus `验收: The demo feels more complete.` | Demo | `AGENTS.md`, `docs/agents/task-types.md`, `docs/agents/boundaries.md`, `docs/agents/development-protocol.md` | none before approved spec | no implementation verification before gate | stop because scope expands demo and acceptance is subjective | PASS only if docs require stop |

Expected: all five rows are PASS by inspection.

- [ ] **Step 7: Commit final verification record only if files changed during verification**

If verification required doc edits, commit the corrections:

```bash
git add README.md AGENTS.md docs/agents
git commit -m "docs: tighten framework verification"
```

Expected: commit succeeds only when Step 1 through Step 6 required edits. If no edits were needed, do not create an empty commit.

## Self-Review Checklist

Before handing off implementation, verify this plan covers the spec:

- `README.md` front door exists in Task 5.
- Human path routes to `docs/tutorial/README.md` in Task 5.
- Agent path routes to `AGENTS.md` in Task 5.
- `AGENTS.md` first-read file exists in Task 1.
- `docs/agents/README.md` exists in Task 2.
- `docs/agents/task-types.md` exists in Task 2.
- `docs/agents/task-template.md` exists in Task 2.
- `docs/agents/development-protocol.md` exists in Task 3.
- `docs/agents/verification.md` exists in Task 3.
- `docs/agents/boundaries.md` exists in Task 4.
- Short task format with `验收:` and `Acceptance:` exists in Task 2.
- Six task examples exist in Task 2.
- Gray-area classification exists in Task 2.
- Experiment ID rules exist in Task 2.
- Short task to plan contract exists in Task 3.
- Fresh and warmed workspace verification exists in Task 3.
- Document size guardrails are verified in Task 6.
- Static-site files remain absent in Task 6.
- Intake-case table exists in Task 6.
- `git diff --check`, shell syntax, unresolved-marker scan, and clean worktree checks are covered in Task 6.

## Execution Options

Recommended execution: Subagent-driven by task, because write sets are mostly disjoint.

Inline execution is also safe because this plan is documentation-only and each task has focused file ownership.
