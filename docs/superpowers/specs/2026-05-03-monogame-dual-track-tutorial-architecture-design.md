# MonoGame Dual-Track Tutorial Architecture Design

Date: 2026-05-03
Status: Draft for strict review

## Purpose

Turn `game_demo` into a serious dual-track MonoGame tutorial product.

The repository must support two first-class users:

- Humans learning MonoGame through a runnable tutorial experience.
- Agents extending or maintaining the tutorial without requiring the user to repeat long context prompts.

This replaces the earlier "Markdown-first framework only" direction. The prior `2026-05-02-monogame-study-framework-design.md` remains useful historical context, but its "no tutorial website in this pass" non-goal is no longer valid.

## Architecture Score Target

This design is not acceptable unless it reaches at least 95/100 under the scoring matrix near the end of this document.

The score must be evidence-based. A high score cannot be claimed just because the idea sounds plausible.

## Reference Set

Primary reference:

- `../pi_demo/pi-tutorial`: Astro + MDX tutorial site, runnable demos, reference validation script, content collection shape.

Secondary references:

- `../hermony`: long-form tutorial app with sections, playgrounds, quizzes, tests, and prose components.
- `../claude_study`: chapter navigation, progress, exercise cards, prompt/code comparison components.
- `../my_hermes_study`: compact Vite tutorial with reusable UI components and playgrounds.

Sample-code mapping reference:

- `../hermony_demo`: tutorial chapter to runnable sample repository/tag mapping.

Explicitly excluded as a structural reference:

- `../game_design`: still in design and gate-building phase; useful as a warning against over-documenting the architecture, not as a model for this tutorial product.

## Current Problem

The repository already has valuable code and documentation:

- `experiments/e01-*` through `experiments/e07-*`
- `experiments/e10-publishing`
- `demo/integrated-demo`
- `docs/tutorial/*.md`
- `docs/agents/*.md`
- `tools/check-env.sh`
- `tools/check-tutorial.sh`

The missing piece is not more standalone Markdown. The missing piece is a shared course kernel that binds the human tutorial path, agent task path, runnable code, verification commands, and expected evidence.

Without that shared kernel:

- Humans see tutorial prose but no product-grade tutorial surface.
- Agents see operating rules but no lesson-scoped work packets.
- Lessons can drift away from experiments.
- Verification can pass while the tutorial structure is broken.
- Future work can accidentally expand the game instead of strengthening the tutorial.

## Non-Goals

This architecture does not authorize:

- Expanding `demo/integrated-demo` into a production game.
- Adding Godot or another engine track.
- Adding new MonoGame experiments unless a lesson gap and spec require it.
- Rewriting all existing tutorial content in one pass.
- Building a marketing landing page.
- Adding video generation or screenshot automation in the first implementation phase.
- Turning the repository into a gate-heavy design governance repo.

## Core Architecture

The repository becomes a manifest-driven dual-track tutorial product.

```text
course manifest
  -> human lessons
  -> agent task packets
  -> experiment/demo code
  -> verification commands
  -> expected evidence
  -> tutorial site navigation
```

The course manifest is the spine. The tutorial site and agent workflow are consumers of the same data, not parallel truths.

## Required Directory Shape

```text
course/
  manifest.json
  schema.json
  lessons/
    01-setup.mdx
    02-first-window.mdx
    03-game-loop.mdx
  agent-tasks/
    01-setup.md
    02-first-window.md
    03-game-loop.md
  evidence/
    03-game-loop/
      expected-overlay.png

tutorial-site/
  package.json
  astro.config.mjs
  tsconfig.json
  src/
    content.config.ts
    data/loadCourse.ts
    layouts/TutorialLayout.astro
    pages/index.astro
    pages/[...lesson].astro
    components/
      CourseNav.astro
      LessonHeader.astro
      CommandBlock.astro
      KeyFileList.astro
      Checkpoint.astro
      EvidencePanel.astro

docs/
  tutorial/      # existing material; migration source and historical fallback
  agents/        # general agent protocol, not per-lesson packets

tools/
  check-env.sh
  check-tutorial.sh
  check-course.sh
  check-course.mjs
```

The first implementation phase may include only a subset of lessons, but the directory roles must be stable from the start.

## Dual-Track Contract

Every lesson must have both tracks.

Human track:

- A lesson file under `course/lessons/`.
- A rendered route in `tutorial-site/`.
- Required sections for learning flow.
- Commands to run.
- Expected observation or explicit pending evidence.
- Checkpoint.

Agent track:

