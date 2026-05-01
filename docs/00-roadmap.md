# Roadmap

Executable checklist derived from `docs/superpowers/specs/2026-04-26-monogame-research-design.md`. The spec is the source of truth for *why*; this file is the source of truth for *what's done yet*.

Convention: tick boxes as work lands. Each acceptance check must be observable from running the program — not "I think it works."

## Week 0 — Bootstrap

- [x] Spec approved.
- [x] `README.md` exists at repo root.
- [x] `GameDemo.sln` exists with solution folders for experiments / demo / tools.
- [x] `tools/check-env.sh` exists and is executable.
- [x] `global.json` pins .NET 10 SDK at repo root.
- [x] `.editorconfig` at repo root.
- [x] `.gitignore` covers .NET / MonoGame build artifacts.
- [x] `docs/00-roadmap.md` (this file) exists.
- [x] `./tools/check-env.sh` exits 0 on the working machine.

Exit: `check-env.sh` is green and the repo can scaffold a MonoGame project via `dotnet new mgdesktopgl`.

## Week 1 — Framework Entry

Goal: working mental model of MonoGame; can scaffold and run basic projects from the template without consulting external docs.

### Notes to write

- [x] `docs/01-monogame-overview.md` — what MonoGame is, what it isn't, lifecycle in plain language.
- [x] `docs/02-engine-comparison.md` — initial draft of MonoGame vs Unity / Godot / raylib / hand-rolled.
- [x] `docs/03-content-pipeline.md` — MGCB workflow, build vs runtime, file formats.

### Experiments

- [x] `experiments/e01-game-loop`
  - Acceptance: window opens at fixed 60 Hz; F1 toggles fixed/variable timestep; frame number printed each second to stdout.
  - Current: implementation, loop-state test, build, fixed-mode run smoke, and automated toggle smoke passed.
- [x] `experiments/e02-2d-rendering`
  - Acceptance: 1000 sprites render at ≥60 fps on the dev machine; F1 toggles batched vs unbatched draw and on-screen frame-time text reflects the difference.
  - Current: behavior test, solution build, SpriteFont/texture content load, and automated batched/unbatched smoke passed.
- [x] `experiments/e03-input`
  - Acceptance: keyboard, mouse, and (if available) gamepad each move a sprite; edge-detection helper distinguishes "pressed this frame" from "held".
  - Current: behavior test, keyboard/mouse/gamepad-style smoke, and solution build passed.
- [x] `experiments/e05-content-pipeline`
  - Acceptance: at least one PNG, one `SpriteFont`, and one audio clip load via Content; one deliberate `.mgcb` error is reproduced and the failure mode is documented in the experiment README.
  - Current: PNG, SpriteFont, WAV, smoke test, deliberate MGCB failure, and solution build passed.

### Per-experiment README requirement

Every experiment under `experiments/` must have a `README.md` with these three sections filled in:

1. How to run.
2. What you should see (including key bindings).
3. What was learned (friction, takeaways, links into `docs/`).

Without these three sections, the experiment is not done — even if the program works.

### Exit criteria for Week 1

- [x] I can `dotnet new mgdesktopgl -o experiments/eXX-foo -n EXXFoo` and add the project to `GameDemo.sln` from memory.
- [x] I can load a PNG and a `SpriteFont` and render them at a known position without rereading docs.
- [x] I can articulate, in writing, what `Initialize` / `LoadContent` / `Update` / `Draw` each own and on what thread they run.
- [x] All four Week 1 experiments build, run, and pass their acceptance checks.

## Week 2 — Capability Experiments

Goal: probe the harder MonoGame subsystems in isolation; produce a concrete friction list with citations.

Four required experiments + two optional stretch experiments. Pull stretch ones in only if the required four finish ahead of schedule. If stretch is skipped, record the reason (deferred vs dropped) at the bottom of this section.

### Required experiments

- [x] `experiments/e04-audio`
  - Acceptance: short `SoundEffect` plays on key press without audible delay; one `Song` loops; toggling music does not glitch sound effects.
  - Current: behavior test, solution build, and automated music/effect smoke passed.
