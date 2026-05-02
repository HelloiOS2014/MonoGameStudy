# MonoGame Study Framework

## What This Is

This repository is a MonoGame Study Framework built from completed Phase 1 research.

It serves two audiences:

- humans learning MonoGame through the existing tutorial, experiments, and integrated demo,
- agents maintaining and extending the framework through explicit task types, boundaries, and verification commands.

It is not a production game project.

## Current Status

Phase 1 research is complete through the focused experiments, integrated demo, closeout report, and technical evaluation.

Current usage is framework-oriented:

- humans start with the tutorial,
- agents start with the agent operating contract,
- shared health is proven through the verification commands below.

## For Humans

Start here:

- Tutorial: [`docs/tutorial/README.md`](docs/tutorial/README.md)
- Troubleshooting: [`docs/tutorial/appendix-troubleshooting.md`](docs/tutorial/appendix-troubleshooting.md)
- Validation log: [`docs/tutorial/validation-log.md`](docs/tutorial/validation-log.md)

The tutorial is Markdown-first. It walks through the existing MonoGame experiments and integrated demo without changing the code.

## For Agents

Start here:

- Agent entrypoint: [`AGENTS.md`](AGENTS.md)
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
dotnet restore GameDemo.sln
dotnet build GameDemo.sln -m:1
./tools/check-tutorial.sh
```

Fast documentation checks:

```bash
git diff --check
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
```

`tools/check-tutorial.sh` opens short-lived MonoGame DesktopGL smoke windows and publishes `e10`.

## Boundaries

See [`docs/agents/boundaries.md`](docs/agents/boundaries.md).

High-level boundaries:

- no Godot track,
- no tutorial website in this pass,
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
  tools/                     environment and tutorial verification
  docs/tutorial/             human learning path
  docs/agents/               agent operating contract
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