- A task packet under `course/agent-tasks/`.
- Allowed files.
- Blocked files.
- Spec-required file patterns.
- Verification commands.
- Acceptance criteria.
- Failure handling.
- Report format.

Completion rule:

- A lesson with only human content is incomplete.
- A lesson with only an agent task is incomplete.
- A lesson without code or command mapping is incomplete unless explicitly marked as orientation.
- A lesson with missing evidence must mark evidence as `pending` with a reason.

## Manifest Schema Contract

`course/schema.json` should be a JSON Schema file. The first implementation may write a hand-authored schema rather than pulling in a validation library, but `tools/check-course.mjs` must enforce the same required fields.

Top-level shape:

```json
{
  "version": 1,
  "title": "MonoGame Study Framework",
  "tracks": ["human", "agent"],
  "lessons": []
}
```

Required top-level fields:

| Field | Type | Rule |
| --- | --- | --- |
| `version` | integer | Must be `1` for this architecture. |
| `title` | string | Non-empty. |
| `tracks` | string array | Must contain exactly `human` and `agent`. |
| `lessons` | array | Non-empty after Phase B. |

Lesson shape:

```json
{
  "id": "03-game-loop",
  "order": 3,
  "title": "Game Loop",
  "summary": "Understand MonoGame Update/Draw cadence through the e01 experiment.",
  "kind": "experiment",
  "human": {
    "path": "course/lessons/03-game-loop.mdx",
    "requiredSections": [
      "Goal",
      "Run",
      "Observe",
      "Key Files",
      "Walkthrough",
      "Common Failures",
      "Exercise",
      "Checkpoint",
      "Next"
    ]
  },
  "agent": {
    "taskPath": "course/agent-tasks/03-game-loop.md",
    "allowedFiles": [
      "course/lessons/03-game-loop.mdx",
      "course/agent-tasks/03-game-loop.md"
    ],
    "blockedFiles": [
      "demo/integrated-demo/**"
    ],
    "specRequiredFiles": [
      "experiments/e01-game-loop/**",
      "experiments/e01-game-loop.Tests/**"
    ]
  },
  "code": {
    "projects": [
      "experiments/e01-game-loop/E01GameLoop.csproj"
    ],
    "tests": [
      "experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj"
    ],
    "keyFiles": [
      "experiments/e01-game-loop/Game1.cs",
      "experiments/e01-game-loop/GameLoopState.cs",
      "experiments/e01-game-loop.Tests/Program.cs"
    ]
  },
  "commands": {
    "run": [
      "dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj"
    ],
    "verify": [
      "dotnet build GameDemo.sln -m:1",
      "dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj"
    ]
  },
  "evidence": {
    "status": "pending",
    "reason": "First implementation creates structure before screenshot capture is automated.",
    "expectedPaths": [
      "course/evidence/03-game-loop/expected-overlay.png"
    ]
  }
}
```

Lesson field rules:

| Field | Type | Rule |
| --- | --- | --- |
| `id` | string | Lowercase slug; unique; should match file prefix. |
| `order` | integer | Unique; increasing course order. |
| `title` | string | Non-empty. |
| `summary` | string | Non-empty; used by tutorial site index. |
| `kind` | enum | `orientation`, `setup`, `experiment`, `capstone`, `appendix`. |
| `human.path` | string | Must exist. |
| `human.requiredSections` | array | Must be non-empty; every heading must exist in the lesson. |
| `agent.taskPath` | string | Must exist. |
| `agent.allowedFiles` | array | Must be non-empty. |
| `agent.blockedFiles` | array | May be empty, but must exist. |
| `agent.specRequiredFiles` | array | May be empty for docs-only changes, but must exist. |
| `code.projects` | array | Required for `experiment` and `capstone`. |
| `code.tests` | array | Required when a matching test project exists. |
| `code.keyFiles` | array | Must exist; used by human and agent views. |
| `commands.run` | array | Required unless `kind` is `orientation` or `appendix`. |
| `commands.verify` | array | Must be non-empty. |
| `evidence.status` | enum | `available`, `pending`, `notApplicable`. |
| `evidence.reason` | string | Required unless status is `available`. |
| `evidence.expectedPaths` | array | Required when status is `available` or `pending`. |

## End-to-End Sample: `03-game-loop`

This sample is the acceptance model for the first implementation slice.

Human lesson file:

```text
course/lessons/03-game-loop.mdx
```

Required content outline:

