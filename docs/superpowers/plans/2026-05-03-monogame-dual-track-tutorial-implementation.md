# MonoGame Dual-Track Tutorial Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build the first working slice of the approved dual-track MonoGame tutorial architecture.

**Architecture:** The course manifest is the single source of truth. Human lessons, agent task packets, tutorial-site navigation, code references, and verification commands all hang from `course/manifest.json`. The first slice migrates setup and game-loop lessons, adds a structural verifier, and adds an Astro tutorial shell that renders only manifest-backed lessons.

**Tech Stack:** MonoGame/.NET 10, Node.js ESM, Astro, MDX, shell scripts, JSON Schema-style manifest contract.

---

## Approved Spec

Implement from:

```text
docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-architecture-design.md
```

Do not use `../game_design` as a structural reference. Do not expand `demo/integrated-demo`. Do not add Godot. Do not create a marketing landing page.

## File Responsibility Map

Create:

- `course/manifest.json`: single source of truth for the first two lessons.
- `course/schema.json`: JSON Schema-style contract for the manifest.
- `course/lessons/01-setup.mdx`: human-facing setup lesson.
- `course/lessons/03-game-loop.mdx`: human-facing game-loop lesson.
- `course/agent-tasks/01-setup.md`: agent packet for setup lesson maintenance.
- `course/agent-tasks/03-game-loop.md`: agent packet for game-loop lesson maintenance.
- `tools/check-course.sh`: shell entrypoint for structural course validation.
- `tools/check-course.mjs`: dependency-free Node verifier.
- `tutorial-site/package.json`: isolated tutorial site package.
- `tutorial-site/astro.config.mjs`: Astro + MDX config.
- `tutorial-site/tsconfig.json`: strict TypeScript config for the site.
- `tutorial-site/src/content.config.ts`: MDX collection loading `course/lessons`.
- `tutorial-site/src/data/loadCourse.ts`: manifest loader and typed course model.
- `tutorial-site/src/layouts/TutorialLayout.astro`: shared lesson chrome.
- `tutorial-site/src/pages/index.astro`: manifest-driven course index.
- `tutorial-site/src/pages/[...lesson].astro`: manifest-driven lesson route.
- `tutorial-site/src/components/CourseNav.astro`: ordered lesson navigation.
- `tutorial-site/src/components/LessonHeader.astro`: lesson metadata block.
- `tutorial-site/src/components/CommandBlock.astro`: command rendering block.
- `tutorial-site/src/components/KeyFileList.astro`: key file list block.
- `tutorial-site/src/components/Checkpoint.astro`: checkpoint callout.
- `tutorial-site/src/components/EvidencePanel.astro`: evidence status block.

Modify:

- `README.md`: route humans to the tutorial site once the first slice exists.
- `AGENTS.md`: route lesson-scoped agent work through `course/manifest.json` and `course/agent-tasks/`.
- `docs/agents/README.md`: mention per-lesson task packets live under `course/agent-tasks/`.

## Task 1: Course Kernel Slice

**Files:**

- Create: `course/manifest.json`
- Create: `course/schema.json`
- Create: `course/lessons/01-setup.mdx`
- Create: `course/lessons/03-game-loop.mdx`
- Create: `course/agent-tasks/01-setup.md`
- Create: `course/agent-tasks/03-game-loop.md`

- [ ] **Step 1: Create course directories**

Run:

```bash
mkdir -p course/lessons course/agent-tasks course/evidence/03-game-loop
```

Expected: command exits with status 0.

- [ ] **Step 2: Create `course/manifest.json`**

Create `course/manifest.json` with:

```json
{
  "version": 1,
  "title": "MonoGame Study Framework",
  "tracks": ["human", "agent"],
  "lessons": [
    {
      "id": "01-setup",
      "order": 1,
      "title": "Setup",
      "summary": "Prepare the macOS DesktopGL and .NET toolchain used by the tutorial.",
      "kind": "setup",
      "human": {
        "path": "course/lessons/01-setup.mdx",
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
        "taskPath": "course/agent-tasks/01-setup.md",
        "allowedFiles": [
          "course/lessons/01-setup.mdx",
          "course/agent-tasks/01-setup.md",
          "course/manifest.json"
        ],
        "blockedFiles": [
          "demo/integrated-demo/**",
          "experiments/**"
        ],
        "specRequiredFiles": [
          "tools/check-env.sh",
          "global.json"
        ]
      },
      "code": {
        "projects": [],
        "tests": [],
        "keyFiles": [
          "global.json",
          "tools/check-env.sh",
          "GameDemo.sln"
        ]
      },
      "commands": {
        "run": [
          "./tools/check-env.sh"
        ],
        "verify": [
          "./tools/check-env.sh"
        ]
      },
      "evidence": {
        "status": "notApplicable",
        "reason": "Setup lesson evidence is command output rather than a screenshot.",
        "expectedPaths": []
      }
    },
    {
      "id": "03-game-loop",
      "order": 3,
      "title": "Game Loop",
      "summary": "Understand MonoGame Update and Draw cadence through the e01 experiment.",
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
          "course/agent-tasks/03-game-loop.md",
          "course/manifest.json"
        ],
        "blockedFiles": [
          "demo/integrated-demo/**",
          "course/lessons/01-setup.mdx",
          "course/agent-tasks/01-setup.md"
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
  ]
}
```

