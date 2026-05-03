# MonoGame Dual-Track Tutorial V1 Quality Gate Design

Date: 2026-05-03
Status: Draft for owner review

## Purpose

Define the real v1 target for `game_demo` and the review system that decides whether it is complete.

The previous dual-track architecture spec is superseded because it allowed a two-lesson slice, a partial site, and a partial manifest to look like an acceptable milestone. That is not enough for this project.

This spec makes the target explicit:

- `course/` is the only course source of truth.
- Humans learn through `tutorial-site/`.
- Agents work through `AGENTS.md`, `course/manifest.json`, and per-lesson task packets.
- The finished v1 must cover the complete 00-10 MonoGame learning path.
- Completion is blocked unless both scored dimensions are at least 95/100:
  - content quality,
  - mainline alignment.

## Current State Audit

Actual current state as of this spec:

| Area | Evidence | Reading |
| --- | --- | --- |
| Legacy human tutorial | `docs/tutorial/00-intro.md` through `docs/tutorial/10-integrated-demo.md` exist. | Useful migration source. |
| Canonical course lessons | `course/lessons/01-setup.mdx` and `course/lessons/03-game-loop.mdx` only. | Incomplete. |
| Agent task packets | `course/agent-tasks/01-setup.md` and `course/agent-tasks/03-game-loop.md` only. | Incomplete. |
| Manifest | `course/manifest.json` lists only setup and game loop. | Incomplete and not v1-ready. |
| Tutorial site | `tutorial-site` renders the current manifest slice. | Functional shell, not full course. |
| Structural verifier | `tools/check-course.mjs` validates listed manifest entries. | Does not enforce full 00-10 coverage. |
| README | Routes humans to `tutorial-site` while saying the site renders the migrated course slice. | Too easy to misread as full tutorial readiness. |

Current implementation score under this spec:

| Dimension | Score | Reason |
| --- | ---: | --- |
| Content quality | 50/100 | Only 2 of 11 canonical lessons exist under `course/`; no complete site course; no full agent track. |
| Mainline alignment | 74/100 | It stays on MonoGame and avoids Godot/game expansion, but partial course artifacts were allowed to look like the main deliverable. |

These scores are not completion scores. They are the baseline that this spec is designed to correct.

## Target Product

V1 is a complete MonoGame study tutorial, not a framework sketch.

Human learner target:

- A learner can open `tutorial-site`, follow lessons 00-10 in order, run the relevant MonoGame experiment or demo for each lesson, inspect the key files, understand the expected visual state, complete the exercise, and pass the checkpoint.

Agent maintainer target:

- An agent can receive a short task such as "tighten the input lesson checkpoint" or "add expected visual state for game loop", map it to a lesson from the manifest, open the matching task packet, stay inside allowed files, detect spec-required runtime changes, run the right verification commands, and report evidence without the owner restating long context.

Repository target:

- `course/` is the canonical course source.
- `tutorial-site/` renders `course/`.
- `docs/tutorial/` is legacy migration material and history only.
- `experiments/` and `demo/integrated-demo/` remain source examples and verification targets, not a new production game.

## Canonical Course Scope

The v1 course must include exactly these canonical lessons:

| Order | Lesson id | Migration source | Kind | Runtime mapping |
| ---: | --- | --- | --- | --- |
| 0 | `00-intro` | `docs/tutorial/00-intro.md` | `orientation` | `demo/integrated-demo` overview commands |
| 1 | `01-setup` | `docs/tutorial/01-setup.md` | `setup` | `tools/check-env.sh` |
| 2 | `02-first-window` | `docs/tutorial/02-first-window.md` | `experiment` | `experiments/e01-game-loop/Program.cs`, `Game1.cs`, and `E01GameLoop.csproj` as the first DesktopGL window path |
| 3 | `03-game-loop` | `docs/tutorial/03-game-loop.md` | `experiment` | `experiments/e01-game-loop` |
| 4 | `04-rendering` | `docs/tutorial/04-rendering.md` | `experiment` | `experiments/e02-2d-rendering` |
| 5 | `05-input` | `docs/tutorial/05-input.md` | `experiment` | `experiments/e03-input` |
| 6 | `06-content-pipeline` | `docs/tutorial/06-content-pipeline.md` | `experiment` | `experiments/e05-content-pipeline` |
| 7 | `07-audio` | `docs/tutorial/07-audio.md` | `experiment` | `experiments/e04-audio` |
| 8 | `08-camera-collision-animation` | `docs/tutorial/08-camera-collision-animation.md` | `experiment` | `experiments/e06-camera-and-collision` and `experiments/e07-animation` |
| 9 | `09-publishing` | `docs/tutorial/09-publishing.md` | `experiment` | `experiments/e10-publishing` |
| 10 | `10-integrated-demo` | `docs/tutorial/10-integrated-demo.md` | `capstone` | `demo/integrated-demo` |

