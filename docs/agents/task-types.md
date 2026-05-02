# Task Types

## Summary Table

| Type | Use For | Spec Required | Plan Required | Allowed Files | Default Verification |
| --- | --- | --- | --- | --- | --- |
| `Docs` | Tutorial, README, reports, roadmap, agent docs | For project-direction changes | For structural docs changes | `README.md`, `AGENTS.md`, `docs/**` | `git diff --check`; unresolved-marker grep on changed docs |
| `Fix` | Broken build, smoke, script, or incorrect docs | When root cause changes architecture or boundaries | When fix spans multiple areas | Failing surface plus minimal support files | Reproduce when feasible; run smallest proving command |
| `Experiment` | Add or extend `experiments/eNN-*` | Yes | Yes | Named experiment, matching tests, `GameDemo.sln`, experiment README | Targeted tests, smoke, solution build |
| `Demo` | Runtime changes under `demo/integrated-demo` | Yes | Yes | Integrated demo and matching tests | Demo tests, demo smoke, solution build |
| `Tooling` | Verification scripts and build helpers | For new scripts or behavior changes | For new scripts or broad behavior changes | `tools/**`, docs describing tool behavior | Shell syntax, focused behavior, failure path |
| `Research` | Investigation, comparison, evaluation, specs, plans | Research may produce one | Research may produce one | `docs/**` research and planning files | Local evidence, assumptions, unknowns, no runtime completion claim |

## Docs

Use for tutorial chapters, README, reports, roadmap text, and agent docs.

Small corrections do not need a spec. Structural documentation changes need a short plan. Direction changes need a spec.

Example:

```text
Docs: Tighten the setup tutorial wording for the MGCB restore step
验收: The changed chapter names the exact command and git diff has no trailing whitespace.
```

## Fix

Use for broken builds, failing smoke tests, incorrect docs, or wrong scripts.

A narrow fix does not need a spec. If root cause changes architecture or project boundaries, stop and draft a spec.

Example:

```text
Fix: Repair the e05 content smoke after an asset path rename
验收: The failing smoke is reproduced first, then the targeted smoke and GameDemo.sln build pass.
```

## Experiment

Use for adding or extending `experiments/eNN-*`.

Spec and implementation plan are required before runtime edits. Required deliverables are the experiment project, matching tests when logic can be tested without graphics, smoke mode, README, and `GameDemo.sln` entry.

Example:

```text
Experiment: Add a mouse-drag input variant to e03
验收: New smoke simulates a drag path and exits; GameDemo.sln builds with 0 errors.
```

## Demo

Use for runtime changes under `demo/integrated-demo`.

Spec and implementation plan are required. The integrated demo is a capstone and validation harness, not a production game.

Example:

```text
Demo: Fix the restart prompt copy after win state
Acceptance: Integrated demo tests pass and the smoke exits after the configured frame limit.
```

## Tooling

Use for `tools/check-env.sh`, `tools/check-tutorial.sh`, future verification scripts, and build helpers.

New scripts and behavior-changing edits require a spec or plan. Typo-only fixes do not.

Example:

```text
Tooling: Add link checks to tutorial verification
验收: The script reports a broken internal tutorial link with a non-zero exit code.
```

## Research

Use for investigation, comparison, evaluation, spec writing, and implementation planning.

Research may produce a spec or plan but must not silently implement runtime changes.

Example:

```text
Research: Evaluate whether an e08 shader experiment belongs in v2
Acceptance: The report lists local evidence, tradeoffs, and a recommendation without runtime changes.
```

## Gray Areas

Use this priority when a request fits more than one type:

```text
Demo > Experiment > Tooling > Fix > Docs > Research
```

- Tutorial wording only is `Docs`.
- Tutorial command text plus script behavior is `Tooling`.
- A new `experiments/eNN-*` directory is `Experiment`.
- Runtime change under `demo/integrated-demo` is `Demo`.
- Docs-only change under `demo/integrated-demo` is `Docs`.
- Broken smoke caused by existing code is `Fix`.
- New behavior while fixing a smoke is `Experiment` or `Demo`.
- New validation scripts or build helpers are `Tooling`.
- Study notes, comparison, evaluation, and planning are `Research`.
- If classification changes the planning gate, use the stricter gate.

## Experiment ID Rules

- Existing implemented experiments are `e01` through `e07` and `e10`.
- Roadmap entries `e08` and `e09` refer to optional 3D and shader topics unless a new spec redefines them.
- A new experiment requires an approved spec or plan that names the ID and directory.
- If the request names a concept but not an ID, propose the ID in the spec or plan before creating files.
- Do not skip to `e11` only because `e08` and `e09` are not implemented.
- Do not reuse an existing ID for a different concept.