- [ ] **Step 3: Create `course/schema.json`**

Create `course/schema.json` with:

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://example.local/monogame-study-framework/course.schema.json",
  "title": "MonoGame Study Framework Course Manifest",
  "type": "object",
  "required": ["version", "title", "tracks", "lessons"],
  "additionalProperties": false,
  "properties": {
    "version": { "const": 1 },
    "title": { "type": "string", "minLength": 1 },
    "tracks": {
      "type": "array",
      "prefixItems": [{ "const": "human" }, { "const": "agent" }],
      "items": false,
      "minItems": 2,
      "maxItems": 2
    },
    "lessons": {
      "type": "array",
      "minItems": 1,
      "items": { "$ref": "#/$defs/lesson" }
    }
  },
  "$defs": {
    "stringArray": {
      "type": "array",
      "items": { "type": "string", "minLength": 1 }
    },
    "lesson": {
      "type": "object",
      "required": ["id", "order", "title", "summary", "kind", "human", "agent", "code", "commands", "evidence"],
      "additionalProperties": false,
      "properties": {
        "id": { "type": "string", "pattern": "^[0-9]{2}-[a-z0-9]+(?:-[a-z0-9]+)*$" },
        "order": { "type": "integer", "minimum": 0 },
        "title": { "type": "string", "minLength": 1 },
        "summary": { "type": "string", "minLength": 1 },
        "kind": { "enum": ["orientation", "setup", "experiment", "capstone", "appendix"] },
        "human": {
          "type": "object",
          "required": ["path", "requiredSections"],
          "additionalProperties": false,
          "properties": {
            "path": { "type": "string", "minLength": 1 },
            "requiredSections": { "$ref": "#/$defs/stringArray" }
          }
        },
        "agent": {
          "type": "object",
          "required": ["taskPath", "allowedFiles", "blockedFiles", "specRequiredFiles"],
          "additionalProperties": false,
          "properties": {
            "taskPath": { "type": "string", "minLength": 1 },
            "allowedFiles": { "$ref": "#/$defs/stringArray" },
            "blockedFiles": { "$ref": "#/$defs/stringArray" },
            "specRequiredFiles": { "$ref": "#/$defs/stringArray" }
          }
        },
        "code": {
          "type": "object",
          "required": ["projects", "tests", "keyFiles"],
          "additionalProperties": false,
          "properties": {
            "projects": { "$ref": "#/$defs/stringArray" },
            "tests": { "$ref": "#/$defs/stringArray" },
            "keyFiles": { "$ref": "#/$defs/stringArray" }
          }
        },
        "commands": {
          "type": "object",
          "required": ["run", "verify"],
          "additionalProperties": false,
          "properties": {
            "run": { "$ref": "#/$defs/stringArray" },
            "verify": { "$ref": "#/$defs/stringArray" }
          }
        },
        "evidence": {
          "type": "object",
          "required": ["status", "reason", "expectedPaths"],
          "additionalProperties": false,
          "properties": {
            "status": { "enum": ["available", "pending", "notApplicable"] },
            "reason": { "type": "string" },
            "expectedPaths": { "$ref": "#/$defs/stringArray" }
          }
        }
      }
    }
  }
}
```

- [ ] **Step 4: Create `course/lessons/01-setup.mdx`**

Create `course/lessons/01-setup.mdx` with:

```mdx
---
id: 01-setup
title: Setup
---

## Goal

Prepare the local macOS DesktopGL toolchain used by this MonoGame tutorial.

## Run

```bash
./tools/check-env.sh
```

## Observe

The script should report the active .NET SDK, MonoGame template availability, and MGCB tooling readiness. Treat any missing tool as a setup blocker before running experiments.

## Key Files

- `global.json`
- `tools/check-env.sh`
- `GameDemo.sln`

## Walkthrough

This repository pins its SDK through `global.json` and keeps the tutorial focused on macOS DesktopGL. `tools/check-env.sh` is the first readiness check because it catches missing SDKs, missing templates, and missing content tooling before a learner opens a game window.

Run this check from the repository root. Do not run it from inside an experiment directory because the script is part of the repository-level bootstrap.

## Common Failures

- `dotnet` is missing from `PATH`: install the pinned SDK and open a fresh shell.
- MonoGame templates are missing: install the MonoGame templates before creating or repairing DesktopGL projects.
- MGCB tooling is missing or stale: restore local tools in the experiment directory that owns the content pipeline.

## Exercise

Open `global.json` and identify the SDK version this repository expects. Then open `tools/check-env.sh` and identify which checks protect a fresh machine from starting the tutorial with a broken toolchain.

## Checkpoint

You can explain why setup is verified before any MonoGame window is launched.

## Next

Continue to the game-loop lesson after the environment check passes.
```

- [ ] **Step 5: Create `course/lessons/03-game-loop.mdx`**

Create `course/lessons/03-game-loop.mdx` with:

```mdx
---
id: 03-game-loop
title: Game Loop
---

