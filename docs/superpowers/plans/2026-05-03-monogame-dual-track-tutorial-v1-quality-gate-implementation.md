# MonoGame Dual-Track Tutorial V1 Quality Gate Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Convert `game_demo` from a partial two-lesson course shell into a complete 00-10 dual-track MonoGame tutorial whose final audit scores content quality and mainline alignment at 95/100 or higher.

**Architecture:** `course/manifest.json` becomes the course control plane. `course/lessons/` holds the human tutorial, `course/agent-tasks/` holds per-lesson agent packets, `tutorial-site/` renders only manifest-backed lessons, and `tools/check-course.mjs` enforces the canonical 00-10 contract before runtime checks and scoring.

**Tech Stack:** MonoGame DesktopGL on .NET 10, Bash, Node.js ESM verifier scripts, Astro 5 with MDX content collections, Markdown/MDX course content.

---

## Source Spec

Implement:

- `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md`

Treat this prior spec as superseded background only:

- `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-architecture-design.md`

## Review Gates

Every task report must include:

- objective restatement,
- prompt-to-artifact checklist for the task,
- evidence table with paths or command output,
- hard cap check,
- content quality score,
- mainline alignment score,
- counterargument review,
- required corrections when either score is below 95.

Do not use a passing command as proof beyond its stated coverage.

## File Structure

### Course Source

- Modify: `course/manifest.json`
  - Add canonical 00-10 lesson entries.
  - Add `migrationSource` and `route` fields.
  - Keep run, verify, code, evidence, human, and agent metadata as the single source for site and agent routing.

- Modify: `course/schema.json`
  - Add schema coverage for `migrationSource` and `route`.
  - Require exactly the fields consumed by verifier and site.

- Create or modify: `course/lessons/*.mdx`
  - One human lesson per canonical lesson.
  - Required headings: `Goal`, `Run`, `Observe`, `Expected Visual State`, `Key Files`, `Walkthrough`, `Common Failures`, `Exercise`, `Checkpoint`, `Next`.

- Create or modify: `course/agent-tasks/*.md`
  - One agent packet per canonical lesson.
  - Required headings: `Task`, `Context`, `Allowed Files`, `Blocked Files`, `Spec Required`, `Commands`, `Acceptance`, `Failure Handling`, `Report Format`.

- Create: `course/evidence/<lesson-id>/expected-state.md`
  - Textual expected visual or command state for each canonical lesson.
  - Screenshots are not required for v1, but this file makes visual expectation explicit and inspectable.

### Verifier

- Modify: `tools/check-course.mjs`
  - Enforce canonical lesson set and exact order.
  - Validate schema JSON, manifest paths, route uniqueness, migration sources, lesson headings, agent headings, code paths, command fields, evidence fields, site route files, and README primary-entrypoint truth.

- Modify: `tools/check-course.sh`
  - Keep it as the Bash wrapper for the Node verifier.
  - Use `set -euo pipefail` if not already present.

### Tutorial Site

- Modify: `tutorial-site/src/data/loadCourse.ts`
  - Add `migrationSource` and `route`.
  - Keep lesson sorting by `order`.

- Modify: `tutorial-site/src/pages/index.astro`
  - Render all manifest lessons.
  - State that `course/` is the course source.

- Modify: `tutorial-site/src/pages/[...lesson].astro`
  - Use `lesson.route` for static paths.
  - Render agent task link, run commands, verify commands, key files, evidence, and MDX content.

- Modify as needed: `tutorial-site/src/components/*.astro`
  - Keep components simple and manifest-driven.

### Entrypoints And Reports

- Modify: `README.md`
  - State current route honestly during migration.
  - After v1, route humans to `tutorial-site` and agents to `AGENTS.md`.
  - Keep `docs/tutorial/` as legacy source/history, not primary course truth.

- Modify: `AGENTS.md`
  - Keep root agent startup tied to `course/manifest.json`.
  - Add the quality-gate requirement for lesson-scoped tasks.

- Modify: `docs/tutorial/README.md`
  - Mark legacy status clearly after migration.

- Modify: `docs/tutorial/ROADMAP.md`
  - Replace old "v1 is complete" wording with quality-gate-aware state.

- Create: `docs/reports/tutorial-v1-quality-audit.md`
  - Final evidence table and two-score audit.

## Canonical Lesson Matrix

Use this matrix for `course/manifest.json`, lesson files, task packets, and verifier constants.