No lesson outside this set belongs in v1 unless a new spec replaces this one.

## Required Directory Shape

```text
course/
  manifest.json
  schema.json
  lessons/
    00-intro.mdx
    01-setup.mdx
    02-first-window.mdx
    03-game-loop.mdx
    04-rendering.mdx
    05-input.mdx
    06-content-pipeline.mdx
    07-audio.mdx
    08-camera-collision-animation.mdx
    09-publishing.mdx
    10-integrated-demo.mdx
  agent-tasks/
    00-intro.md
    01-setup.md
    02-first-window.md
    03-game-loop.md
    04-rendering.md
    05-input.md
    06-content-pipeline.md
    07-audio.md
    08-camera-collision-animation.md
    09-publishing.md
    10-integrated-demo.md
  evidence/
    <lesson-id>/
      expected-state.md
      optional-screenshot-or-capture-assets
```

`docs/tutorial/` remains in the repository during v1 as a legacy migration source. It must not be described as the primary learning route once v1 is implemented.

## Human Lesson Contract

Every file under `course/lessons/` must use this section set:

- `Goal`
- `Run`
- `Observe`
- `Expected Visual State`
- `Key Files`
- `Walkthrough`
- `Common Failures`
- `Exercise`
- `Checkpoint`
- `Next`

Rules:

- `Goal` states what the learner will understand after the lesson.
- `Run` includes concrete commands, not vague prose.
- `Observe` describes stdout, window behavior, or both.
- `Expected Visual State` is mandatory for every GUI lesson. Screenshots are optional in v1, but textual expected state is not optional.
- `Key Files` links to real experiment, demo, test, content, or tool files.
- `Walkthrough` explains the code relationship, not generic MonoGame theory.
- `Common Failures` uses issues likely in this repo on macOS DesktopGL.
- `Exercise` is a small inspection or modification task that does not require new game features.
- `Checkpoint` is observable or explainable.
- `Next` points to the next canonical lesson or completion.

## Agent Task Packet Contract

Every file under `course/agent-tasks/` must use this section set:

- `Task`
- `Context`
- `Allowed Files`
- `Blocked Files`
- `Spec Required`
- `Commands`
- `Acceptance`
- `Failure Handling`
- `Report Format`

Rules:

- `Task` says what an agent may maintain for that lesson.
- `Context` maps the lesson to experiments, demo, docs, and migration source.
- `Allowed Files` is narrow and lesson-scoped.
- `Blocked Files` prevents unrelated lesson churn and game expansion.
- `Spec Required` lists runtime files or behavior that cannot be changed without a new approved spec.
- `Commands` includes structural and lesson-specific verification.
- `Acceptance` is observable.
- `Failure Handling` says how to report blocked GUI, network, SDK, or content pipeline checks.
- `Report Format` forces changed files, commands, results, and skipped checks.

## Manifest Contract

`course/manifest.json` is the course control plane.

The manifest must contain:

- the canonical lesson list in order 0-10,
- lesson id, order, title, summary, and kind,
- migration source,
- human lesson path,
- agent task path,
- site route slug,
- code projects,
- test projects,
- key files,
- run commands,
- verify commands,
- evidence status and paths.

The site must not maintain a second lesson list.

Agent docs must not maintain a second lesson list.

If a lesson is missing from the manifest, v1 is incomplete.