## Goal

Learn how MonoGame separates simulation work in `Update` from rendering work in `Draw`.

## Run

```bash
dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj
```

## Observe

The experiment opens a DesktopGL window and displays loop or timing state. The learner should see the on-screen state continue changing while the app runs.

## Key Files

- `experiments/e01-game-loop/Game1.cs`
- `experiments/e01-game-loop/GameLoopState.cs`
- `experiments/e01-game-loop.Tests/Program.cs`

## Walkthrough

`Game1.cs` owns the MonoGame lifecycle. `Update` advances state and `Draw` renders the current state. `GameLoopState.cs` keeps the loop state testable outside a graphics window, which is why the companion test project can validate behavior without opening DesktopGL.

The important learning point is the boundary: game simulation belongs in state that can be tested, while MonoGame-specific drawing stays near the game class.

## Common Failures

- DesktopGL window does not open: re-run setup and confirm the SDK and MonoGame dependencies are available.
- The learner expects the test project to render a window: the test project validates state and intentionally avoids graphics.
- Build fails after unrelated edits: run `dotnet build GameDemo.sln -m:1` and fix compile errors before changing lesson text.

## Exercise

Find where elapsed loop state is updated, then find where drawing reads that state. Explain which part can be tested without a window and why.

## Checkpoint

You can explain why the test project validates loop state without launching MonoGame.

## Next

Continue to rendering.
```

- [ ] **Step 6: Create `course/agent-tasks/01-setup.md`**

Create `course/agent-tasks/01-setup.md` with:

```md
# Agent Task: 01 Setup

## Task

Maintain or improve the setup lesson without changing repository toolchain behavior.

## Context

This lesson maps to repository bootstrap files: `global.json`, `tools/check-env.sh`, and `GameDemo.sln`.

## Allowed Files