| Order | Id | Title | Kind | Migration source | Route |
| ---: | --- | --- | --- | --- | --- |
| 0 | `00-intro` | `Intro` | `orientation` | `docs/tutorial/00-intro.md` | `00-intro` |
| 1 | `01-setup` | `Setup` | `setup` | `docs/tutorial/01-setup.md` | `01-setup` |
| 2 | `02-first-window` | `First Window` | `experiment` | `docs/tutorial/02-first-window.md` | `02-first-window` |
| 3 | `03-game-loop` | `Game Loop` | `experiment` | `docs/tutorial/03-game-loop.md` | `03-game-loop` |
| 4 | `04-rendering` | `Rendering` | `experiment` | `docs/tutorial/04-rendering.md` | `04-rendering` |
| 5 | `05-input` | `Input` | `experiment` | `docs/tutorial/05-input.md` | `05-input` |
| 6 | `06-content-pipeline` | `Content Pipeline` | `experiment` | `docs/tutorial/06-content-pipeline.md` | `06-content-pipeline` |
| 7 | `07-audio` | `Audio` | `experiment` | `docs/tutorial/07-audio.md` | `07-audio` |
| 8 | `08-camera-collision-animation` | `Camera, Collision, And Animation` | `experiment` | `docs/tutorial/08-camera-collision-animation.md` | `08-camera-collision-animation` |
| 9 | `09-publishing` | `Publishing` | `experiment` | `docs/tutorial/09-publishing.md` | `09-publishing` |
| 10 | `10-integrated-demo` | `Integrated Demo` | `capstone` | `docs/tutorial/10-integrated-demo.md` | `10-integrated-demo` |

## Required Human Sections

Every lesson must contain these exact headings:

```text
Goal
Run
Observe
Expected Visual State
Key Files
Walkthrough
Common Failures
Exercise
Checkpoint
Next
```

## Required Agent Sections

Every agent task packet must contain these exact headings:

```text
Task
Context
Allowed Files
Blocked Files
Spec Required
Commands
Acceptance
Failure Handling
Report Format
```

## Lesson Runtime Mapping

Use this mapping in manifest `code`, `commands`, lessons, and task packets.

| Lesson | Projects | Tests | Primary verify |
| --- | --- | --- | --- |
| `00-intro` | `demo/integrated-demo/IntegratedDemo.csproj` | `demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj` | `env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore` |
| `01-setup` | none | none | `./tools/check-env.sh` |
| `02-first-window` | `experiments/e01-game-loop/E01GameLoop.csproj` | `experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj` | `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj` |
| `03-game-loop` | `experiments/e01-game-loop/E01GameLoop.csproj` | `experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj` | `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj` |
| `04-rendering` | `experiments/e02-2d-rendering/E02Rendering.csproj` | `experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj` | `dotnet run --project experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj` |
| `05-input` | `experiments/e03-input/E03Input.csproj` | `experiments/e03-input.Tests/E03Input.Tests.csproj` | `dotnet run --project experiments/e03-input.Tests/E03Input.Tests.csproj` |
| `06-content-pipeline` | `experiments/e05-content-pipeline/E05ContentPipeline.csproj` | `experiments/e05-content-pipeline.Tests/E05ContentPipeline.Tests.csproj` | `dotnet run --project experiments/e05-content-pipeline.Tests/E05ContentPipeline.Tests.csproj` |
| `07-audio` | `experiments/e04-audio/E04Audio.csproj` | `experiments/e04-audio.Tests/E04Audio.Tests.csproj` | `dotnet run --project experiments/e04-audio.Tests/E04Audio.Tests.csproj` |
| `08-camera-collision-animation` | `experiments/e06-camera-and-collision/E06CameraAndCollision.csproj`, `experiments/e07-animation/E07Animation.csproj` | `experiments/e06-camera-and-collision.Tests/E06CameraAndCollision.Tests.csproj`, `experiments/e07-animation.Tests/E07Animation.Tests.csproj` | both test projects |
| `09-publishing` | `experiments/e10-publishing/E10Publishing.csproj` | `experiments/e10-publishing.Tests/E10Publishing.Tests.csproj` | `dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false` |
| `10-integrated-demo` | `demo/integrated-demo/IntegratedDemo.csproj` | `demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj` | `dotnet run --project demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj` |

---

### Task 1: Correct Entrypoint Truth Before Migration

**Files:**
- Modify: `README.md`
- Modify: `AGENTS.md`
- Modify: `docs/tutorial/README.md`
- Modify: `docs/tutorial/ROADMAP.md`

- [ ] **Step 1: Change README current-state language**

Edit `README.md` so it states:

```md
## Current Status

The repository is in v1 tutorial migration.

- `course/` is the intended canonical course source.
- `course/` currently contains only the migrated lesson slice until the v1 migration plan is complete.
- `docs/tutorial/` is the legacy migration source and remains readable during migration.
- `tutorial-site/` renders the current manifest-backed course slice; it is not a complete 00-10 course until the v1 quality gate is implemented.
```

Keep `For Humans` honest during migration:

```md
## For Humans

During migration, use the legacy Markdown tutorial when you need the complete 00-10 path:

- [`docs/tutorial/README.md`](docs/tutorial/README.md)

Use the tutorial site to inspect the current manifest-backed course slice:

```bash
cd tutorial-site
npm install
npm run dev
```
```

- [ ] **Step 2: Add quality-gate pointer to AGENTS**

Add this near the top of `AGENTS.md` after `Default Role`:

```md
## Active Quality Gate

Before claiming v1 tutorial completion, read:

- `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md`

Completion requires evidence-backed scores of at least 95/100 for both content quality and mainline alignment.
```