## Tutorial Site Contract

`tutorial-site/` must be a real course entrypoint:

- Home page lists all 00-10 lessons from the manifest.
- Lesson pages render every manifest lesson.
- Navigation comes only from the manifest.
- Lesson pages expose:
  - title and summary,
  - run commands,
  - verify commands,
  - key files,
  - evidence status,
  - checkpoint,
  - link to the agent task packet for maintainers.
- `npm run build` must build all lesson routes.

V1 does not require:

- Remotion video,
- advanced SourceReader,
- screenshot gallery,
- Playwright visual regression.

Those are allowed future work after v1.

## Verifier Contract

`tools/check-course.sh` and `tools/check-course.mjs` must enforce the v1 course contract.

Required checks:

- `course/manifest.json` exists and parses.
- `course/schema.json` exists and parses.
- manifest lesson ids exactly match the canonical 00-10 set.
- manifest orders are exactly 0 through 10.
- every human lesson path exists.
- every agent task path exists.
- every migration source path exists.
- every route slug is unique.
- every required human heading exists.
- every required agent heading exists.
- every code project, test project, and key file path exists, unless explicitly marked not applicable.
- every run command and verify command is non-empty where required.
- every GUI lesson has `Expected Visual State`.
- every evidence entry has a status, reason when pending/not applicable, and expected paths when available.
- `tutorial-site/src/pages/[...lesson].astro` exists.
- `tutorial-site/src/pages/index.astro` exists.
- README does not describe `docs/tutorial/` as the primary course entrypoint.

What `check-course` does not prove:

- MonoGame windows launch.
- screenshots match actual runtime output.
- lesson prose is excellent.
- an agent made a semantically correct edit.

Those must be checked through the scoring audit and runtime verification.

## Runtime Verification Contract

Completion audit must report these commands:

```bash
./tools/check-course.sh
npm run build
dotnet build GameDemo.sln -m:1
./tools/check-tutorial.sh
```

`npm run build` runs inside `tutorial-site/`.

`./tools/check-tutorial.sh` opens DesktopGL smoke windows. If GUI access is blocked, the final report must say it is blocked and must not claim visual verification completion.

Passing these commands is required but not sufficient. They are evidence inputs, not the scoring result.

## Hard Score Caps

The scoring system must apply caps before adding points.

Content quality caps:

| Condition | Max content quality score |
| --- | ---: |
| `course/` does not contain all 00-10 lessons | 70 |
| Any lesson lacks an agent task packet | 75 |
| Any lesson lacks `Expected Visual State` | 85 |
| Tutorial site does not render all manifest lessons | 85 |
| README misstates primary entrypoints | 85 |
| `check-course` does not enforce full canonical lesson coverage | 90 |
| Prompt-to-artifact audit is missing | 90 |

Mainline alignment caps:

| Condition | Max mainline alignment score |
| --- | ---: |
| Work adds Godot or another engine track | 60 |
| Work expands `demo/integrated-demo` into a production game | 70 |
| `docs/tutorial/` and `course/` both act as primary sources | 80 |
| Agent docs duplicate per-lesson truth outside task packets | 85 |
| Site keeps an independent lesson list outside the manifest | 85 |
| Verifier green status is used as completion proof beyond its coverage | 90 |
| No explicit anti-drift audit is present | 90 |

If any cap applies, the score cannot exceed that cap.

## Content Quality Scoring

Content quality is scored out of 100:

| Category | Max | Evidence required |
| --- | ---: | --- |
| Canonical lesson completeness | 20 | All 00-10 lesson files, manifest entries, and routes exist. |
| Human learning usefulness | 20 | Each lesson has concrete run/observe/walkthrough/exercise/checkpoint content tied to repo code. |
| Visual expectation clarity | 15 | GUI lessons explain expected visual state; non-GUI lessons explain command output. |
| Code mapping quality | 15 | Key files, projects, tests, and content pipeline assets are correct and useful. |
| Agent task usefulness | 15 | Every task packet can guide a short lesson-scoped maintenance task. |
| Site usability | 10 | Buildable site, complete navigation, readable lesson pages, no parallel lesson list. |
| Entry point honesty | 5 | README, AGENTS, and legacy docs state the true source of truth. |