- `course/lessons/01-setup.mdx`
- `course/agent-tasks/01-setup.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `experiments/**`

## Spec Required

Any change to `tools/check-env.sh` or `global.json` requires an approved spec because it changes bootstrap behavior.

## Commands

- `./tools/check-env.sh`
- `./tools/check-course.sh`

## Acceptance

- Human lesson still contains every section required by the manifest.
- Agent task packet still contains every required operating section.
- Manifest paths still resolve.
- Verification commands are reported with results.

## Failure Handling

If `./tools/check-env.sh` fails because the local machine is missing a tool, report the missing tool and do not edit unrelated files.

## Report Format

Report changed files, verification commands, and any setup checks that could not be completed locally.
```

- [ ] **Step 7: Create `course/agent-tasks/03-game-loop.md`**

Create `course/agent-tasks/03-game-loop.md` with:

```md
# Agent Task: 03 Game Loop

## Task

Maintain or improve the Game Loop lesson without changing experiment behavior.

## Context

This lesson maps to `experiments/e01-game-loop` and `experiments/e01-game-loop.Tests`.

## Allowed Files

- `course/lessons/03-game-loop.mdx`
- `course/agent-tasks/03-game-loop.md`
- `course/manifest.json` only when updating metadata for this lesson

## Blocked Files

- `demo/integrated-demo/**`
- `course/lessons/01-setup.mdx`
- `course/agent-tasks/01-setup.md`

## Spec Required

Any change under `experiments/e01-game-loop/**` or `experiments/e01-game-loop.Tests/**` requires an approved spec because it changes tutorial source behavior.

## Commands

- `dotnet build GameDemo.sln -m:1`
- `dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj`
- `./tools/check-course.sh`

## Acceptance

- Human lesson still contains every section required by the manifest.
- Agent task packet still contains every required operating section.
- Manifest paths still resolve.
- Verification commands are reported with results.

## Failure Handling

If a MonoGame GUI command cannot run in the current environment, do not claim visual verification. Report the skipped command and run non-GUI checks.

## Report Format

Report changed files, verification commands, and any unverified visual evidence.
```

- [ ] **Step 8: Validate JSON syntax**

Run:

```bash
node -e "JSON.parse(require('fs').readFileSync('course/manifest.json','utf8')); JSON.parse(require('fs').readFileSync('course/schema.json','utf8')); console.log('course json ok')"
```

Expected:

```text
course json ok
```

- [ ] **Step 9: Commit course kernel**

Run:

```bash
git add course
git commit -m "feat: add dual-track course kernel"
```

Expected: commit succeeds.

## Task 2: Structural Course Verifier

**Files:**

- Create: `tools/check-course.sh`
- Create: `tools/check-course.mjs`

- [ ] **Step 1: Create failing verifier shell first**

Create `tools/check-course.sh` with:

```sh
#!/usr/bin/env bash
set -euo pipefail

node tools/check-course.mjs
```

Run:

```bash
chmod +x tools/check-course.sh
./tools/check-course.sh
```

Expected: fails because `tools/check-course.mjs` does not exist.

- [ ] **Step 2: Create `tools/check-course.mjs`**

Create `tools/check-course.mjs` with:

```js
#!/usr/bin/env node
import { existsSync, readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const root = process.cwd();
const manifestPath = resolve(root, 'course/manifest.json');

const requiredAgentSections = [
  'Task',
  'Context',
  'Allowed Files',
  'Blocked Files',
  'Spec Required',
  'Commands',
  'Acceptance',
  'Failure Handling',
  'Report Format',
];

let errors = 0;

function fail(message) {
  console.error(`ERROR: ${message}`);
  errors += 1;
}

function readJson(path) {
  try {
    return JSON.parse(readFileSync(path, 'utf8'));
  } catch (error) {
    fail(`${path} is not valid JSON: ${error.message}`);
    return null;
  }
}

function assertNonEmptyString(value, label) {
  if (typeof value !== 'string' || value.trim() === '') {
    fail(`${label} must be a non-empty string`);
  }
}

function assertArray(value, label) {
  if (!Array.isArray(value)) {
    fail(`${label} must be an array`);
    return [];
  }
  return value;
}

function assertNonEmptyArray(value, label) {
  const array = assertArray(value, label);
  if (array.length === 0) {
    fail(`${label} must not be empty`);
  }
  return array;
}

function assertPathExists(path, label) {
  if (!existsSync(resolve(root, path))) {
    fail(`${label} does not exist: ${path}`);
  }
}

function hasHeading(content, heading) {
  const escaped = heading.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
  const pattern = new RegExp(`^#{1,6}\\s+${escaped}\\s*$`, 'm');
  return pattern.test(content);
}

function validateTopLevel(manifest) {
  if (manifest.version !== 1) {
    fail('version must be 1');
  }

  assertNonEmptyString(manifest.title, 'title');

  const tracks = assertArray(manifest.tracks, 'tracks');
  if (tracks.length !== 2 || tracks[0] !== 'human' || tracks[1] !== 'agent') {
    fail('tracks must be exactly ["human", "agent"]');
  }

  assertNonEmptyArray(manifest.lessons, 'lessons');
}

function validateLesson(lesson, seenIds, seenOrders) {
  assertNonEmptyString(lesson.id, 'lesson.id');
  if (!/^[0-9]{2}-[a-z0-9]+(?:-[a-z0-9]+)*$/.test(lesson.id)) {
    fail(`lesson.id is not a valid slug: ${lesson.id}`);
  }

  if (seenIds.has(lesson.id)) {
    fail(`duplicate lesson id: ${lesson.id}`);
  }
  seenIds.add(lesson.id);

  if (!Number.isInteger(lesson.order)) {
    fail(`${lesson.id}.order must be an integer`);
  } else if (seenOrders.has(lesson.order)) {
    fail(`duplicate lesson order: ${lesson.order}`);
  }
  seenOrders.add(lesson.order);

  assertNonEmptyString(lesson.title, `${lesson.id}.title`);
  assertNonEmptyString(lesson.summary, `${lesson.id}.summary`);

  const validKinds = new Set(['orientation', 'setup', 'experiment', 'capstone', 'appendix']);
  if (!validKinds.has(lesson.kind)) {
    fail(`${lesson.id}.kind is invalid: ${lesson.kind}`);
  }

  validateHumanTrack(lesson);
  validateAgentTrack(lesson);
  validateCode(lesson);
  validateCommands(lesson);
  validateEvidence(lesson);
}

function validateHumanTrack(lesson) {
  if (!lesson.human) {
    fail(`${lesson.id}.human is required`);
    return;
  }

  assertNonEmptyString(lesson.human.path, `${lesson.id}.human.path`);
  assertPathExists(lesson.human.path, `${lesson.id}.human.path`);

  const sections = assertNonEmptyArray(lesson.human.requiredSections, `${lesson.id}.human.requiredSections`);
  const contentPath = resolve(root, lesson.human.path);
  const content = existsSync(contentPath) ? readFileSync(contentPath, 'utf8') : '';

  for (const section of sections) {
    assertNonEmptyString(section, `${lesson.id}.human.requiredSections[]`);
    if (!hasHeading(content, section)) {
      fail(`${lesson.human.path} is missing heading: ${section}`);
    }
  }
}

function validateAgentTrack(lesson) {
  if (!lesson.agent) {
    fail(`${lesson.id}.agent is required`);
    return;
  }

  assertNonEmptyString(lesson.agent.taskPath, `${lesson.id}.agent.taskPath`);
  assertPathExists(lesson.agent.taskPath, `${lesson.id}.agent.taskPath`);

  assertNonEmptyArray(lesson.agent.allowedFiles, `${lesson.id}.agent.allowedFiles`);
  assertArray(lesson.agent.blockedFiles, `${lesson.id}.agent.blockedFiles`);
  assertArray(lesson.agent.specRequiredFiles, `${lesson.id}.agent.specRequiredFiles`);

  for (const file of lesson.agent.allowedFiles) {
    if (!file.includes('*')) {
      assertPathExists(file, `${lesson.id}.agent.allowedFiles[]`);
    }
  }

  const taskPath = resolve(root, lesson.agent.taskPath);
  const content = existsSync(taskPath) ? readFileSync(taskPath, 'utf8') : '';
  for (const section of requiredAgentSections) {
    if (!hasHeading(content, section)) {
      fail(`${lesson.agent.taskPath} is missing heading: ${section}`);
    }
  }
}

function validateCode(lesson) {
  if (!lesson.code) {
    fail(`${lesson.id}.code is required`);
    return;
  }

  const projects = assertArray(lesson.code.projects, `${lesson.id}.code.projects`);
  const tests = assertArray(lesson.code.tests, `${lesson.id}.code.tests`);
  const keyFiles = assertNonEmptyArray(lesson.code.keyFiles, `${lesson.id}.code.keyFiles`);

  if ((lesson.kind === 'experiment' || lesson.kind === 'capstone') && projects.length === 0) {
    fail(`${lesson.id}.code.projects must not be empty for ${lesson.kind} lessons`);
  }

  for (const project of projects) assertPathExists(project, `${lesson.id}.code.projects[]`);
  for (const test of tests) assertPathExists(test, `${lesson.id}.code.tests[]`);
  for (const file of keyFiles) assertPathExists(file, `${lesson.id}.code.keyFiles[]`);
}

function validateCommands(lesson) {
  if (!lesson.commands) {
    fail(`${lesson.id}.commands is required`);
    return;
  }

  const run = assertArray(lesson.commands.run, `${lesson.id}.commands.run`);
  const verify = assertNonEmptyArray(lesson.commands.verify, `${lesson.id}.commands.verify`);

  if (!['orientation', 'appendix'].includes(lesson.kind) && run.length === 0) {
    fail(`${lesson.id}.commands.run must not be empty for ${lesson.kind} lessons`);
  }

  for (const command of run) assertNonEmptyString(command, `${lesson.id}.commands.run[]`);
  for (const command of verify) assertNonEmptyString(command, `${lesson.id}.commands.verify[]`);
}

function validateEvidence(lesson) {
  if (!lesson.evidence) {
    fail(`${lesson.id}.evidence is required`);
    return;
  }

  const validStatuses = new Set(['available', 'pending', 'notApplicable']);
  if (!validStatuses.has(lesson.evidence.status)) {
    fail(`${lesson.id}.evidence.status is invalid: ${lesson.evidence.status}`);
  }

  if (lesson.evidence.status !== 'available') {
    assertNonEmptyString(lesson.evidence.reason, `${lesson.id}.evidence.reason`);
  }

  const paths = assertArray(lesson.evidence.expectedPaths, `${lesson.id}.evidence.expectedPaths`);
  if (['available', 'pending'].includes(lesson.evidence.status) && paths.length === 0) {
    fail(`${lesson.id}.evidence.expectedPaths must not be empty when evidence is ${lesson.evidence.status}`);
  }

  if (lesson.evidence.status === 'available') {
    for (const path of paths) assertPathExists(path, `${lesson.id}.evidence.expectedPaths[]`);
  }
}

const manifest = readJson(manifestPath);
if (manifest) {
  validateTopLevel(manifest);
  const seenIds = new Set();
  const seenOrders = new Set();
  for (const lesson of manifest.lessons || []) {
    validateLesson(lesson, seenIds, seenOrders);
  }
}

if (errors > 0) {
  console.error(`\n${errors} course validation error(s).`);
  process.exit(1);
}

console.log('Course manifest OK.');
```

- [ ] **Step 3: Run verifier**

Run:

```bash
./tools/check-course.sh
```

Expected:

```text
Course manifest OK.
```

- [ ] **Step 4: Run shell syntax check**

Run:

```bash
bash -n tools/check-course.sh
```

Expected: command exits with status 0.

- [ ] **Step 5: Commit verifier**

Run:

```bash
git add tools/check-course.sh tools/check-course.mjs
git commit -m "feat: add course structure verifier"
```

Expected: commit succeeds.

## Task 3: Astro Tutorial Site Shell

**Files:**

- Create: `tutorial-site/package.json`
- Create: `tutorial-site/astro.config.mjs`
- Create: `tutorial-site/tsconfig.json`
- Create: `tutorial-site/src/content.config.ts`
- Create: `tutorial-site/src/data/loadCourse.ts`
- Create: `tutorial-site/src/layouts/TutorialLayout.astro`
- Create: `tutorial-site/src/pages/index.astro`
- Create: `tutorial-site/src/pages/[...lesson].astro`
- Create: `tutorial-site/src/components/CourseNav.astro`
- Create: `tutorial-site/src/components/LessonHeader.astro`
- Create: `tutorial-site/src/components/CommandBlock.astro`
- Create: `tutorial-site/src/components/KeyFileList.astro`
- Create: `tutorial-site/src/components/Checkpoint.astro`
- Create: `tutorial-site/src/components/EvidencePanel.astro`

- [ ] **Step 1: Create tutorial site directories**

Run:

```bash
mkdir -p tutorial-site/src/components tutorial-site/src/data tutorial-site/src/layouts tutorial-site/src/pages
```

Expected: command exits with status 0.

- [ ] **Step 2: Create `tutorial-site/package.json`**

Create `tutorial-site/package.json` with:

```json
{
  "name": "monogame-study-tutorial-site",
  "private": true,
  "version": "0.1.0",
  "type": "module",
  "scripts": {
    "dev": "astro dev",
    "build": "astro check && astro build",
    "preview": "astro preview"
  },
  "dependencies": {
    "@astrojs/mdx": "^4.0.0",
    "astro": "^5.18.1"
  },
  "devDependencies": {
    "@astrojs/check": "^0.9.4",
    "typescript": "^5.9.3"
  }
}
```

- [ ] **Step 3: Create `tutorial-site/astro.config.mjs`**

Create `tutorial-site/astro.config.mjs` with:

```js
import { defineConfig } from 'astro/config';
import mdx from '@astrojs/mdx';

export default defineConfig({
  integrations: [mdx()],
});
```

- [ ] **Step 4: Create `tutorial-site/tsconfig.json`**

Create `tutorial-site/tsconfig.json` with:

```json
{
  "extends": "astro/tsconfigs/strict",
  "include": [".astro/types.d.ts", "**/*"],
  "exclude": ["dist"]
}
```

- [ ] **Step 5: Create `tutorial-site/src/content.config.ts`**

Create `tutorial-site/src/content.config.ts` with:

```ts
import { defineCollection, z } from 'astro:content';
import { glob } from 'astro/loaders';

