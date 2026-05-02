# MonoGame Tutorial Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a Markdown-first MonoGame beginner tutorial that teaches the existing experiments and integrated demo without changing game behavior.

**Architecture:** The tutorial lives under `docs/tutorial/` as self-contained Markdown chapters. Each chapter uses the same eight-section structure, links to existing projects, and includes copy-pasteable commands from the repository root. Root `README.md` points readers to `docs/tutorial/00-intro.md`.

**Tech Stack:** Markdown, existing .NET 10 / MonoGame projects, existing shell verification commands.

---

## Shared Rules

Every chapter created by this plan must include these headings in this order:

```markdown
# <Title>

## Goal

## What You Will Run

## Key Files

## Walkthrough

## Expected Output

## Common Problems

## Checkpoint
```

Every command must be runnable from `/Users/panghu/code/rsearch/game_demo` unless the chapter explicitly says otherwise. Do not rename or edit existing experiment/demo source files while implementing this tutorial.

Use these verification commands after each task:

```bash
git diff --check
git status --short --untracked-files=all
```

Use this structural check after any task that creates tutorial chapters:

```bash
rg -n "^## (Goal|What You Will Run|Key Files|Walkthrough|Expected Output|Common Problems|Checkpoint)$" docs/tutorial
```

## Task 1: Tutorial Skeleton And README Entry

**Files:**
- Create: `docs/tutorial/00-intro.md`
- Create: `docs/tutorial/01-setup.md`
- Create: `docs/tutorial/02-first-window.md`
- Create: `docs/tutorial/03-game-loop.md`
- Create: `docs/tutorial/04-rendering.md`
- Create: `docs/tutorial/05-input.md`
- Create: `docs/tutorial/06-content-pipeline.md`
- Create: `docs/tutorial/07-audio.md`
- Create: `docs/tutorial/08-camera-collision-animation.md`
- Create: `docs/tutorial/09-publishing.md`
- Create: `docs/tutorial/10-integrated-demo.md`
- Create: `docs/tutorial/appendix-troubleshooting.md`
- Modify: `README.md`

- [ ] **Step 1: Create the tutorial directory and chapter files**

Create `docs/tutorial/` and add all 12 chapter files listed above. Each file initially contains the shared heading skeleton and a title matching its filename:

```markdown
# Intro

## Goal

## What You Will Run

## Key Files

## Walkthrough

## Expected Output

## Common Problems

## Checkpoint
```

Use these exact titles:

```text
00-intro.md -> # Intro
01-setup.md -> # Setup
02-first-window.md -> # First Window
03-game-loop.md -> # Game Loop
04-rendering.md -> # Rendering
05-input.md -> # Input
06-content-pipeline.md -> # Content Pipeline
07-audio.md -> # Audio
08-camera-collision-animation.md -> # Camera, Collision, And Animation
09-publishing.md -> # Publishing
10-integrated-demo.md -> # Integrated Demo
appendix-troubleshooting.md -> # Troubleshooting
```

- [ ] **Step 2: Add README tutorial entry**

Modify `README.md` near the integrated demo section and add:

```markdown
## Tutorial

Start here: [`docs/tutorial/00-intro.md`](docs/tutorial/00-intro.md).

The tutorial is Markdown-first. It walks through the existing MonoGame experiments and integrated demo without changing the code.
```

- [ ] **Step 3: Verify skeleton**

Run:

```bash
rg -l "^# " docs/tutorial | wc -l
rg -n "^## Checkpoint$" docs/tutorial | wc -l
git diff --check
```

Expected:

```text
12
12
```

and `git diff --check` exits 0.

- [ ] **Step 4: Commit**

```bash
git add README.md docs/tutorial
git commit -m "docs: add tutorial skeleton"
```

## Task 2: Intro And Setup Chapters

**Files:**
- Modify: `docs/tutorial/00-intro.md`
- Modify: `docs/tutorial/01-setup.md`

- [ ] **Step 1: Write `00-intro.md`**

Content requirements:

- `Goal`: state that the tutorial turns the existing research repo into a path from zero MonoGame knowledge to a tiny 2D collector demo.
- `What You Will Run`: include:

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

and:

