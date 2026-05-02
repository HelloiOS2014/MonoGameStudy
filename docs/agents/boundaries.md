# Boundaries

## Purpose

These boundaries keep the repository a MonoGame Study Framework: a human learning path plus an agent operating contract built from completed Phase 1 research.

The repo is not a production game, engine comparison platform, or general tutorial website.

## Allowed

- Improve human tutorial docs.
- Improve agent docs.
- Fix broken verification scripts.
- Fix narrow build or smoke failures.
- Update README routing.
- Write research notes, specs, and implementation plans.
- Tighten verification evidence and command documentation.

## Not Allowed

- No Godot track.
- No tutorial website in this pass.
- No casual expansion of `demo/integrated-demo`.
- No new game features without spec.
- No new MonoGame experiment without approved spec or plan.
- No target platform change away from macOS DesktopGL without spec.
- No `.NET 10` pinning change without spec.
- No destructive git commands.
- No unrelated commits.

## Requires A Spec

- `Experiment` work.
- `Demo` runtime work.
- Broad `Tooling` behavior changes.
- Boundary changes.
- Architecture changes.
- Project-direction changes.
- New toolchain requirements.

## Rationale

`demo/integrated-demo` is a capstone and validation harness. Expanding it casually turns the framework into an unfinished game project.

Godot is outside this repository's future path. This repo records MonoGame research and teaches the existing MonoGame experiments.

New experiments are allowed only when a spec or plan names the experiment ID, directory, purpose, verification, and learning outcome.

New tools are allowed only when they reduce repeated verification work and include failure behavior.

## Boundary Violation Response

When a request violates a boundary:

1. Stop before implementation.
2. State the violated boundary.
3. Propose a narrower safe task or request an approved spec.
4. Do not edit runtime code while blocked.

If the user clarifies the task into an allowed scope, reclassify it and continue with the matching task type.