const lessons = defineCollection({
  loader: glob({ pattern: '*.mdx', base: '../course/lessons' }),
  schema: z.object({
    id: z.string(),
    title: z.string(),
  }),
});

export const collections = { lessons };
```

- [ ] **Step 6: Create `tutorial-site/src/data/loadCourse.ts`**

Create `tutorial-site/src/data/loadCourse.ts` with:

```ts
import { readFileSync } from 'node:fs';

export interface CourseManifest {
  version: number;
  title: string;
  tracks: ['human', 'agent'];
  lessons: CourseLesson[];
}

export interface CourseLesson {
  id: string;
  order: number;
  title: string;
  summary: string;
  kind: 'orientation' | 'setup' | 'experiment' | 'capstone' | 'appendix';
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

const manifestUrl = new URL('../../../course/manifest.json', import.meta.url);

export function loadCourse(): CourseManifest {
  const raw = readFileSync(manifestUrl, 'utf8');
  const manifest = JSON.parse(raw) as CourseManifest;
  return {
    ...manifest,
    lessons: [...manifest.lessons].sort((a, b) => a.order - b.order),
  };
}

export function findLesson(id: string): CourseLesson | undefined {
  return loadCourse().lessons.find((lesson) => lesson.id === id);
}
```

- [ ] **Step 7: Create components**

Create `tutorial-site/src/components/CourseNav.astro` with:

```astro
---
import type { CourseLesson } from '../data/loadCourse';

interface Props {
  lessons: CourseLesson[];
  currentId?: string;
}

const { lessons, currentId } = Astro.props;
---

<nav aria-label="Course lessons" class="course-nav">
  <ol>
    {lessons.map((lesson) => (
      <li>
        <a href={`/${lesson.id}`} aria-current={lesson.id === currentId ? 'page' : undefined}>
          <span>{String(lesson.order).padStart(2, '0')}</span>
          {lesson.title}
        </a>
      </li>
    ))}
  </ol>
</nav>
```

Create `tutorial-site/src/components/LessonHeader.astro` with:

```astro
---
import type { CourseLesson } from '../data/loadCourse';

interface Props {
  lesson: CourseLesson;
}

const { lesson } = Astro.props;
---

<header class="lesson-header">
  <p class="eyebrow">{lesson.kind}</p>
  <h1>{lesson.title}</h1>
  <p>{lesson.summary}</p>
</header>
```

Create `tutorial-site/src/components/CommandBlock.astro` with:

```astro
---
interface Props {
  title: string;
  commands: string[];
}

const { title, commands } = Astro.props;
---

<section class="command-block">
  <h2>{title}</h2>
  {commands.map((command) => (
    <pre><code>{command}</code></pre>
  ))}
</section>
```

Create `tutorial-site/src/components/KeyFileList.astro` with:

```astro
---
interface Props {
  files: string[];
}

const { files } = Astro.props;
---

<section class="key-files">
  <h2>Key Files</h2>
  <ul>
    {files.map((file) => (
      <li><code>{file}</code></li>
    ))}
  </ul>
</section>
```

Create `tutorial-site/src/components/Checkpoint.astro` with:

```astro
---
interface Props {
  sections: string[];
}

const { sections } = Astro.props;
---

<section class="checkpoint">
  <h2>Lesson Contract</h2>
  <p>This lesson must keep these sections:</p>
  <ul>
    {sections.map((section) => (
      <li>{section}</li>
    ))}
  </ul>
</section>
```

Create `tutorial-site/src/components/EvidencePanel.astro` with:

```astro
---
import type { CourseLesson } from '../data/loadCourse';

interface Props {
  lesson: CourseLesson;
}

const { lesson } = Astro.props;
---

<section class="evidence-panel">
  <h2>Evidence</h2>
  <p>Status: <strong>{lesson.evidence.status}</strong></p>
  {lesson.evidence.reason && <p>{lesson.evidence.reason}</p>}
  {lesson.evidence.expectedPaths.length > 0 && (
    <ul>
      {lesson.evidence.expectedPaths.map((path) => (
        <li><code>{path}</code></li>
      ))}
    </ul>
  )}
</section>
```

- [ ] **Step 8: Create layout**

Create `tutorial-site/src/layouts/TutorialLayout.astro` with:

```astro
---
import CourseNav from '../components/CourseNav.astro';
import type { CourseLesson } from '../data/loadCourse';

interface Props {
  title: string;
  lessons: CourseLesson[];
  currentId?: string;
}

const { title, lessons, currentId } = Astro.props;
---

<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>{title}</title>
    <style>
      :root {
        color-scheme: light;
        font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif;
        background: #f7f8fb;
        color: #1f2933;
      }