```mdx
---
id: 03-game-loop
title: Game Loop
---

## Goal
Learn how MonoGame separates simulation work in `Update` from rendering work in `Draw`.

## Run
<CommandBlock command="dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj" />

## Observe
The window opens and displays loop/timing state. The learner should see the overlay update while the app runs.

## Key Files
<KeyFileList files={[
  "experiments/e01-game-loop/Game1.cs",
  "experiments/e01-game-loop/GameLoopState.cs",
  "experiments/e01-game-loop.Tests/Program.cs"
]} />

## Walkthrough
Explain `Update`, `Draw`, fixed timestep expectations, and why testable state is separated from the MonoGame window.

## Common Failures
Cover missing SDK, DesktopGL launch failures, and GUI smoke expectations.

## Exercise
Ask the learner to identify where elapsed time is accumulated and where drawing reads state.

## Checkpoint
The learner can explain why the test project validates loop state without opening a window.

## Next
Move to rendering.
```

Agent task packet:

```text
course/agent-tasks/03-game-loop.md
```

Required content outline:

```md
# Agent Task: 03 Game Loop

## Task
Maintain or improve the Game Loop lesson without changing the experiment behavior.

## Context
This lesson maps to `experiments/e01-game-loop` and its test project.

## Allowed Files
- `course/lessons/03-game-loop.mdx`
- `course/agent-tasks/03-game-loop.md`
- `course/manifest.json` only when updating metadata for this lesson.

## Blocked Files
- `demo/integrated-demo/**`
- unrelated lesson files

## Spec Required
Any change under `experiments/e01-game-loop/**` or `experiments/e01-game-loop.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj`
- `./tools/check-course.sh`

## Acceptance
- Human lesson still contains every required section from the manifest.
- Agent task packet still contains every required section.
- Manifest paths still resolve.
- Verification commands are reported with results.

## Failure Handling
If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run non-GUI checks.

## Report Format
Report changed files, verification commands, and any unverified visual evidence.
```

This sample proves the intended relationship:

```text
manifest entry
  -> human MDX lesson
  -> agent task packet
  -> e01 code and tests
  -> check-course structural verifier
  -> dotnet build/test behavior verifier
```

## Tutorial Site Information Architecture

The tutorial site uses Astro because the project is content-led and `pi_demo/pi-tutorial` already shows this shape works for code-heavy lessons.

Pages:

| Page | Purpose |
| --- | --- |
| `tutorial-site/src/pages/index.astro` | Course index, environment command, lesson list, track status. |
| `tutorial-site/src/pages/[...lesson].astro` | Render a lesson by manifest id. |

Layouts:

| Layout | Purpose |
| --- | --- |
| `TutorialLayout.astro` | Page chrome, nav, table of contents, previous/next. |

Components:

| Component | Purpose |
| --- | --- |
| `CourseNav.astro` | Manifest-driven ordered lesson navigation. |
| `LessonHeader.astro` | Title, summary, kind, related code paths. |
| `CommandBlock.astro` | Copyable run/verify command block. |
| `KeyFileList.astro` | Links to relevant experiment files. |
| `Checkpoint.astro` | End-of-lesson learning check. |
| `EvidencePanel.astro` | Shows available evidence or explicit pending status. |

Data flow:

```text
course/manifest.json
  -> tutorial-site/src/data/loadCourse.ts
  -> index route lesson list
  -> lesson route metadata
  -> MDX content render
```

The site must not maintain a second course list. If a lesson is not in the manifest, it does not appear in navigation.

## Agent Workflow

Root `AGENTS.md` remains the agent entrypoint, but it should not carry per-lesson details.

Agent startup:

1. Read `AGENTS.md`.
2. Read `course/manifest.json`.
3. Classify the user request:
   - exact lesson id
   - lesson title keyword
   - task type from `docs/agents/task-types.md`
4. If lesson-scoped, open the matching `course/agent-tasks/<lesson-id>.md`.
5. Apply allowed/blocked/spec-required rules.
6. Run the manifest verification commands that match the change.
7. Report evidence and any skipped checks.

Short task examples:

```text
完善 03-game-loop 的 Checkpoint
```

Agent behavior:

- Map to lesson `03-game-loop`.
- Open `course/agent-tasks/03-game-loop.md`.
- Edit only allowed files.
- Run `./tools/check-course.sh`.
- Run non-GUI verify commands if required.

```text
让 e01 的游戏循环显示更多计时信息
```

Agent behavior:

- Map to `03-game-loop`.
- Detect requested experiment behavior change.
- Stop for spec because `experiments/e01-game-loop/**` is in `specRequiredFiles`.

## Verification Design

### `tools/check-course.sh`

Shell wrapper:

```sh
#!/usr/bin/env bash
set -euo pipefail
node tools/check-course.mjs
```

### `tools/check-course.mjs`

Responsibilities:

- Parse `course/manifest.json`.
- Validate top-level fields.
- Validate lesson uniqueness and ordering.
- Validate every path exists.
- Validate required human headings.
- Validate required agent task headings.
- Validate command arrays are non-empty where required.
- Validate evidence status and pending reason.
- Fail if any lesson has only one track.

What it proves:

- The dual-track course structure is internally connected.
- Tutorial site navigation has valid source data.
- Agent task packets exist and have required operating sections.
- Lesson-to-code references point to real files.

What it does not prove:

- MonoGame windows launch successfully.
- Visual output matches screenshots.
- The lesson prose is pedagogically excellent.
- Agent edits are semantically correct.
- Publish output works.

Those remain covered by human review, `dotnet build`, lesson-specific test commands, and `tools/check-tutorial.sh`.

### Existing Checks

`tools/check-env.sh` remains environment readiness.

`tools/check-tutorial.sh` remains full MonoGame dry-run and publish verification. It should not become the structural course verifier.

## Migration Rules

Existing `docs/tutorial/*.md` files are migration sources, not waste.

Rules:

- Keep `docs/tutorial/` until all lessons are migrated and the site is usable.
- Do not delete old tutorial files in the first implementation phase.
- Migrate one lesson at a time into `course/lessons/*.mdx`.
- Each migrated lesson must get a manifest entry and an agent task packet in the same change.
- README should route humans to the tutorial site once at least the initial lesson slice is available.
- README should route agents to `AGENTS.md`, which then routes lesson work through the manifest.

Initial migration slice:

| Existing file | New lesson | Reason |
| --- | --- | --- |
| `docs/tutorial/01-setup.md` | `course/lessons/01-setup.mdx` | Environment setup is the first user friction point. |
| `docs/tutorial/03-game-loop.md` | `course/lessons/03-game-loop.mdx` | Good end-to-end experiment sample with tests. |

Deferred:

- Full screenshot capture.
- Full content rewrite.
- Every lesson route.
- Video or Remotion.
- New experiments.

## Mainline Drift Guard

The architecture must keep the repository on the MonoGame tutorial mainline.

Rules:

- No manifest entry, no new lesson.
- No agent task packet, no completed lesson.
- No human lesson, no completed agent task.
- Experiment code changes require a spec unless the manifest explicitly permits the file.
- `demo/integrated-demo` remains capstone evidence, not a product game.
- Godot is out of scope.
- `game_design` is not a structural model.
- Tutorial site first screen is a course, not a landing page.
- Agent docs must not be mixed into human lesson prose.
- Human lesson prose must not be required reading for routine agent maintenance.

## Risks And Mitigations

| Risk | Mitigation |
| --- | --- |
| Adding Astro introduces Node dependency surface. | Keep site isolated in `tutorial-site/`; existing .NET checks remain valid. |
| Manifest becomes bureaucratic overhead. | Keep fields tied to actual consumers: site, agent task, verifier. |
| Agent line drifts from human line. | `check-course` fails if either track is missing. |
| Visual evidence slows first implementation. | Allow explicit `pending` with required reason; do not silently omit evidence. |
| Existing docs become stale. | Treat old docs as migration source; README must clarify current entrypoints. |
| Tutorial becomes a game expansion project. | Enforce spec-required patterns for experiment/demo changes. |

## Implementation Phases

Phase A: Architecture acceptance

- Add this spec.
- Self-review for contradictions, missing fields, and scope creep.
- Get user approval before implementation planning.

Phase B: Course kernel slice

- Add `course/manifest.json`.
- Add `course/schema.json`.
- Add `course/lessons/01-setup.mdx`.
- Add `course/lessons/03-game-loop.mdx`.
- Add matching agent task packets.

Phase C: Structural verifier

- Add `tools/check-course.sh`.
- Add `tools/check-course.mjs`.
- Validate the Phase B slice.

Phase D: Tutorial site shell

- Add `tutorial-site/`.
- Render course index from manifest.
- Render the two migrated lessons.

Phase E: Remaining migration

- Migrate remaining lessons one at a time.
- Keep human and agent tracks paired.

Phase F: Evidence hardening

- Add screenshots or visual evidence.
- Choose one approved evidence-capture policy before removing `pending` evidence status from migrated lessons:
  - manual checked-in screenshots,
  - scripted MonoGame capture,
  - Playwright-only site screenshots for rendered lesson pages.

## Scoring Matrix

| Category | Max | Score | Evidence |
| --- | ---: | ---: | --- |
| Dual-track structure | 25 | 25 | Manifest requires `human` and `agent`; completion rules reject one-sided lessons. |
| Tutorial product shape | 15 | 13 | Astro site IA is defined; component list and manifest data flow are specified. Two points held back because visual wireframes and final styling rules are not included. |
| Agent executability | 20 | 18 | Task packet sections, startup workflow, allowed/blocked/spec-required rules are defined. Two points held back because short-task matching is intentionally deterministic and the exact matching implementation belongs in the implementation plan. |
| Shared manifest kernel | 15 | 15 | Detailed top-level and lesson field rules, sample entry, and verifier coverage are defined. |
| Existing repo compatibility | 10 | 10 | Existing docs/checks remain; migration is incremental; code experiments are not rewritten. |
| Mainline drift prevention | 10 | 10 | Explicit drift guard blocks game expansion, Godot track, one-sided lessons, and `game_design` structural dependency. |
| Implementation control | 5 | 5 | Phases A-F are narrow and ordered; first slice is only setup + game loop. |
| Total | 100 | 96 | Architecture score only; implementation score remains zero until built and verified. |

## Strict Review Result

Architecture content score: 96/100.

This does not mean the project is implemented.

This does mean the architecture is sufficiently specified to become the basis for an implementation plan after approval.

The four held-back points are intentional:

- No visual wireframe for the tutorial site yet.
- No final styling rules for the tutorial site yet.
- Short-task matching is deterministic and simple in v1 rather than NLP-heavy.
- The exact short-task matching implementation belongs in the implementation plan, not this architecture spec.

These are acceptable tradeoffs for a first implementation plan because they reduce scope and protect the MonoGame tutorial mainline.

## Prompt-To-Artifact Audit

This section maps the current user requirements to concrete design evidence.

| Requirement | Evidence In This Spec | Status |
| --- | --- | --- |
| Build a serious architecture design, not another loose idea. | `Core Architecture`, `Required Directory Shape`, `Manifest Schema Contract`, `Tutorial Site Information Architecture`, `Agent Workflow`, and `Implementation Phases`. | Covered. |
| Keep human and agent tutorial tracks equally important. | `Dual-Track Contract` requires every lesson to have both `course/lessons/` and `course/agent-tasks/` artifacts. | Covered. |
| Do not mix human tutorial prose with agent operating instructions. | `Mainline Drift Guard` says agent docs must not be mixed into human prose, and human prose must not be required reading for routine agent maintenance. | Covered. |
| Prevent agent users from needing long repeated prompts. | `Agent Workflow` routes short tasks through `AGENTS.md`, `course/manifest.json`, and lesson task packets. | Covered. |
| Avoid copying the wrong sibling project. | `Reference Set` excludes `../game_design` as a structural reference and makes `../pi_demo/pi-tutorial` the primary reference. | Covered. |
| Do not make a pure Markdown tutorial. | `Required Directory Shape` requires `tutorial-site/`, Astro pages, components, and manifest-driven rendering. | Covered. |
| Preserve the MonoGame mainline. | `Non-Goals` and `Mainline Drift Guard` block Godot, product-game expansion, and casual experiment additions. | Covered. |
| Make scoring rigorous and evidence-based. | `Architecture Score Target`, `Scoring Matrix`, and `Strict Review Result` distinguish architecture score from implementation score. | Covered. |
| Include an approval gate. | `Approval Gate` blocks implementation planning until user review and acceptance. | Covered. |
| Avoid treating checks as proof beyond their coverage. | `Verification Design` lists what `check-course` proves and what it does not prove. | Covered. |
| Provide an end-to-end sample. | `End-to-End Sample: 03-game-loop` includes human lesson outline, agent task packet outline, code/test mapping, and verification relation. | Covered. |
| Keep implementation incremental. | `Implementation Phases` starts with architecture acceptance, then a two-lesson course kernel slice, then verifier, then tutorial site shell. | Covered. |

Uncovered by design on purpose:

- Actual implementation files do not exist yet.
- The tutorial site is not built yet.
- Visual evidence is not captured yet.
- The implementation plan is not written yet.

These are not defects in the architecture spec. They are explicitly deferred until after approval.

## Approval Gate

Do not write the implementation plan until this spec is reviewed and approved.

Approval question:

Is this architecture accepted as the `game_demo` dual-track tutorial direction?
