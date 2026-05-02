# Phase 1 Closeout

Date: 2026-05-02

## Decision

Phase 1 is complete. This repository should remain a MonoGame learning and evaluation record, not grow into a larger game project.

The useful next move is a separate Godot parity experiment that rebuilds the same tiny collector demo and compares development speed, tool friction, and packaging effort.

## Final State

- Week 0 bootstrap is complete.
- Week 1 framework-entry experiments are complete.
- Week 2 required capability experiments are complete.
- Week 2 optional 3D and shader stretch experiments were deferred and should stay deferred.
- Week 3 integrated demo and evaluation report are complete.

Key local entry points:

- Integrated demo: `demo/integrated-demo/IntegratedDemo.csproj`
- Evaluation report: `docs/reports/monogame-technical-evaluation.md`
- Roadmap checklist: `docs/00-roadmap.md`
- Godot parity plan: `docs/superpowers/plans/2026-05-02-godot-parity-demo.md`

## Final Recommendation

Use MonoGame again when the goal is to learn or own engine-shaped code: loop timing, rendering, input polling, content builds, audio routing, collision, animation, and publishing mechanics.

Do not use MonoGame as the default path for quickly making finished games. For that, run the Godot parity experiment next and compare the same feature set against editor-first tooling.

## Verification Commands

Run these before treating the closeout as final:

```bash
git diff --check
dotnet build GameDemo.sln --no-restore -m:1
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
git status --short --untracked-files=all
```

Expected evidence:

- `git diff --check` exits 0.
- `dotnet build GameDemo.sln --no-restore -m:1` exits 0 with 0 warnings and 0 errors.
- Demo smoke logs `Phase: won.`, `Phase: restarted.`, and `Smoke: exit.`
- After commit and tag, `git status --short --untracked-files=all` prints no changes.

## Tag

Create an annotated tag after the closeout commit:

```bash
git tag -a phase1-complete -m "Phase 1 MonoGame research complete"
```