      body {
        margin: 0;
      }

      a {
        color: #1459b8;
      }

      .shell {
        display: grid;
        grid-template-columns: minmax(220px, 280px) minmax(0, 1fr);
        min-height: 100vh;
      }

      aside {
        border-right: 1px solid #d8dee8;
        background: #ffffff;
        padding: 24px;
      }

      main {
        max-width: 920px;
        padding: 32px;
      }

      pre {
        overflow-x: auto;
        border: 1px solid #d8dee8;
        background: #101828;
        color: #f7f8fb;
        padding: 12px;
      }

      code {
        font-family: "SFMono-Regular", Consolas, monospace;
      }

      .course-nav ol {
        list-style: none;
        padding: 0;
      }

      .course-nav li + li {
        margin-top: 10px;
      }

      .course-nav a[aria-current="page"] {
        font-weight: 700;
      }

      .eyebrow {
        color: #5f6b7a;
        text-transform: uppercase;
        font-size: 0.8rem;
      }

      @media (max-width: 760px) {
        .shell {
          display: block;
        }

        aside {
          border-right: 0;
          border-bottom: 1px solid #d8dee8;
        }
      }
    </style>
  </head>
  <body>
    <div class="shell">
      <aside>
        <h2>MonoGame Study</h2>
        <CourseNav lessons={lessons} currentId={currentId} />
      </aside>
      <main>
        <slot />
      </main>
    </div>
  </body>
</html>
```

- [ ] **Step 9: Create index page**

Create `tutorial-site/src/pages/index.astro` with:

```astro
---
import TutorialLayout from '../layouts/TutorialLayout.astro';
import { loadCourse } from '../data/loadCourse';

const course = loadCourse();
---

<TutorialLayout title={course.title} lessons={course.lessons}>
  <h1>{course.title}</h1>
  <p>Manifest-driven MonoGame tutorial for humans and agents.</p>