```bash
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

- `Key Files`: link to `README.md`, `docs/00-roadmap.md`, `docs/reports/monogame-technical-evaluation.md`, and `demo/integrated-demo/README.md`.
- `Walkthrough`: explain `experiments/` as topic-focused finished states and `demo/integrated-demo/` as the capstone.
- `Expected Output`: include the smoke lines `Phase: won.`, `Phase: restarted.`, and `Smoke: exit.`
- `Common Problems`: mention that the tutorial assumes macOS DesktopGL and that first restore/build can take minutes.
- `Checkpoint`: tell the reader they are ready when they can explain the difference between `experiments/` and `demo/`.

- [ ] **Step 2: Write `01-setup.md`**

Content requirements:

- `Goal`: prepare the machine for .NET 10 and MonoGame DesktopGL.
- `What You Will Run`: include:

```bash
dotnet --version
dotnet new list mgdesktopgl
./tools/check-env.sh
```

- `Key Files`: link to `global.json`, `tools/check-env.sh`, `.editorconfig`, `.gitignore`, and `README.md`.
- `Walkthrough`: explain `global.json` pins .NET 10, `check-env.sh` validates macOS/.NET/template readiness, and each MonoGame project has local MGCB tools.
- `Expected Output`: mention `.NET SDK 10.0.107` on this machine and `Environment ready`.
- `Common Problems`: cover missing SDK, missing `mgdesktopgl`, and NuGet/network failures.
- `Checkpoint`: reader can run `./tools/check-env.sh` and gets exit code 0.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "DEMO_SMOKE_EXIT_AFTER_FRAMES|check-env.sh|mgdesktopgl|Environment ready" docs/tutorial/00-intro.md docs/tutorial/01-setup.md
git diff --check
```

Expected: all four terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/00-intro.md docs/tutorial/01-setup.md
git commit -m "docs: add tutorial intro and setup"
```

## Task 3: First Window And Game Loop Chapters

**Files:**
- Modify: `docs/tutorial/02-first-window.md`
- Modify: `docs/tutorial/03-game-loop.md`

- [ ] **Step 1: Write `02-first-window.md`**

Content requirements:

- `Goal`: open the first MonoGame DesktopGL window and understand the template shape.
- `What You Will Run`: include:

```bash
dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj
```

- `Key Files`: link to `experiments/e01-game-loop/Program.cs`, `experiments/e01-game-loop/Game1.cs`, `experiments/e01-game-loop/E01GameLoop.csproj`, and `experiments/e01-game-loop/README.md`.
- `Walkthrough`: explain `Program.cs` creates `Game1`, `Game1` inherits `Game`, `Initialize` configures window/startup state, `LoadContent` loads resources, `Update` changes simulation state, and `Draw` renders.
- `Expected Output`: window opens and stdout prints initialization/frame information.
- `Common Problems`: first run may restore packages, a GUI window needs local desktop access, Escape exits.
- `Checkpoint`: reader can name what `Initialize`, `LoadContent`, `Update`, and `Draw` own.

- [ ] **Step 2: Write `03-game-loop.md`**

Content requirements:

- `Goal`: understand fixed vs variable timestep in MonoGame.
- `What You Will Run`: include:

```bash
env E01_SMOKE_TOGGLE_AFTER_FRAMES=30 E01_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj --no-restore
```

- `Key Files`: link to `experiments/e01-game-loop/GameLoopState.cs`, `experiments/e01-game-loop/LoopSmokeSettings.cs`, and `experiments/e01-game-loop.Tests/Program.cs`.
- `Walkthrough`: explain `IsFixedTimeStep`, frame counting, F1 toggle, and why smoke environment variables are used instead of manual input.
- `Expected Output`: include `Update: timestep mode changed to Variable.` and `Smoke: exit.`
- `Common Problems`: synthetic key events can be unreliable on macOS, so automated smoke uses env vars.
- `Checkpoint`: reader can describe why fixed 60Hz is useful for deterministic learning experiments.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "Initialize|LoadContent|Update|Draw|IsFixedTimeStep|E01_SMOKE" docs/tutorial/02-first-window.md docs/tutorial/03-game-loop.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/02-first-window.md docs/tutorial/03-game-loop.md
git commit -m "docs: add first window and game loop tutorial"
```

## Task 4: Rendering And Input Chapters

**Files:**
- Modify: `docs/tutorial/04-rendering.md`
- Modify: `docs/tutorial/05-input.md`

- [ ] **Step 1: Write `04-rendering.md`**

Content requirements:

- `Goal`: draw sprites and understand batching.
- `What You Will Run`: include:

```bash
env E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj --no-restore
```

