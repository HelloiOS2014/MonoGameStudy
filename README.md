# MonoGame Study Framework

## What This Is

This repository is a MonoGame Study Framework built from completed Phase 1 research.

It serves two audiences:

- humans learning MonoGame through the human-facing tutorial site, course lessons, experiments, and integrated demo,
- agents maintaining and extending the framework through the course manifest, lesson task packets, explicit boundaries, and verification commands.

It is not a production game project.

## Current Status

The repository has a complete dual-track v1 tutorial.

- `course/manifest.json` is the canonical course source.
- `course/lessons/` contains the complete 00-10 human lesson path.
- `course/agent-tasks/` contains the matching 00-10 lesson task packets for agents.
- `tutorial-site/` is the human-facing tutorial site for the complete 00-10 v1 course.
- `docs/tutorial/` is the legacy migration source.

## For Humans

Primary course entrypoint:

```bash
cd tutorial-site
npm install
npm run dev
```

The tutorial site is the human-facing course reader. Agent instructions stay in `AGENTS.md`, `course/agent-tasks/`, and `docs/agents/`.

Legacy migration source:

- [`docs/tutorial/README.md`](docs/tutorial/README.md)

## Publish Tutorial Site

GitHub Pages deployment is tag-driven:

```bash
git tag vYYYY.MM.DD
git push origin vYYYY.MM.DD
```

The `Deploy Tutorial Site` workflow builds `tutorial-site/` with Astro and deploys `tutorial-site/dist` to GitHub Pages. The workflow also supports manual `workflow_dispatch`.

## For Agents

Start here:

- Agent entrypoint: [`AGENTS.md`](AGENTS.md)
- Course manifest: [`course/manifest.json`](course/manifest.json)
- Lesson task packets: [`course/agent-tasks/`](course/agent-tasks/)
- Agent guide: [`docs/agents/README.md`](docs/agents/README.md)
- Task types: [`docs/agents/task-types.md`](docs/agents/task-types.md)
- Verification matrix: [`docs/agents/verification.md`](docs/agents/verification.md)
- Boundaries: [`docs/agents/boundaries.md`](docs/agents/boundaries.md)

Agents can work from short tasks:

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

English tasks can use `Acceptance:` instead of `验收:`.

## Shared Verification

Fresh workspace path:

```bash
./tools/check-env.sh
./tools/check-course.sh
dotnet restore GameDemo.sln
dotnet build GameDemo.sln -m:1
./tools/check-tutorial.sh
```

Fast documentation checks:

```bash
git diff --check
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
bash -n tools/check-course.sh
./tools/check-course.sh
```

`tools/check-tutorial.sh` opens short-lived MonoGame DesktopGL smoke windows and publishes `e10`.

## Boundaries

See [`docs/agents/boundaries.md`](docs/agents/boundaries.md).

High-level boundaries:

- no Godot track,
- no lesson without both a human lesson and an agent task packet,
- no casual expansion of `demo/integrated-demo`,
- no new experiment without approved spec or plan,
- no target platform change away from macOS DesktopGL without spec,
- no `.NET 10` pinning change without spec.

## Project Map

```text
game_demo/
  README.md                  framework front door
  AGENTS.md                  required first-read file for agents
  GameDemo.sln               top-level solution
  global.json                .NET 10 SDK selection
  course/                    canonical manifest, lessons, agent task packets, and evidence
  tutorial-site/             Astro tutorial site generated from the course manifest
  .github/workflows/         tag-driven GitHub Pages deployment
  tools/                     environment, course, and tutorial verification
  docs/tutorial/             legacy human learning path and migration source
  docs/agents/               general agent operating contract
  docs/reports/              Phase 1 closeout and evaluation
  experiments/               focused MonoGame reference experiments
  demo/integrated-demo/      capstone validation demo
```

## Phase 1 Record

Phase 1 remains part of the project history:

- Research design: [`docs/superpowers/specs/2026-04-26-monogame-research-design.md`](docs/superpowers/specs/2026-04-26-monogame-research-design.md)
- Original roadmap: [`docs/00-roadmap.md`](docs/00-roadmap.md)
- Tutorial roadmap: [`docs/tutorial/ROADMAP.md`](docs/tutorial/ROADMAP.md)
- Closeout: [`docs/reports/phase1-closeout.md`](docs/reports/phase1-closeout.md)
- Technical evaluation: [`docs/reports/monogame-technical-evaluation.md`](docs/reports/monogame-technical-evaluation.md)
- Validation log: [`docs/tutorial/validation-log.md`](docs/tutorial/validation-log.md)

Historical note: after Phase 1, the project recommendation was to preserve this repository as a research result rather than casually expand the integrated demo into a larger game. The current framework pass keeps that boundary while adding human and agent entry points for continued study and maintenance.
