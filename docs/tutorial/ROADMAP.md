# Tutorial Roadmap

This file is the planning boundary for the tutorial. It prevents the project from drifting into "do one more thing" mode.

Active dual-track v1 planning is governed by `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md`. This legacy Markdown roadmap records historical planning for `docs/tutorial/`; it does not override the v1 migration plan.

Dual-track v1 completion is governed by `docs/superpowers/specs/2026-05-03-monogame-dual-track-tutorial-v1-quality-gate-design.md` and the final audit at `docs/reports/tutorial-v1-quality-audit.md`.

## Current State

The legacy Markdown tutorial is complete. The dual-track v1 course is canonical in `course/manifest.json`, `course/lessons/`, `course/agent-tasks/`, and `tutorial-site/`. The quality audit records the completion evidence.

Legacy Markdown completion means:

- The tutorial has a navigable entry point: `docs/tutorial/README.md`.
- Every chapter from `00-intro.md` through `10-integrated-demo.md` exists.
- Troubleshooting exists at `appendix-troubleshooting.md`.
- The full command path is automated by `tools/check-tutorial.sh`.
- A validation record exists at `validation-log.md`.
- `./tools/check-tutorial.sh` has passed on the current macOS DesktopGL machine.
- `origin/main` contains the tutorial commits.

## Legacy Historical Milestones

These milestones describe the old Markdown tutorial path. They are superseded for active dual-track v1 work, where `course/` is the canonical course source.

### v1.1 Editorial Hardening

Purpose: make the existing Markdown tutorial easier to read without changing code or adding new lessons.

Allowed:

- tighten wording,
- fix broken links,
- normalize command formatting,
- trim noisy output snippets,
- add small clarifying notes discovered by dry runs.

Not allowed:

- new MonoGame experiments,
- new game features,
- website tooling,
- screenshots or video capture infrastructure.

Exit criteria:

- `./tools/check-tutorial.sh` passes,
- Markdown links and chapter structure are checked,
- changes are committed as documentation-only.

### v1.2 Visual Evidence

Purpose: add screenshots or short visual captures so a reader can compare what they see with expected output.

Allowed:

- screenshots for each GUI experiment,
- a screenshot for the integrated demo,
- a short note beside each image explaining the expected visual state.

Not allowed:

- redesigning the game UI,
- changing experiment behavior to look better,
- adding a screenshot framework that requires a long maintenance burden.

Exit criteria:

- screenshots are referenced from the relevant chapters,
- image files are reasonably sized,
- `./tools/check-tutorial.sh` still passes.

### v1.3 Static Tutorial Site (Legacy/Superseded)

Purpose: historically, publish the existing Markdown tutorial as a simple website.

Allowed:

- static-site wrapper around the existing tutorial content,
- navigation sidebar,
- code block styling,
- GitHub Pages or equivalent static hosting.

Not allowed:

- rewriting the tutorial into a separate source of truth,
- adding accounts, comments, analytics, or backend services,
- changing the MonoGame demo as part of website work.

Historical exit criteria:

- the legacy Markdown tutorial remains readable as migration source,
- active dual-track v1 source-of-truth decisions remain governed by the quality-gate spec,
- site build is scripted,
- local site build and `./tools/check-tutorial.sh` both pass.

### v2 Separate Tiny MonoGame Game

Purpose: use what was learned here to start a real tiny game prototype.

Allowed:

- a new repository or isolated project,
- one screen,
- one mechanic,
- one level,
- a strict first-playable stop point.

Not allowed in this repository:

- expanding `demo/integrated-demo` into the game,
- adding large game systems,
- switching to Godot,
- treating this research repo as the production game.

Exit criteria:

- separate spec,
- separate implementation plan,
- separate verification commands.

## Stop Rules

Do not continue from this legacy roadmap by picking a random "next nice thing." Stop and write or follow the active quality-gate spec when:

- the work does not fit v1.1, v1.2, v1.3, or v2,
- the work changes MonoGame runtime behavior,
- the work introduces a new toolchain,
- the work would take more than one focused milestone to explain.

## Operating Rule

Every future milestone needs:

1. a written design/spec,
2. a task plan,
3. commits per coherent slice,
4. verification evidence before claiming completion,
5. a push to `origin/main` when done.