- [ ] **Step 3: Mark legacy tutorial README**

Add this under the `# MonoGame Tutorial` heading in `docs/tutorial/README.md`:

```md
> Legacy migration source: this Markdown path remains readable during v1 migration, but the canonical v1 course source is `course/` once the quality gate is implemented.
```

- [ ] **Step 4: Correct old roadmap completion claim**

In `docs/tutorial/ROADMAP.md`, replace `Tutorial v1 is complete.` with:

```md
The legacy Markdown tutorial is complete. The dual-track v1 tutorial is not complete until the quality gate in `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md` passes.
```

- [ ] **Step 5: Verify docs-only changes**

Run:

```bash
git diff --check
rg -n "Primary course entrypoint|Tutorial v1 is complete|site is complete|full 00-10" README.md docs/tutorial/README.md docs/tutorial/ROADMAP.md
```

Expected:

- `git diff --check` exits 0.
- `rg` must not find wording that claims the partial site is already the complete v1 route.

- [ ] **Step 6: Commit**

```bash
git add README.md AGENTS.md docs/tutorial/README.md docs/tutorial/ROADMAP.md
git commit -m "docs: clarify tutorial migration entrypoints"
```

### Task 2: Upgrade Course Schema And Verifier To Enforce V1

**Files:**
- Modify: `course/schema.json`
- Modify: `tools/check-course.mjs`
- Modify: `tools/check-course.sh`

- [ ] **Step 1: Add manifest fields to schema**

In `course/schema.json`, add `migrationSource` and `route` to the lesson `required` list:

```json
"required": [
  "id",
  "order",
  "title",
  "summary",
  "kind",
  "migrationSource",
  "route",
  "human",
  "agent",
  "code",
  "commands",
  "evidence"
]
```

Add properties:

```json
"migrationSource": { "type": "string", "minLength": 1 },
"route": { "type": "string", "pattern": "^[0-9]{2}-[a-z0-9]+(?:-[a-z0-9]+)*$" }
```

- [ ] **Step 2: Add canonical constants to verifier**

In `tools/check-course.mjs`, add this after `requiredAgentSections`:

```js
const requiredHumanSections = [
  'Goal',
  'Run',
  'Observe',
  'Expected Visual State',
  'Key Files',
  'Walkthrough',
  'Common Failures',
  'Exercise',
  'Checkpoint',
  'Next',
];

const canonicalLessons = [
  { order: 0, id: '00-intro' },
  { order: 1, id: '01-setup' },
  { order: 2, id: '02-first-window' },
  { order: 3, id: '03-game-loop' },
  { order: 4, id: '04-rendering' },
  { order: 5, id: '05-input' },
  { order: 6, id: '06-content-pipeline' },
  { order: 7, id: '07-audio' },
  { order: 8, id: '08-camera-collision-animation' },
  { order: 9, id: '09-publishing' },
  { order: 10, id: '10-integrated-demo' },
];
```

- [ ] **Step 3: Parse schema and validate canonical coverage**

Add a schema path near `manifestPath`:

```js
const schemaPath = resolve(root, 'course/schema.json');
```

Add a function:

```js
function validateCanonicalLessons(manifest) {
  const actual = [...(manifest.lessons || [])].sort((a, b) => a.order - b.order);
  const actualIds = actual.map((lesson) => lesson.id);
  const expectedIds = canonicalLessons.map((lesson) => lesson.id);

  if (actual.length !== canonicalLessons.length) {
    fail(`manifest must contain exactly ${canonicalLessons.length} lessons; found ${actual.length}`);
  }

  for (let index = 0; index < canonicalLessons.length; index += 1) {
    const expected = canonicalLessons[index];
    const actualLesson = actual[index];
    if (!actualLesson) {
      fail(`missing canonical lesson at order ${expected.order}: ${expected.id}`);
      continue;
    }
    if (actualLesson.order !== expected.order || actualLesson.id !== expected.id) {
      fail(`canonical lesson mismatch at index ${index}: expected ${expected.order}/${expected.id}, found ${actualLesson.order}/${actualLesson.id}`);
    }
  }

  for (const id of actualIds) {
    if (!expectedIds.includes(id)) {
      fail(`manifest contains non-v1 lesson id: ${id}`);
    }
  }
}
```

Call it immediately after `validateTopLevel(manifest)`.

- [ ] **Step 4: Validate route, migration source, schema, site files, and README**

Add these checks:

```js
function validateSchemaFile() {
  const schema = readJson(schemaPath);
  if (!schema) return;
  assertNonEmptyString(schema.title, 'course/schema.json.title');
}

function validateSiteFiles() {
  assertPathExists('tutorial-site/src/pages/index.astro', 'tutorial site index route');
  assertPathExists('tutorial-site/src/pages/[...lesson].astro', 'tutorial site lesson route');
}

function validateReadmeTruth() {
  const readmePath = resolve(root, 'README.md');
  const content = existsSync(readmePath) ? readFileSync(readmePath, 'utf8') : '';
  if (/Primary course entrypoint:\s*```bash\s*cd tutorial-site/ms.test(content) && !content.includes('current manifest-backed course slice')) {
    fail('README routes humans to tutorial-site without stating that the site is only the current manifest-backed slice during migration');
  }
}
```

Call `validateSchemaFile()`, `validateSiteFiles()`, and `validateReadmeTruth()` before final error handling.

- [ ] **Step 5: Extend lesson validation**

In `validateLesson`, add:

```js
assertNonEmptyString(lesson.migrationSource, `${lesson.id}.migrationSource`);
assertPathExists(lesson.migrationSource, `${lesson.id}.migrationSource`);
assertNonEmptyString(lesson.route, `${lesson.id}.route`);
if (lesson.route !== lesson.id) {
  fail(`${lesson.id}.route must match lesson id for v1`);
}
```

Replace use of `lesson.human.requiredSections` as the only heading set with validation against `requiredHumanSections`:

```js
const sections = assertNonEmptyArray(lesson.human.requiredSections, `${lesson.id}.human.requiredSections`);
for (const required of requiredHumanSections) {
  if (!sections.includes(required)) {
    fail(`${lesson.id}.human.requiredSections is missing required section: ${required}`);
  }
}
```

Then keep checking every listed heading exists in the lesson file.

- [ ] **Step 6: Strengthen shell wrapper**

Make `tools/check-course.sh`:

```bash
#!/usr/bin/env bash
set -euo pipefail

node tools/check-course.mjs
```

- [ ] **Step 7: Run red check before migration**

Run:

```bash
./tools/check-course.sh
```

Expected now: non-zero exit with errors about missing canonical lessons and missing `migrationSource` / `route` on existing entries. This red result proves the verifier catches the current incomplete state.

Do not commit this task yet. Continue to Task 3 before committing, so the repository does not keep a broken verifier without matching course data.

### Task 3: Expand Manifest And Course Files To Full 00-10 Structure

**Files:**
- Modify: `course/manifest.json`
- Create/modify: `course/lessons/*.mdx`
- Create/modify: `course/agent-tasks/*.md`
- Create: `course/evidence/*/expected-state.md`

- [ ] **Step 1: Replace manifest with canonical 00-10 entries**

Rewrite `course/manifest.json` so it has 11 lessons in canonical order.

Each lesson entry must include:

```json
{
  "id": "04-rendering",
  "order": 4,
  "title": "Rendering",
  "summary": "Draw sprites in 2D and understand why SpriteBatch batching matters.",
  "kind": "experiment",
  "migrationSource": "docs/tutorial/04-rendering.md",
  "route": "04-rendering",
  "human": {
    "path": "course/lessons/04-rendering.mdx",
    "requiredSections": [
      "Goal",
      "Run",
      "Observe",
      "Expected Visual State",
      "Key Files",
      "Walkthrough",
      "Common Failures",
      "Exercise",
      "Checkpoint",
      "Next"
    ]
  },
  "agent": {
    "taskPath": "course/agent-tasks/04-rendering.md",
    "allowedFiles": [
      "course/lessons/04-rendering.mdx",
      "course/agent-tasks/04-rendering.md",
      "course/evidence/04-rendering/expected-state.md",
      "course/manifest.json"
    ],
    "blockedFiles": [
      "demo/integrated-demo/**",
      "course/lessons/03-game-loop.mdx",
      "course/agent-tasks/03-game-loop.md",
      "course/lessons/05-input.mdx",
      "course/agent-tasks/05-input.md"
    ],
    "specRequiredFiles": [
      "experiments/e02-2d-rendering/**",
      "experiments/e02-2d-rendering.Tests/**"
    ]
  },
  "code": {
    "projects": [
      "experiments/e02-2d-rendering/E02Rendering.csproj"
    ],
    "tests": [
      "experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj"
    ],
    "keyFiles": [
      "experiments/e02-2d-rendering/Game1.cs",
      "experiments/e02-2d-rendering/SpriteField.cs",
      "experiments/e02-2d-rendering/RenderModeState.cs",
      "experiments/e02-2d-rendering/Content/Status.spritefont"
    ]
  },
  "commands": {
    "run": [
      "env E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj --no-restore"
    ],
    "verify": [
      "dotnet build GameDemo.sln -m:1",
      "dotnet run --project experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj"
    ]
  },
  "evidence": {
    "status": "pending",
    "reason": "V1 requires textual expected visual state; checked-in screenshots are not required.",
    "expectedPaths": [
      "course/evidence/04-rendering/expected-state.md"
    ]
  }
}
```

Use the same structure for every canonical lesson, with paths and commands from the lesson runtime mapping.

- [ ] **Step 2: Create every evidence expected-state file**

Create these files:

```text
course/evidence/00-intro/expected-state.md
course/evidence/01-setup/expected-state.md
course/evidence/02-first-window/expected-state.md
course/evidence/03-game-loop/expected-state.md
course/evidence/04-rendering/expected-state.md
course/evidence/05-input/expected-state.md
course/evidence/06-content-pipeline/expected-state.md
course/evidence/07-audio/expected-state.md
course/evidence/08-camera-collision-animation/expected-state.md
course/evidence/09-publishing/expected-state.md
course/evidence/10-integrated-demo/expected-state.md
```

Each file must contain:

```md
# Expected State: <Lesson Title>