- `Key Files`: link to `experiments/e02-2d-rendering/Game1.cs`, `experiments/e02-2d-rendering/SpriteField.cs`, `experiments/e02-2d-rendering/RenderModeState.cs`, and `experiments/e02-2d-rendering/Content/Status.spritefont`.
- `Walkthrough`: explain `SpriteBatch.Begin`, `SpriteBatch.Draw`, `SpriteBatch.DrawString`, runtime texture creation, batched vs unbatched draw, and frame-time text.
- `Expected Output`: mention 1000 sprites, `mode=Batched`, `Update: render mode changed to Unbatched.`, and `Smoke: exit.`
- `Common Problems`: missing SpriteFont content and content build restore.
- `Checkpoint`: reader can explain why grouping draw calls matters.

- [ ] **Step 2: Write `05-input.md`**

Content requirements:

- `Goal`: poll keyboard, mouse, and optional gamepad state.
- `What You Will Run`: include:

```bash
env E03_SMOKE_EXIT_AFTER_FRAMES=120 dotnet run --project experiments/e03-input/E03Input.csproj --no-restore
```

- `Key Files`: link to `experiments/e03-input/InputSnapshot.cs`, `experiments/e03-input/ButtonEdgeState.cs`, `experiments/e03-input/PlayerMotion.cs`, `experiments/e03-input/InputSmokeSettings.cs`, and `experiments/e03-input.Tests/Program.cs`.
- `Walkthrough`: explain polling, previous/current button state, `PressedThisFrame`, mouse target movement, and gamepad-style axis input.
- `Expected Output`: include smoke frame lines for `input=axis` and `input=mouse`.
- `Common Problems`: no gamepad connected is acceptable; the smoke path simulates gamepad-style input.
- `Checkpoint`: reader can distinguish pressed-this-frame from held.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "SpriteBatch|Batched|Unbatched|ButtonEdgeState|PressedThisFrame|E03_SMOKE" docs/tutorial/04-rendering.md docs/tutorial/05-input.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/04-rendering.md docs/tutorial/05-input.md
git commit -m "docs: add rendering and input tutorial"
```

## Task 5: Content Pipeline And Audio Chapters

**Files:**
- Modify: `docs/tutorial/06-content-pipeline.md`
- Modify: `docs/tutorial/07-audio.md`

- [ ] **Step 1: Write `06-content-pipeline.md`**

Content requirements:

- `Goal`: load a PNG, SpriteFont, and WAV through MGCB.
- `What You Will Run`: include:

```bash
env E05_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj --no-restore
```

and the deliberate failure command:

```bash
cd experiments/e05-content-pipeline
dotnet mgcb /quiet /@:docs/broken-content.mgcb
```

- `Key Files`: link to `experiments/e05-content-pipeline/Content/Content.mgcb`, `experiments/e05-content-pipeline/ContentAssets.cs`, `experiments/e05-content-pipeline/docs/broken-content.mgcb`, and `docs/03-content-pipeline.md`.
- `Walkthrough`: explain logical asset names, `.xnb` output, `Content.RootDirectory`, `Content.Load<T>`, and `EnableMGCBItems=false`.
- `Expected Output`: include `LoadContent: texture=Images/pipeline_tile`, `Smoke: played Content-loaded sound.`, and missing source file error for broken MGCB.
- `Common Problems`: relative paths, filename casing, MGCB globbing, missing `.xnb`.
- `Checkpoint`: reader can predict the logical name used by `Content.Load<Texture2D>("Images/pipeline_tile")`.

- [ ] **Step 2: Write `07-audio.md`**

Content requirements:

- `Goal`: use `SoundEffect` for short sounds and `Song`/`MediaPlayer` for looping music.
- `What You Will Run`: include:

```bash
env E04_SMOKE_EXIT_AFTER_FRAMES=150 dotnet run --project experiments/e04-audio/E04Audio.csproj --no-restore
```

- `Key Files`: link to `experiments/e04-audio/Game1.cs`, `experiments/e04-audio/AudioControlState.cs`, `experiments/e04-audio/Content/Content.mgcb`, and `experiments/e04-audio.Tests/Program.cs`.
- `Walkthrough`: explain independent audio actions, `SoundEffect.Play`, `MediaPlayer.Play`, `MediaPlayer.Stop`, and `MediaPlayer.IsRepeating`.
- `Expected Output`: include `Audio: music loop started.`, `Audio: music stopped.`, `Audio: sound effect played at frame 120.`, and `Smoke: exit.`
- `Common Problems`: global `MediaPlayer` state and reused WAV asset for a focused API test.
- `Checkpoint`: reader can choose between `SoundEffect` and `Song` for a given sound.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "Content\\.mgcb|Content\\.Load|EnableMGCBItems|SoundEffect|MediaPlayer|E04_SMOKE" docs/tutorial/06-content-pipeline.md docs/tutorial/07-audio.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/06-content-pipeline.md docs/tutorial/07-audio.md
git commit -m "docs: add content pipeline and audio tutorial"
```

