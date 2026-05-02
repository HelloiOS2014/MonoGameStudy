# MonoGame Tutorial Productization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Turn the Markdown tutorial into a self-contained tutorial package with a clear entry point, one-command dry-run verification, and recorded validation evidence.

**Architecture:** Keep this as documentation-first infrastructure inside the existing repo. Add a tutorial index under `docs/tutorial/`, add a repo-level verification script under `tools/`, and update README/appendix links so readers know how to navigate and verify the tutorial.

**Tech Stack:** Markdown, Bash, existing .NET 10 / MonoGame projects, existing smoke environment variables.

---

## Task 1: Tutorial Index And README

**Files:**
- Create: `docs/tutorial/README.md`
- Modify: `README.md`

- [ ] **Step 1: Create tutorial index**

Create `docs/tutorial/README.md` with:

```markdown
# MonoGame Tutorial

This tutorial teaches the existing MonoGame research repo as a finished learning path. Start with setup, move through one concept per experiment, then finish with the integrated 2D collector demo.

## Path

| Order | Chapter | What It Proves |
| --- | --- | --- |
| 0 | [Intro](00-intro.md) | Repo shape and integrated demo goal |
| 1 | [Setup](01-setup.md) | macOS, .NET 10, MonoGame template readiness |
| 2 | [First Window](02-first-window.md) | MonoGame project lifecycle |
| 3 | [Game Loop](03-game-loop.md) | Fixed vs variable timestep |
| 4 | [Rendering](04-rendering.md) | SpriteBatch drawing and batching |
| 5 | [Input](05-input.md) | Polling and edge detection |
| 6 | [Content Pipeline](06-content-pipeline.md) | MGCB asset names and failure modes |
| 7 | [Audio](07-audio.md) | SoundEffect vs Song/MediaPlayer |
| 8 | [Camera, Collision, And Animation](08-camera-collision-animation.md) | Common game-layer systems |
| 9 | [Publishing](09-publishing.md) | Self-contained macOS DesktopGL publish |
| 10 | [Integrated Demo](10-integrated-demo.md) | Complete start/play/win/restart loop |
| Appendix | [Troubleshooting](appendix-troubleshooting.md) | Diagnosis paths for common failures |

## Verify Everything

Run the dry-run script from the repo root:

```bash
./tools/check-tutorial.sh
```

The script opens short-lived MonoGame smoke windows, verifies the deliberate MGCB failure, publishes `e10`, and runs the published executable without `dotnet` on `PATH`.

## Current Validation

Latest recorded validation: [validation-log.md](validation-log.md).
```

- [ ] **Step 2: Update root README tutorial section**

Change the README tutorial entry so it points first to `docs/tutorial/README.md`, then mentions `./tools/check-tutorial.sh`.

- [ ] **Step 3: Verify**

Run:

```bash
rg -n "docs/tutorial/README.md|check-tutorial.sh|validation-log.md" README.md docs/tutorial/README.md
git diff --check
```

Expected: all three terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add README.md docs/tutorial/README.md
git commit -m "docs: add tutorial index"
```

## Task 2: One-Command Tutorial Dry Run

**Files:**
- Create: `tools/check-tutorial.sh`

- [ ] **Step 1: Create script**

Create executable `tools/check-tutorial.sh` with these responsibilities:

- Resolve repo root from the script path.
- Run environment/template checks.
- Run solution build.
- Run every tutorial smoke command.
- Verify the deliberate MGCB failure exits non-zero and mentions `missing_texture.png`.
- Publish `e10`.
- Run the published `E10Publishing` executable with a minimal `PATH`.
- Print `Tutorial dry run passed.` only if every check succeeds.

- [ ] **Step 2: Verify script syntax and permissions**

Run:

```bash
bash -n tools/check-tutorial.sh
test -x tools/check-tutorial.sh
git diff --check
```

Expected: each command exits 0.

- [ ] **Step 3: Commit**

```bash
git add tools/check-tutorial.sh
git commit -m "docs: add tutorial dry-run script"
```

## Task 3: Validation Log And Troubleshooting Links

**Files:**
- Create: `docs/tutorial/validation-log.md`
- Modify: `docs/tutorial/appendix-troubleshooting.md`

- [ ] **Step 1: Add validation log**

Create `docs/tutorial/validation-log.md` with date `2026-05-02`, machine notes, command categories, and the evidence that build/smoke/publish checks passed during the dry run.

- [ ] **Step 2: Link script from troubleshooting**

Add a short section to `appendix-troubleshooting.md` named `### Full tutorial dry run` that points to `./tools/check-tutorial.sh` and explains that it intentionally includes one expected MGCB failure.

- [ ] **Step 3: Verify**

Run:

```bash
rg -n "Full tutorial dry run|Tutorial dry run passed|2026-05-02|missing_texture.png" docs/tutorial/validation-log.md docs/tutorial/appendix-troubleshooting.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/validation-log.md docs/tutorial/appendix-troubleshooting.md
git commit -m "docs: record tutorial validation"
```

## Task 4: Final Verification And Push

**Files:**
- Modify documentation or script only if verification exposes an issue.

- [ ] **Step 1: Run structural checks**

```bash
for section in "Goal" "What You Will Run" "Key Files" "Walkthrough" "Expected Output" "Common Problems" "Checkpoint"; do
  count=$(rg -l "^## ${section}$" docs/tutorial | wc -l | tr -d ' ')
  echo "${section}: ${count}"
done
rg -n -U "## (Goal|What You Will Run|Key Files|Walkthrough|Expected Output|Common Problems|Checkpoint)\n\n##" docs/tutorial
```

Expected: each section count is `12`; empty-section scan has no matches.

- [ ] **Step 2: Run script verification**

Run:

```bash
./tools/check-tutorial.sh
```

Expected: exits 0 and prints `Tutorial dry run passed.`

- [ ] **Step 3: Final repository checks**

Run:

```bash
git diff --check
git status --short --untracked-files=all
```

Expected: diff check exits 0; status is clean after commits.

- [ ] **Step 4: Push**

```bash
git push origin main
```

Expected: `origin/main` updates to the final tutorial productization commit.

## Self-Review Checklist

- Spec coverage: The plan gives the tutorial a navigable index, a repeatable verification command, and recorded validation evidence.
- Completion scan: The plan contains concrete files, commands, expected outputs, and commit messages.
- Scope control: No website, screenshots, or new game features are introduced in this pass.