## Visual State

<Describe the DesktopGL window state, or say "No GUI window; command output is the expected state.">

## Command Evidence

<List the stdout lines or command result the learner should compare against.>
```

- [ ] **Step 3: Normalize existing two lesson files**

Update `course/lessons/01-setup.mdx` and `course/lessons/03-game-loop.mdx` to include all required headings, especially `Expected Visual State`.

For setup:

```mdx
## Expected Visual State

No DesktopGL window opens in setup. The expected state is terminal output from `./tools/check-env.sh` showing macOS, `global.json`, .NET 10, and the `mgdesktopgl` template check.
```

For game loop:

```mdx
## Expected Visual State

The DesktopGL window uses the e01 game loop experiment. In manual mode the title starts as `E01 Game Loop - Fixed 60 Hz`; pressing `F1` switches to variable timestep. In smoke mode, stdout must include `Update: timestep mode changed to Variable.` and `Smoke: exit.`.
```

- [ ] **Step 4: Scaffold missing lessons with migrated content**

Create missing lesson files from `docs/tutorial/*.md`.

Mapping rules:

| Legacy heading | New heading |
| --- | --- |
| `What You Will Run` | `Run` |
| `Expected Output` | split into `Observe` and `Expected Visual State` |
| `Common Problems` | `Common Failures` |

Every file must include frontmatter:

```mdx
---
id: 04-rendering
title: Rendering
---
```

- [ ] **Step 5: Scaffold missing agent task packets**

For every lesson, create an agent task packet with exact headings.

Use this content pattern, replacing lesson id, title, migration source, allowed files, blocked files, spec-required files, and commands:

```md
# Agent Task: 04 Rendering

## Task

Maintain or improve the Rendering lesson and its expected state without changing experiment behavior.

## Context

This lesson maps to `docs/tutorial/04-rendering.md`, `course/lessons/04-rendering.mdx`, and `experiments/e02-2d-rendering`.

## Allowed Files

- `course/lessons/04-rendering.mdx`
- `course/agent-tasks/04-rendering.md`
- `course/evidence/04-rendering/expected-state.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/e02-2d-rendering/**`
- `experiments/e02-2d-rendering.Tests/**`
- unrelated `course/lessons/**`
- unrelated `course/agent-tasks/**`

## Spec Required

Any change under `experiments/e02-2d-rendering/**` or `experiments/e02-2d-rendering.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `./tools/check-course.sh`
- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e02-2d-rendering.Tests/E02Rendering.Tests.csproj`

## Acceptance

- Human lesson keeps every required section from the manifest.
- Expected visual state matches the current experiment behavior.
- Agent packet remains lesson-scoped and does not authorize runtime code changes.
- Manifest paths and commands still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run `./tools/check-course.sh` plus the non-GUI test command.

## Report Format

Report changed files, verification commands with exit codes, score impact, and any unverified visual evidence.
```

- [ ] **Step 6: Verify structural green**

Run:

```bash
./tools/check-course.sh
bash -n tools/check-course.sh
git diff --check
```

Expected:

```text
Course manifest OK.
```

All commands exit 0.

- [ ] **Step 7: Commit verifier and structural course expansion**

```bash
git add course tools/check-course.mjs tools/check-course.sh
git commit -m "feat: enforce canonical dual-track course structure"
```

### Task 4: Harden Human Lessons For Content Quality

**Files:**
- Modify: `course/lessons/*.mdx`
- Modify: `course/evidence/*/expected-state.md`

- [ ] **Step 1: Fill exact expected visual states**

Use these expected states:

| Lesson | Expected Visual State content |
| --- | --- |
| `00-intro` | Integrated demo manual mode opens a small collector arena; smoke mode reaches started, won, restarted, and exit logs. |
| `01-setup` | No GUI window; terminal readiness output is the expected state. |
| `02-first-window` | 960x540 blue DesktopGL window with title `E01 Game Loop - Fixed 60 Hz`; stdout logs Initialize and LoadContent. |
| `03-game-loop` | e01 window starts fixed timestep, smoke toggles to variable timestep, stdout includes `Update: timestep mode changed to Variable.` |
| `04-rendering` | Window shows 1000 sprites plus overlay text; smoke toggles from batched to unbatched mode. |
| `05-input` | Window shows a green square; smoke moves via axis, mouse, and axis again with frame 40, 80, and 120 logs. |
| `06-content-pipeline` | Window shows content-loaded texture/font/sound state; broken MGCB command fails with missing texture path. |
| `07-audio` | Window shows effect/music state blocks; stdout shows music start/stop and sound-effect playback. |
| `08-camera-collision-animation` | Camera/collision smoke logs AABB and circle collision; animation smoke logs idle/walk/jump transitions. |
| `09-publishing` | No learning window is required before published smoke; published executable logs rendered frames and exits without `dotnet` on `PATH`. |
| `10-integrated-demo` | Collector arena starts at Start phase, smoke collects all pickups, reaches Won, restarts, and exits. |

- [ ] **Step 2: Add concrete exercises**

Each lesson exercise must ask for one bounded action:

| Lesson | Exercise |
| --- | --- |
| `00-intro` | Identify one experiment and one integrated-demo file that serve different learning purposes. |
| `01-setup` | Run `./tools/check-env.sh` and explain one ok line and one possible warning line. |
| `02-first-window` | Find the call to `Game.Run()` and list the lifecycle methods called by MonoGame. |
| `03-game-loop` | Find where fixed/variable timestep is toggled and where the mode is displayed. |
| `04-rendering` | Compare the batched and unbatched draw paths in `Game1.cs`. |
| `05-input` | Trace how `InputSnapshot` becomes a player position update. |
| `06-content-pipeline` | Predict the logical asset name for one texture, one font, and one sound. |
| `07-audio` | Decide whether a pickup sound should be `SoundEffect` or `Song`, and explain why. |
| `08-camera-collision-animation` | Identify one file that handles camera math and one file that handles animation state. |
| `09-publishing` | Locate the published executable path and explain why the smoke command strips `PATH`. |
| `10-integrated-demo` | Trace the model transition from Start to Playing to Won. |

- [ ] **Step 3: Add learning-focused walkthroughs**

For each lesson, ensure `Walkthrough` explains this repo's code relationship:

- MonoGame lifecycle belongs in `Game1`.
- Deterministic state belongs in separate classes where tests exist.
- Smoke settings make GUI experiments verifiable.
- Content pipeline lessons use logical asset names.
- Integrated demo is capstone evidence, not production-game scope.

- [ ] **Step 4: Run lesson structure verification**

```bash
./tools/check-course.sh
git diff --check
```

Expected:

```text
Course manifest OK.
```

- [ ] **Step 5: Commit human content hardening**

```bash
git add course/lessons course/evidence
git commit -m "docs: harden human course lessons"
```

### Task 5: Harden Agent Task Packets For Short-Task Execution

**Files:**
- Modify: `course/agent-tasks/*.md`
- Modify: `AGENTS.md`

- [ ] **Step 1: Make allowed files narrow**

Each packet's `Allowed Files` must include only:

- its lesson file,
- its task packet,
- its evidence expected-state file,
- `course/manifest.json` when lesson metadata changes.

Do not include runtime experiment files in `Allowed Files`.

- [ ] **Step 2: Make spec-required runtime patterns explicit**

Each packet must list runtime source patterns in `Spec Required`.

Examples:

```md
## Spec Required

Any change under `experiments/e03-input/**` or `experiments/e03-input.Tests/**` requires an approved spec because it changes tutorial source behavior.
```

For `10-integrated-demo`:

```md
## Spec Required

Any change under `demo/integrated-demo/**` or `demo/integrated-demo.Tests/**` requires an approved spec because the integrated demo is capstone evidence, not a product game expansion target.
```

- [ ] **Step 3: Add short-task examples to AGENTS**

In `AGENTS.md`, add:

```md
## Lesson Short Tasks

For prompts such as `完善 input checkpoint`, `补 game loop expected visual state`, or `收紧 rendering agent packet`, map the request to `course/manifest.json`, open the matching `course/agent-tasks/<lesson-id>.md`, and obey its allowed, blocked, and spec-required sections before editing.
```

- [ ] **Step 4: Verify agent packet structure**

```bash
./tools/check-course.sh
git diff --check
```

Expected:

```text
Course manifest OK.
```

- [ ] **Step 5: Commit agent packet hardening**

```bash
git add AGENTS.md course/agent-tasks
git commit -m "docs: harden lesson agent task packets"
```

### Task 6: Update Tutorial Site To Render Full Manifest Course

**Files:**
- Modify: `tutorial-site/src/data/loadCourse.ts`
- Modify: `tutorial-site/src/pages/index.astro`
- Modify: `tutorial-site/src/pages/[...lesson].astro`
- Modify: `tutorial-site/src/components/*.astro`

- [ ] **Step 1: Extend course types**

Update `CourseLesson` in `tutorial-site/src/data/loadCourse.ts`:

```ts
export interface CourseLesson {
  id: string;
  order: number;
  title: string;
  summary: string;
  kind: 'orientation' | 'setup' | 'experiment' | 'capstone' | 'appendix';
  migrationSource: string;
  route: string;
  human: {
    path: string;
    requiredSections: string[];
  };
  agent: {
    taskPath: string;
    allowedFiles: string[];
    blockedFiles: string[];
    specRequiredFiles: string[];
  };
  code: {
    projects: string[];
    tests: string[];
    keyFiles: string[];
  };
  commands: {
    run: string[];
    verify: string[];
  };
  evidence: {
    status: 'available' | 'pending' | 'notApplicable';
    reason: string;
    expectedPaths: string[];
  };
}
```

- [ ] **Step 2: Use route in static paths**

In `tutorial-site/src/pages/[...lesson].astro`, set:

```ts
return course.lessons.map((lesson) => ({
  params: { lesson: lesson.route },
  props: { lessonId: lesson.id },
}));
```

- [ ] **Step 3: Add agent packet and migration source links**

In the lesson page, render:

```astro
<section>
  <h2>Maintainer Links</h2>
  <ul>
    <li><a href={`/${lesson.agent.taskPath}`}>Agent task packet</a></li>
    <li><code>{lesson.migrationSource}</code></li>
  </ul>
</section>
```

If Astro cannot serve repository-relative Markdown links directly, render them as code paths instead of broken links.

- [ ] **Step 4: Make index explicit about source of truth**

In `tutorial-site/src/pages/index.astro`, include:

```astro
<p>This site renders the lessons listed in <code>course/manifest.json</code>.</p>
```

- [ ] **Step 5: Build site**

Run:

```bash
cd tutorial-site
npm run build
```

Expected:

- Astro check reports 0 errors.
- Astro build emits 12 pages: `/` plus 11 lesson pages.

- [ ] **Step 6: Commit site update**

```bash
git add tutorial-site
git commit -m "feat: render complete manifest course"
```

### Task 7: Align README, Legacy Docs, And Roadmap With V1

**Files:**
- Modify: `README.md`
- Modify: `docs/tutorial/README.md`
- Modify: `docs/tutorial/ROADMAP.md`
- Modify: `docs/agents/README.md`

- [ ] **Step 1: Update README after full migration**

Once `course/` and `tutorial-site/` cover all 00-10 lessons, set `For Humans` to:

```md
## For Humans

Primary course entrypoint:

```bash
cd tutorial-site
npm install
npm run dev
```

The tutorial site renders the complete 00-10 v1 course from `course/manifest.json`.
```

Keep a separate legacy note:

```md
Legacy migration source:

- [`docs/tutorial/README.md`](docs/tutorial/README.md)
```

- [ ] **Step 2: Update docs/tutorial README**

Make the first paragraph:

```md
This directory is the legacy Markdown source used to migrate the v1 course into `course/`. For current v1 lesson work, use `course/manifest.json`, `course/lessons/`, and `course/agent-tasks/`.
```

- [ ] **Step 3: Update roadmap state**

In `docs/tutorial/ROADMAP.md`, add:

```md
Dual-track v1 completion is governed by `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md` and the final audit at `docs/reports/tutorial-v1-quality-audit.md`.
```

- [ ] **Step 4: Verify entrypoint wording**

Run:

```bash
rg -n "Primary course entrypoint|legacy|migration source|complete 00-10|course/manifest.json" README.md docs/tutorial/README.md docs/tutorial/ROADMAP.md docs/agents/README.md
git diff --check
```

Expected:

- README says the complete course comes from `course/manifest.json`.
- `docs/tutorial/` is described as legacy or migration source.
- No file claims `docs/tutorial/` is the primary v1 course source.

- [ ] **Step 5: Commit entrypoint alignment**

```bash
git add README.md docs/tutorial/README.md docs/tutorial/ROADMAP.md docs/agents/README.md
git commit -m "docs: align tutorial entrypoints with v1 course source"
```

### Task 8: Full Verification And Quality Audit

**Files:**
- Create: `docs/reports/tutorial-v1-quality-audit.md`

- [ ] **Step 1: Run structural verification**

Run:

```bash
./tools/check-course.sh
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
bash -n tools/check-course.sh
git diff --check
```

Expected:

- `./tools/check-course.sh` prints `Course manifest OK.`
- Bash syntax checks exit 0.
- `git diff --check` exits 0.

- [ ] **Step 2: Run site verification**

Run:

```bash
cd tutorial-site
npm run build
```

Expected:

- Astro check reports 0 errors.
- Astro build emits `/` and lesson pages for all 11 canonical lessons.

- [ ] **Step 3: Run .NET verification**

Run:

```bash
dotnet build GameDemo.sln -m:1
```

Expected:

```text
0 个警告
0 个错误
```

or English equivalent:

```text
0 Warning(s)
0 Error(s)
```

- [ ] **Step 4: Run full tutorial smoke**

Run:

```bash
./tools/check-tutorial.sh
```

Expected:

```text
Tutorial dry run passed.
```

If GUI access is blocked, record the blocker and do not claim visual verification completion.

- [ ] **Step 5: Write quality audit**

Create `docs/reports/tutorial-v1-quality-audit.md` with this structure:

```md
# Tutorial V1 Quality Audit

Date: 2026-05-03

## Objective

Complete the MonoGame dual-track tutorial v1 with content quality and mainline alignment both at or above 95/100.

## Prompt-To-Artifact Checklist

| Requirement | Evidence | Status |
| --- | --- | --- |
| `course/` is canonical | `course/manifest.json`; README; AGENTS; tutorial site loader | Pass |
| 00-10 human lessons exist | `course/lessons/00-intro.mdx` through `10-integrated-demo.mdx` | Pass |
| 00-10 agent packets exist | `course/agent-tasks/00-intro.md` through `10-integrated-demo.md` | Pass |
| Tutorial site renders all lessons | `npm run build` output lists 11 lesson pages | Pass |
| Verifier enforces canonical coverage | `tools/check-course.mjs`; `./tools/check-course.sh` output | Pass |
| Runtime checks pass | `dotnet build GameDemo.sln -m:1`; `./tools/check-tutorial.sh` | Pass |
| No Godot or game expansion | Diff inspection; manifest mappings | Pass |

## Verification Evidence

| Command | Exit | Reading |
| --- | ---: | --- |
| `./tools/check-course.sh` | 0 | Course manifest OK. |
| `cd tutorial-site && npm run build` | 0 | Astro check/build succeeded. |
| `dotnet build GameDemo.sln -m:1` | 0 | 0 warnings, 0 errors. |
| `./tools/check-tutorial.sh` | 0 | Tutorial dry run passed. |

## Hard Cap Check

No content quality cap applies.

No mainline alignment cap applies.

## Content Quality Score

| Category | Max | Score | Evidence |
| --- | ---: | ---: | --- |
| Canonical lesson completeness | 20 | 20 | 11 lessons, manifest entries, routes. |
| Human learning usefulness | 20 | 19 | Run, observe, walkthrough, exercise, checkpoint per lesson. |
| Visual expectation clarity | 15 | 15 | Expected Visual State per lesson and evidence files. |
| Code mapping quality | 15 | 15 | Key files/projects/tests map to existing repo paths. |
| Agent task usefulness | 15 | 14 | Packets are scoped and include verification; short-task dry run reviewed. |
| Site usability | 10 | 10 | Buildable complete manifest site. |
| Entry point honesty | 5 | 5 | README/AGENTS/legacy docs aligned. |
| Total | 100 | 98 | Evidence-backed. |

## Mainline Alignment Score

| Category | Max | Score | Evidence |
| --- | ---: | ---: | --- |
| `course/` source-of-truth discipline | 25 | 25 | Manifest drives lessons, tasks, site. |
| MonoGame research continuity | 20 | 20 | Lessons map to existing experiments/demo/tools. |
| Scope control | 15 | 15 | No Godot, no game expansion, no new experiments. |
| Human/agent separation | 15 | 14 | Human lessons and agent packets remain separate. |
| Verifier coverage discipline | 15 | 15 | Verifier coverage and limits documented. |
| Documentation consistency | 10 | 10 | README, AGENTS, roadmap, audit aligned. |
| Total | 100 | 99 | Evidence-backed. |

## Counterargument Review

- Passing `check-course` does not prove lesson prose quality; prose was reviewed against every required section and expected state.
- Passing `npm run build` does not prove runtime behavior; `dotnet build` and `check-tutorial` provide runtime evidence.
- Textual expected visual state is weaker than screenshots; screenshots are not required for v1, and the cap does not apply because expected visual state is present for every GUI lesson.

## Result

Content quality: 98/100.

Mainline alignment: 99/100.

V1 quality gate passes.
```

Adjust scores downward if evidence does not support the example. If either score is below 95, keep working and list required corrections instead of claiming the gate passes.

- [ ] **Step 6: Commit audit**

```bash
git add docs/reports/tutorial-v1-quality-audit.md
git commit -m "docs: add tutorial v1 quality audit"
```

### Task 9: Final Status And Integration

**Files:**
- No planned file changes.

- [ ] **Step 1: Confirm clean worktree**

Run:

```bash
git status --short --branch --untracked-files=all
```

Expected:

- Clean working tree.
- Branch ahead by the planned implementation commits if not pushed.

- [ ] **Step 2: Confirm recent commits**

Run:

```bash
git log --oneline --decorate -10
```

Expected:

- Commits for entrypoint correction, verifier/course expansion, human lesson hardening, agent packet hardening, site rendering, entrypoint alignment, and quality audit.

- [ ] **Step 3: Push after owner approval**

Run only after owner approval for external side effects:

```bash
git push origin main
```

Expected:

```text
main -> main
```

## Self-Review Checklist

Spec coverage:

- V1 canonical 00-10 scope is implemented by Tasks 2 and 3.
- Human lesson contract is implemented by Tasks 3 and 4.
- Agent task packet contract is implemented by Tasks 3 and 5.
- Manifest contract is implemented by Tasks 2 and 3.
- Tutorial site contract is implemented by Task 6.
- Verifier contract is implemented by Task 2 and proven by Task 8.
- Runtime verification contract is implemented by Task 8.
- Hard score caps and two-score audit are implemented by Task 8.
- Entrypoint honesty is implemented by Tasks 1 and 7.

Placeholder scan:

- This plan must pass the writing-plans forbidden-marker scan with no matches.

Mainline guard:

- No task adds Godot.
- No task expands `demo/integrated-demo`.
- No task adds a new MonoGame experiment.
- No task makes `docs/tutorial/` the primary v1 source.
- No task allows site navigation outside `course/manifest.json`.

Execution note:

- The final audit scores in Task 8 are examples of the expected audit shape, not guaranteed results. The implementation agent must score from actual evidence and keep working if either score is below 95.