## Task 6: Camera, Collision, Animation, And Publishing Chapters

**Files:**
- Modify: `docs/tutorial/08-camera-collision-animation.md`
- Modify: `docs/tutorial/09-publishing.md`

- [ ] **Step 1: Write `08-camera-collision-animation.md`**

Content requirements:

- `Goal`: add camera transforms, collision checks, and simple animation state.
- `What You Will Run`: include:

```bash
env E06_SMOKE_EXIT_AFTER_FRAMES=180 dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj --no-restore
env E07_SMOKE_EXIT_AFTER_FRAMES=170 dotnet run --project experiments/e07-animation/E07Animation.csproj --no-restore
```

- `Key Files`: link to `experiments/e06-camera-and-collision/Camera2D.cs`, `experiments/e06-camera-and-collision/Collision2D.cs`, `experiments/e06-camera-and-collision/CollisionScene.cs`, `experiments/e07-animation/FrameAnimation.cs`, `experiments/e07-animation/CharacterStateMachine.cs`, and `experiments/e07-animation/AnimationController.cs`.
- `Walkthrough`: explain world vs screen coordinates, `SpriteBatch` transform matrix, AABB overlap, circle distance check, frame accumulation, and idle/walk/jump transitions.
- `Expected Output`: include `Collision: aabb=True, circle=True.`, `Transition: Idle -> Walk`, `Transition: Idle -> Jump`, and `Smoke: exit.`
- `Common Problems`: collision debug drawing is user code, animation graphs are not built in, and smoke mode replaces manual input.
- `Checkpoint`: reader can explain which parts MonoGame provides and which parts they wrote.

- [ ] **Step 2: Write `09-publishing.md`**

Content requirements:

- `Goal`: publish a self-contained macOS DesktopGL build.
- `What You Will Run`: include:

```bash
dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false
env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing
```