- [x] `experiments/e06-camera-and-collision`
  - Acceptance: 2D camera supports translate + zoom via input; AABB and circle collision tested with at least one moving and one static body; collision flags rendered as colored outlines.
  - Current: camera/collision behavior test, project build, and automated pan/zoom/collision smoke passed.
- [ ] `experiments/e07-animation`
  - Acceptance: sprite animates frame-by-frame; idle/walk/jump state machine reacts to input; transitions logged to stdout when they fire.
- [ ] `experiments/e10-publishing`
  - Acceptance: `dotnet publish -c Release -r osx-x64` (and/or `osx-arm64`) produces a runnable bundle; the bundle launches without the SDK on PATH; bundle layout documented.

### Optional stretch experiments

- [ ] `experiments/e08-basic-3d`
  - Acceptance: a model loads via Content, renders with `BasicEffect` lighting, and an orbit camera works via mouse drag.
- [ ] `experiments/e09-shader`
  - Acceptance: a custom `.fx` shader compiles via MGFX, renders a sprite with a non-trivial effect (e.g. wave distortion or grayscale), and exposes one runtime-tunable parameter via keyboard.

### Notes to update

- [ ] `docs/02-engine-comparison.md` updated with a "what's painful in MonoGame" subsection citing specific experiments.
- [ ] `docs/04-platforms-and-publishing.md` exists with the publishing findings from `e10-publishing`.

### Stretch decision

Record here at end of Week 2 whether `e08-basic-3d` and `e09-shader` were:

- [ ] completed,
- [ ] deferred to Week 3 if time allows in the integrated demo,
- [ ] dropped to follow-up phase.

### Exit criteria for Week 2

- [ ] All four required Week 2 experiments build and run on macOS DesktopGL.
- [ ] Each completed experiment README is non-empty with the three required sections.
- [ ] A first-pass "should I keep using MonoGame?" opinion exists in scratch form, supported by experiment evidence.
- [ ] Stretch decision above is filled in.

## Week 3 — Integrated Demo And Evaluation

Goal: small playable demo + honest written evaluation.

### Demo

- [ ] Genre chosen and recorded in `demo/integrated-demo/README.md` with one paragraph of justification, drawing from Week 2 friction findings.
- [ ] `demo/integrated-demo` builds via `GameDemo.sln`.
- [ ] Core loop is playable end-to-end: start screen → play → win/lose → restart or quit.
- [ ] Decide whether to include a basic 3D or shader scene as a validation harness; record the decision and (if included) ship one such scene. Encouraged but not mandatory — only include if the optional Week 2 stretch experiments succeeded and the cost is low.
- [ ] `demo/integrated-demo/README.md` documents controls, how to run, and known limitations.

### Evaluation report

- [ ] `docs/reports/monogame-technical-evaluation.md` exists.
- [ ] Each strength/weakness/risk claim cites a specific experiment or demo behavior.
- [ ] Comparison set matches the spec: Unity, Godot, raylib, hand-rolled. No other engines introduced.
- [ ] Closes with a one-paragraph answer to "should I keep using MonoGame for my next project?"

### Exit criteria for Week 3

- [ ] Demo can be played start-to-finish without crashing.
- [ ] Evaluation report has zero unsupported claims.
- [ ] Top-level `README.md` updated to point at the demo and the evaluation report.

## Abort checks (run weekly)

If any of these become true, stop forward work, redirect remaining time to writing a *negative* evaluation report, and treat that as a successful Phase 1 outcome.

- [ ] MonoGame templates do not run on the target macOS / .NET 10 environment after a reasonable bootstrap effort.
- [ ] MGCB Editor unusable on Apple Silicon AND hand-editing `.mgcb` is also blocked (not merely awkward).
- [ ] A blocking MonoGame bug prevents Week 1 experiments from completing with no near-term workaround.
- [ ] After Week 2, friction is severe enough that a Week 3 integrated demo would not be honest evidence of competence.