To score 95 or higher, no category may be below 90 percent of its max.

## Mainline Alignment Scoring

Mainline alignment is scored out of 100:

| Category | Max | Evidence required |
| --- | ---: | --- |
| `course/` source-of-truth discipline | 25 | Manifest, lessons, task packets, site, and agent flow use `course/`. |
| MonoGame research continuity | 20 | Lessons map to existing experiments, demo, tools, and validation logs. |
| Scope control | 15 | No Godot track, no production game expansion, no new experiments without spec. |
| Human/agent separation | 15 | Human lessons do not embed agent operating rules; task packets do not become learner prose. |
| Verifier coverage discipline | 15 | Verifier coverage is explicit and not overclaimed. |
| Documentation consistency | 10 | README, AGENTS, roadmap, and spec agree on the current route. |

To score 95 or higher, no category may be below 90 percent of its max.

## Review Workflow

Every design, plan, implementation batch, and final report must include:

1. Objective restatement.
2. Prompt-to-artifact checklist.
3. Evidence table with file paths, commands, or concrete inspection results.
4. Hard cap check.
5. Content quality score.
6. Mainline alignment score.
7. Counterargument review: why the score might be too high.
8. Required corrections if either score is below 95.

Do not use intent, effort, or passing commands as a substitute for the checklist.

## Prompt-To-Artifact Checklist For V1

| Requirement | Required artifact or evidence |
| --- | --- |
| User wants a tutorial, not only Markdown. | `tutorial-site` builds and renders all 00-10 lessons from `course/manifest.json`. |
| Human and agent lines are both required. | Every lesson has both `course/lessons/<id>.mdx` and `course/agent-tasks/<id>.md`. |
| Agent should not need long repeated prompts. | Each task packet has allowed files, blocked files, spec-required files, commands, acceptance, and report format. |
| `course/` is canonical. | README, AGENTS, site data loader, and verifier all route through `course/manifest.json`. |
| Existing MonoGame project remains the subject. | Lessons map to `experiments/`, `demo/integrated-demo`, and `tools/check-tutorial.sh`. |
| No Godot. | README/spec/AGENTS boundaries keep Godot out of scope. |
| No production game expansion. | Integrated demo stays capstone evidence, not product scope. |
| Scoring is required. | Review workflow includes content quality and mainline alignment scores. |
| Scores must exceed 95. | Completion audit blocks if either score is below 95. |
| Scoring must be rigorous. | Hard caps, evidence table, and counterargument review are required. |
| No proxy completion signals. | Runtime checks are evidence inputs only; scoring still audits coverage. |

## Anti-Proxy Rules

The following claims are invalid:

- "`npm run build` passed, so the tutorial is complete."
- "`check-course` passed, so content quality is high."
- "`dotnet build` passed, so human lessons are correct."
- "`check-tutorial` passed, so the site is complete."
- "The design sounds complete, so it scores 95."

Valid claims must say exactly what the evidence proves and what it does not prove.

## Implementation Direction

The implementation plan should not start with new UI polish.

Required order:

1. Update source-of-truth docs so README/AGENTS stop overstating the current partial site.
2. Upgrade `course/manifest.json` and `tools/check-course.mjs` to enforce canonical 00-10 coverage.
3. Migrate lessons one by one from `docs/tutorial/` into `course/lessons/`.
4. Add matching agent task packets in the same batch as each lesson.
5. Update the tutorial site only to consume and render the expanded manifest.
6. Run structural, site, .NET, and tutorial smoke verification.
7. Perform the two-score completion audit.

This order keeps the work on the tutorial mainline and prevents more site shell work from masking missing content.

## Approval Gate

This spec does not authorize implementation yet.

Approval required:

- Owner accepts this quality gate as the v1 target.
- Owner accepts that old two-lesson architecture is superseded.
- Owner accepts that implementation cannot be called complete unless both scored dimensions are at least 95/100 with evidence.

After approval, the next artifact is an implementation plan, not immediate code edits.