- `Key Files`: link to `experiments/e10-publishing/E10Publishing.csproj`, `experiments/e10-publishing/README.md`, and `docs/04-platforms-and-publishing.md`.
- `Walkthrough`: explain RID, self-contained output, minimal PATH smoke, native dependencies, and why this is not a signed `.app`.
- `Expected Output`: include `Smoke: rendered 30 update frames.` and `Smoke: exit.`
- `Common Problems`: publish directory is large, runtime files are expected, and signing/notarization are out of scope.
- `Checkpoint`: reader can find the published executable and run it without `dotnet` on `PATH`.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "Camera2D|AABB|Circle|FrameAnimation|dotnet publish|self-contained|E10_SMOKE" docs/tutorial/08-camera-collision-animation.md docs/tutorial/09-publishing.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/08-camera-collision-animation.md docs/tutorial/09-publishing.md
git commit -m "docs: add systems and publishing tutorial"
```

## Task 7: Integrated Demo And Troubleshooting Appendix

**Files:**
- Modify: `docs/tutorial/10-integrated-demo.md`
- Modify: `docs/tutorial/appendix-troubleshooting.md`

- [ ] **Step 1: Write `10-integrated-demo.md`**

Content requirements:

- `Goal`: connect previous concepts into the playable collector demo.
- `What You Will Run`: include:

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

- `Key Files`: link to `demo/integrated-demo/DemoState.cs`, `demo/integrated-demo/DemoTypes.cs`, `demo/integrated-demo/Game1.cs`, `demo/integrated-demo/Content/Content.mgcb`, and `demo/integrated-demo.Tests/Program.cs`.
- `Walkthrough`: explain start/play/win/loss phases, deterministic model, pickups, hazards, restart, content-loaded font/audio, and why smoke uses a fixed simulation step with vsync disabled.
- `Expected Output`: include `Phase: started.`, `Collect: score=3/3.`, `Phase: won.`, `Phase: restarted.`, and `Smoke: exit.`
- `Common Problems`: GUI smoke can hang under vsync/fixed timestep on macOS; the demo disables vsync and uses fixed smoke simulation time.
- `Checkpoint`: reader can run the demo manually or use smoke to prove the full loop.

- [ ] **Step 2: Write `appendix-troubleshooting.md`**

Content requirements:

- `Goal`: diagnose common failures from this repo.
- `What You Will Run`: include:

```bash
./tools/check-env.sh
dotnet build GameDemo.sln --no-restore -m:1
git diff --check
```

- `Key Files`: link to `tools/check-env.sh`, `global.json`, `.gitignore`, `docs/03-content-pipeline.md`, `docs/04-platforms-and-publishing.md`, and `docs/reports/phase1-closeout.md`.
- `Walkthrough`: create one subsection per failure mode from the spec: SDK mismatch, missing template, NuGet/network restore, MGCB restore, `.mgcb` globbing, missing `.xnb`, SpriteFont/font availability, macOS DesktopGL smoke quirks, publish layout confusion.
- `Expected Output`: include `Environment ready`, `0 warnings`, `0 errors`, and no `git diff --check` output.
- `Common Problems`: explain when to rerun restore without `--no-restore`.
- `Checkpoint`: reader can pick the right diagnostic command for a failure.

- [ ] **Step 3: Verify chapters**

Run:

```bash
rg -n "DemoState|Phase: won|Phase: restarted|Environment ready|missing \\.xnb|mgcb|Publish" docs/tutorial/10-integrated-demo.md docs/tutorial/appendix-troubleshooting.md
git diff --check
```

Expected: all key terms appear and diff check exits 0.

- [ ] **Step 4: Commit**

```bash
git add docs/tutorial/10-integrated-demo.md docs/tutorial/appendix-troubleshooting.md
git commit -m "docs: add integrated demo tutorial and troubleshooting"
```

## Task 8: Final Verification And Push

**Files:**
- Modify: documentation only if final verification finds a broken link, missing section, or inaccurate command.

- [ ] **Step 1: Verify all tutorial files exist**

Run:

```bash
find docs/tutorial -maxdepth 1 -type f | sort
```

Expected list includes exactly:

```text
docs/tutorial/00-intro.md
docs/tutorial/01-setup.md
docs/tutorial/02-first-window.md
docs/tutorial/03-game-loop.md
docs/tutorial/04-rendering.md
docs/tutorial/05-input.md
docs/tutorial/06-content-pipeline.md
docs/tutorial/07-audio.md
docs/tutorial/08-camera-collision-animation.md
docs/tutorial/09-publishing.md
docs/tutorial/10-integrated-demo.md
docs/tutorial/appendix-troubleshooting.md
```

- [ ] **Step 2: Verify required sections**

Run:

```bash
for section in "Goal" "What You Will Run" "Key Files" "Walkthrough" "Expected Output" "Common Problems" "Checkpoint"; do
  count=$(rg -l "^## ${section}$" docs/tutorial | wc -l | tr -d ' ')
  echo "${section}: ${count}"
done
```

Expected:

```text
Goal: 12
What You Will Run: 12
Key Files: 12
Walkthrough: 12
Expected Output: 12
Common Problems: 12
Checkpoint: 12
```

- [ ] **Step 3: Verify no empty instructional sections**

Run:

```bash
rg -n "## (Goal|What You Will Run|Key Files|Walkthrough|Expected Output|Common Problems|Checkpoint)\\n\\n##" docs/tutorial
```

Expected: no matches.

- [ ] **Step 4: Verify build**

Run:

```bash
dotnet build GameDemo.sln --no-restore -m:1
```

Expected: exits 0 with:

```text
0 warnings
0 errors
```

- [ ] **Step 5: Verify integrated demo smoke**

Run:

```bash
env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore
```

Expected output includes:

```text
Phase: won.
Phase: restarted.
Smoke: exit.
```

- [ ] **Step 6: Final diff checks**

Run:

```bash
git diff --check
git status --short --untracked-files=all
```

Expected: diff check exits 0. `git status` shows only intended tutorial documentation changes before the final commit.

- [ ] **Step 7: Commit final fixes if needed**

If final verification required documentation edits, commit them:

```bash
git add README.md docs/tutorial
git commit -m "docs: finalize monogame tutorial"
```

If no final edits were required, do not create an empty commit.

- [ ] **Step 8: Push**

Run:

```bash
git push origin main
```

Expected: `main` updates on `git@github.com:HelloiOS2014/MonoGameStudy.git`.

## Self-Review Checklist

- Spec coverage: Tasks 1-7 create every chapter required by `docs/superpowers/specs/2026-05-02-monogame-tutorial-design.md`; Task 8 verifies README, chapter structure, build, and smoke.
- Completion scan: The plan uses concrete filenames, commands, headings, and required content; implementation must not add empty chapter sections.
- Type consistency: Chapter filenames and headings match the spec exactly.