  <section>
    <h2>Start</h2>
    <pre><code>./tools/check-env.sh</code></pre>
  </section>

  <section>
    <h2>Lessons</h2>
    <ol>
      {course.lessons.map((lesson) => (
        <li>
          <a href={`/${lesson.id}`}>{String(lesson.order).padStart(2, '0')} - {lesson.title}</a>
          <p>{lesson.summary}</p>
        </li>
      ))}
    </ol>
  </section>
</TutorialLayout>
```

- [ ] **Step 10: Create lesson page route**

Create `tutorial-site/src/pages/[...lesson].astro` with:

```astro
---
import { getCollection, render } from 'astro:content';
import TutorialLayout from '../layouts/TutorialLayout.astro';
import LessonHeader from '../components/LessonHeader.astro';
import CommandBlock from '../components/CommandBlock.astro';
import KeyFileList from '../components/KeyFileList.astro';
import Checkpoint from '../components/Checkpoint.astro';
import EvidencePanel from '../components/EvidencePanel.astro';
import { loadCourse, findLesson } from '../data/loadCourse';

export async function getStaticPaths() {
  const course = loadCourse();
  return course.lessons.map((lesson) => ({
    params: { lesson: lesson.id },
    props: { lessonId: lesson.id },
  }));
}

const { lessonId } = Astro.props;
const course = loadCourse();
const lesson = findLesson(lessonId);

if (!lesson) {
  throw new Error(`Lesson not found in manifest: ${lessonId}`);
}

const entries = await getCollection('lessons');
const entry = entries.find((item) => item.data.id === lesson.id);

if (!entry) {
  throw new Error(`Lesson content not found: ${lesson.id}`);
}

const { Content } = await render(entry);
---

<TutorialLayout title={`${lesson.title} - ${course.title}`} lessons={course.lessons} currentId={lesson.id}>
  <LessonHeader lesson={lesson} />
  <CommandBlock title="Run" commands={lesson.commands.run} />
  <KeyFileList files={lesson.code.keyFiles} />
  <EvidencePanel lesson={lesson} />
  <Content />
  <CommandBlock title="Verify" commands={lesson.commands.verify} />
  <Checkpoint sections={lesson.human.requiredSections} />
</TutorialLayout>
```

- [ ] **Step 11: Install tutorial-site dependencies**

Run:

```bash
cd tutorial-site
npm install
```

Expected: `package-lock.json` is created and install exits with status 0.

- [ ] **Step 12: Build tutorial site**

Run:

```bash
cd tutorial-site
npm run build
```

Expected: Astro check and build complete successfully.

- [ ] **Step 13: Commit tutorial site shell**

Run:

```bash
git add tutorial-site
git commit -m "feat: add manifest-driven tutorial site"
```

Expected: commit succeeds.

## Task 4: Entrypoint Routing

**Files:**

- Modify: `README.md`
- Modify: `AGENTS.md`
- Modify: `docs/agents/README.md`

- [ ] **Step 1: Update README human path**

Modify `README.md` so `For Humans` points to both the tutorial site and the legacy Markdown tutorial:

````md
## For Humans

Primary course entrypoint:

```bash
cd tutorial-site
npm install
npm run dev
```

The tutorial site is manifest-driven and renders the migrated course slice from `course/manifest.json`.

Fallback/reference material:

- Tutorial source history: [`docs/tutorial/README.md`](docs/tutorial/README.md)
- Troubleshooting: [`docs/tutorial/appendix-troubleshooting.md`](docs/tutorial/appendix-troubleshooting.md)
- Validation log: [`docs/tutorial/validation-log.md`](docs/tutorial/validation-log.md)
````

- [ ] **Step 2: Update README project map**

Modify the `Project Map` in `README.md` so it includes:

```text
  course/                    manifest, lessons, agent task packets, evidence placeholders
  tutorial-site/             Astro tutorial site generated from the course manifest
```

Expected: existing entries for `experiments/`, `demo/`, `docs/agents/`, and `tools/` remain.

- [ ] **Step 3: Update `AGENTS.md` startup**

Modify `AGENTS.md` so first actions include:

```md
1. Run `git status --short --untracked-files=all`.
2. Read `course/manifest.json` for lesson-scoped work.
3. For lesson-scoped work, open the matching `course/agent-tasks/<lesson-id>.md`.
4. Use `docs/agents/task-types.md` only after deciding whether the request is lesson-scoped.
```

Expected: existing verification and git safety rules remain.

- [ ] **Step 4: Update `docs/agents/README.md`**

Modify `docs/agents/README.md` so it states:

```md
Per-lesson task packets live in `course/agent-tasks/`. Use this directory only for general agent operating rules.
```

Expected: the file still links to task types, task template, development protocol, verification, and boundaries.

- [ ] **Step 5: Verify course and shell docs**

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

`bash -n` and `git diff --check` exit with status 0.

- [ ] **Step 6: Commit entrypoint routing**

Run:

```bash
git add README.md AGENTS.md docs/agents/README.md
git commit -m "docs: route humans and agents through course manifest"
```

Expected: commit succeeds.

## Task 5: Full Verification Pass

**Files:**

- No planned file changes.

- [ ] **Step 1: Run structural checks**

Run:

```bash
./tools/check-course.sh
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
bash -n tools/check-course.sh
git diff --check
```

Expected:

```text
Course manifest OK.
```

All shell syntax and diff checks exit with status 0.

- [ ] **Step 2: Run .NET build**

Run:

```bash
dotnet build GameDemo.sln -m:1
```

Expected:

```text
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

- [ ] **Step 3: Run game-loop non-GUI test**

Run:

```bash
dotnet run --project experiments/e01-game-loop.Tests/E01GameLoop.Tests.csproj
```

Expected: test runner exits with status 0.

- [ ] **Step 4: Run tutorial site build**

Run:

```bash
cd tutorial-site
npm run build
```

Expected: Astro check and build complete successfully.

- [ ] **Step 5: Record any skipped GUI checks**

If `./tools/check-tutorial.sh` is not run because it opens DesktopGL windows, record this in the final implementation report:

```text
Skipped: ./tools/check-tutorial.sh
Reason: opens GUI smoke windows; structural course verification and non-GUI .NET checks passed.
```

If GUI smoke is acceptable in the current session, run:

```bash
./tools/check-tutorial.sh
```

Expected: command exits with status 0.

- [ ] **Step 6: Final status**

Run:

```bash
git status --short --untracked-files=all
```

Expected: no uncommitted files except intentionally uncommitted local artifacts such as generated `tutorial-site/dist/` if it is ignored. If untracked generated files appear, either add the required source files or ignore generated output explicitly.

## Self-Review Checklist

Spec coverage:

- Dual-track contract: Task 1 creates human lessons and agent task packets; Task 2 verifies both tracks.
- Manifest schema: Task 1 creates `course/schema.json`; Task 2 enforces the practical rules.
- End-to-end sample: Task 1 implements `03-game-loop`; Task 3 renders it.
- Tutorial site IA: Task 3 creates Astro pages, layout, and components.
- Agent workflow: Task 4 updates `AGENTS.md` and `docs/agents/README.md`.
- Verification design: Task 2 adds `check-course`; Task 5 runs the full pass.
- Migration rules: Task 1 migrates `01-setup` and `03-game-loop`; no old docs are deleted.
- Mainline drift guard: Tasks block experiment and demo edits except through spec-required rules.

Known non-goals preserved:

- No Godot track.
- No new experiments.
- No integrated-demo product expansion.
- No marketing landing page.
- No video generation.
- No screenshot automation in the first slice.

Execution handoff:

- Use `superpowers:subagent-driven-development` for parallel execution by task when multiple workers are available.
- Use `superpowers:executing-plans` for inline execution when a single session should apply the plan step by step.
