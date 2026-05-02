# Godot Parity Demo Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a separate Godot 4.x version of the MonoGame integrated collector demo and compare development speed, tool friction, and packaging effort.

**Architecture:** Create a new repository outside `game_demo` so the comparison is clean. Keep the game tiny: one scene, one player, three pickups, two hazards, start/play/win/loss phases, restart, sound, and a short comparison report. Use a deterministic model script with a headless test script before wiring the scene.

**Tech Stack:** Godot 4.x, GDScript, macOS desktop export or local run, no third-party plugins.

---

## File Structure

New repository path:

```text
/Users/panghu/code/rsearch/godot_collector_comparison/
```

Files to create:

- `project.godot` — minimal Godot project metadata.
- `scenes/Main.tscn` — single playable scene.
- `scripts/game_state.gd` — deterministic game model: phase, player, pickups, hazards, score.
- `scripts/main.gd` — scene wiring, input, drawing nodes, labels, audio.
- `tests/test_game_state.gd` — headless assertions for start, collect, win, loss, restart.
- `README.md` — how to run, controls, what to observe.
- `docs/comparison-report.md` — direct comparison with the MonoGame integrated demo.

## Task 1: Tooling Check

**Files:**
- Create: none
- Modify: none
- Test: Godot CLI availability

- [ ] **Step 1: Check whether Godot CLI exists**

Run:

```bash
which godot
godot --version
```

Expected if installed: a path and a Godot 4.x version string.

- [ ] **Step 2: Install Godot if missing**

Run on macOS:

```bash
brew install --cask godot
```

Expected: Homebrew installs a Godot app. If the `godot` CLI is still missing, use the app binary path in subsequent commands:

```bash
/Applications/Godot.app/Contents/MacOS/Godot --version
```

- [ ] **Step 3: Commit nothing**

This task only prepares the machine. Do not edit `game_demo`.

## Task 2: Create The Project Skeleton

**Files:**
- Create: `/Users/panghu/code/rsearch/godot_collector_comparison/project.godot`
- Create: `/Users/panghu/code/rsearch/godot_collector_comparison/scenes/Main.tscn`
- Create: `/Users/panghu/code/rsearch/godot_collector_comparison/scripts/main.gd`
- Create: `/Users/panghu/code/rsearch/godot_collector_comparison/README.md`

- [ ] **Step 1: Create directories**

Run:

```bash
mkdir -p /Users/panghu/code/rsearch/godot_collector_comparison/scenes
mkdir -p /Users/panghu/code/rsearch/godot_collector_comparison/scripts
mkdir -p /Users/panghu/code/rsearch/godot_collector_comparison/tests
mkdir -p /Users/panghu/code/rsearch/godot_collector_comparison/docs
cd /Users/panghu/code/rsearch/godot_collector_comparison
git init
```

- [ ] **Step 2: Create `project.godot`**

Write:

```ini
config_version=5

[application]
config/name="Godot Collector Comparison"
run/main_scene="res://scenes/Main.tscn"
config/features=PackedStringArray("4.0")

[display]
window/size/viewport_width=960
window/size/viewport_height=540
```

- [ ] **Step 3: Create starter scene and script**

`scenes/Main.tscn`:

```ini
[gd_scene load_steps=2 format=3]

[ext_resource type="Script" path="res://scripts/main.gd" id="1"]

[node name="Main" type="Node2D"]
script = ExtResource("1")
```

`scripts/main.gd`:

```gdscript
extends Node2D

func _ready() -> void:
    print("Godot collector comparison started.")
```

- [ ] **Step 4: Run the project**

Run:

```bash
godot --path /Users/panghu/code/rsearch/godot_collector_comparison --quit-after 1
```

Expected: stdout includes `Godot collector comparison started.`

- [ ] **Step 5: Commit**

```bash
git add project.godot scenes/Main.tscn scripts/main.gd
git commit -m "chore: bootstrap godot collector comparison"
```

## Task 3: Add The Deterministic Game Model

**Files:**
- Create: `scripts/game_state.gd`
- Create: `tests/test_game_state.gd`

- [ ] **Step 1: Write the failing headless test**

`tests/test_game_state.gd`:

```gdscript
extends SceneTree

const GameState = preload("res://scripts/game_state.gd")

func _assert(condition: bool, message: String) -> void:
    if not condition:
        push_error(message)
        quit(1)

func _init() -> void:
    var game := GameState.new()
    _assert(game.phase == "start", "game starts on start phase")

    var result := game.update(Vector2.ZERO, true, false, 1.0 / 60.0)
    _assert(result["started"], "start press begins game")
    _assert(game.phase == "playing", "phase becomes playing")

    for i in range(130):
        result = game.update(Vector2.RIGHT, false, false, 1.0 / 60.0)
        if game.phase != "playing":
            break

    _assert(result["won"], "moving right collects all pickups and wins")
    _assert(game.phase == "won", "phase becomes won")
    _assert(game.score == game.target_score, "score reaches target")

    result = game.update(Vector2.ZERO, false, true, 1.0 / 60.0)
    _assert(result["restarted"], "restart begins a new run")
    _assert(game.phase == "playing", "restart returns to playing")
    _assert(game.score == 0, "restart clears score")

    var loss_game := GameState.new()
    loss_game.update(Vector2.ZERO, true, false, 1.0 / 60.0)
    for i in range(80):
        result = loss_game.update(Vector2.DOWN, false, false, 1.0 / 60.0)
        if loss_game.phase != "playing":
            break

    _assert(result["lost"], "moving down into hazard loses")
    _assert(loss_game.phase == "lost", "phase becomes lost")
    print("Godot game_state tests passed.")
    quit(0)
```

- [ ] **Step 2: Run test to verify it fails**

Run:

```bash
godot --headless --path /Users/panghu/code/rsearch/godot_collector_comparison -s tests/test_game_state.gd
```

Expected: FAIL because `scripts/game_state.gd` does not exist.

- [ ] **Step 3: Implement `scripts/game_state.gd`**

```gdscript
class_name GameState
extends RefCounted

const PLAYER_RADIUS := 18.0
const PLAYER_SPEED := 160.0

var target_score := 3
var phase := "start"
var player := Vector2(90, 120)
var score := 0
var pickups := []
var hazards := []

func _init() -> void:
    _reset_layout()

func update(move_axis: Vector2, start_pressed: bool, restart_pressed: bool, delta: float) -> Dictionary:
    var result := {
        "started": false,
        "restarted": false,
        "collected": false,
        "won": false,
        "lost": false,
    }

    if phase == "start":
        if start_pressed:
            _reset_run()
            result["started"] = true
        return result

    if phase == "won" or phase == "lost":
        if restart_pressed:
            _reset_run()
            result["restarted"] = true
        return result

    if move_axis.length_squared() > 1.0:
        move_axis = move_axis.normalized()

    player += move_axis * PLAYER_SPEED * delta
    player.x = clamp(player.x, 40.0, 880.0)
    player.y = clamp(player.y, 70.0, 430.0)

    for pickup in pickups:
        if pickup["collected"]:
            continue
        if _circles_overlap(player, PLAYER_RADIUS, pickup["center"], pickup["radius"]):
            pickup["collected"] = true
            score += 1
            result["collected"] = true

    if score >= target_score:
        phase = "won"
        result["won"] = true
        return result

    for hazard in hazards:
        if _circles_overlap(player, PLAYER_RADIUS, hazard["center"], hazard["radius"]):
            phase = "lost"
            result["lost"] = true
            return result

    return result

func _reset_run() -> void:
    phase = "playing"
    player = Vector2(90, 120)
    score = 0
    _reset_layout()

func _reset_layout() -> void:
    pickups = [
        {"center": Vector2(170, 120), "radius": 16.0, "collected": false},
        {"center": Vector2(250, 120), "radius": 16.0, "collected": false},
        {"center": Vector2(330, 120), "radius": 16.0, "collected": false},
    ]
    hazards = [
        {"center": Vector2(90, 240), "radius": 26.0},
        {"center": Vector2(430, 120), "radius": 30.0},
    ]

func _circles_overlap(a: Vector2, ar: float, b: Vector2, br: float) -> bool:
    var radii := ar + br
    return a.distance_squared_to(b) <= radii * radii
```

- [ ] **Step 4: Run test to verify it passes**

Run:

```bash
godot --headless --path /Users/panghu/code/rsearch/godot_collector_comparison -s tests/test_game_state.gd
```

Expected: PASS with `Godot game_state tests passed.`

- [ ] **Step 5: Commit**

```bash
git add scripts/game_state.gd tests/test_game_state.gd
git commit -m "feat: add deterministic collector model"
```

## Task 4: Wire The Playable Scene

**Files:**
- Modify: `scripts/main.gd`
- Modify: `README.md`

- [ ] **Step 1: Replace `scripts/main.gd`**

```gdscript
extends Node2D

const GameState = preload("res://scripts/game_state.gd")

var game := GameState.new()

func _ready() -> void:
    queue_redraw()

func _process(delta: float) -> void:
    var axis := Vector2.ZERO
    axis.x = Input.get_action_strength("ui_right") - Input.get_action_strength("ui_left")
    axis.y = Input.get_action_strength("ui_down") - Input.get_action_strength("ui_up")

    var result := game.update(axis, Input.is_action_just_pressed("ui_accept"), Input.is_action_just_pressed("ui_accept"), delta)
    if result["started"]:
        print("Phase: started.")
    if result["restarted"]:
        print("Phase: restarted.")
    if result["collected"]:
        print("Collect: score=%s/%s." % [game.score, game.target_score])
    if result["won"]:
        print("Phase: won.")
    if result["lost"]:
        print("Phase: lost.")

    if Input.is_action_just_pressed("ui_cancel"):
        get_tree().quit()

    queue_redraw()

func _draw() -> void:
    draw_rect(Rect2(40, 70, 840, 360), Color(0.13, 0.16, 0.20))
    for hazard in game.hazards:
        draw_circle(hazard["center"], hazard["radius"], Color(0.90, 0.30, 0.28))
    for pickup in game.pickups:
        if not pickup["collected"]:
            draw_circle(pickup["center"], pickup["radius"], Color(0.98, 0.82, 0.28))
    draw_circle(game.player, game.PLAYER_RADIUS, Color(0.30, 0.82, 0.58))

    var title := ""
    if game.phase == "start":
        title = "GODOT MICRO COLLECTOR - Press Enter"
    elif game.phase == "won":
        title = "YOU WIN - Press Enter"
    elif game.phase == "lost":
        title = "YOU LOST - Press Enter"
    else:
        title = "Score %s/%s - Collect yellow, avoid red" % [game.score, game.target_score]

    draw_string(ThemeDB.fallback_font, Vector2(52, 42), title, HORIZONTAL_ALIGNMENT_LEFT, -1, 18, Color.WHITE)
    draw_string(ThemeDB.fallback_font, Vector2(52, 470), "Move: arrows/WASD if mapped   Quit: Esc", HORIZONTAL_ALIGNMENT_LEFT, -1, 16, Color(0.75, 0.82, 0.90))
```

- [ ] **Step 2: Add input mappings if WASD is desired**

Open Godot editor once and map `W/A/S/D` to `ui_up/ui_left/ui_down/ui_right`, or keep arrow keys only for the parity pass. Record which choice was used in `README.md`.

- [ ] **Step 3: Run the scene**

Run:

```bash
godot --path /Users/panghu/code/rsearch/godot_collector_comparison
```

Expected: window opens, Enter starts, arrow keys move, yellow pickups increase score, red hazard loses, Enter restarts.

- [ ] **Step 4: Write `README.md`**

```markdown
# Godot Collector Comparison

Small Godot parity demo for comparing against `/Users/panghu/code/rsearch/game_demo/demo/integrated-demo`.

## How to run

```bash
godot --path /Users/panghu/code/rsearch/godot_collector_comparison
```

## What you should see

Press Enter to start. Collect three yellow pickups, avoid red hazards, and reach the win screen. Press Enter after win/loss to restart.

## What was learned

This first commit proves the scene opens. The final comparison notes belong in `docs/comparison-report.md` after the playable scene is implemented and verified.
```

- [ ] **Step 5: Commit**

```bash
git add scripts/main.gd README.md
git commit -m "feat: add playable godot collector scene"
```

## Task 5: Write The Comparison Report

**Files:**
- Create: `docs/comparison-report.md`

- [ ] **Step 1: Create report**

```markdown
# Godot vs MonoGame Collector Comparison

## Scope

Both demos implement start, play, win/loss, restart, player movement, pickups, hazards, and a tiny visual presentation.

## Time And Friction

| Area | MonoGame evidence | Godot evidence |
| ---- | ----------------- | -------------- |
| Project setup | `game_demo` needed .NET SDK, templates, solution setup, MGCB tools | Write elapsed setup time in minutes, exact Godot launch command, and one concrete setup friction point or `none`. |
| Rendering | MonoGame required explicit SpriteBatch/runtime shapes | Write whether drawing used `_draw`, scene nodes, or both; include one concrete rendering friction point or `none`. |
| Input | MonoGame used polling and edge helpers | Write the exact Godot input actions used and whether WASD required editor mapping. |
| Content/audio | MonoGame used MGCB for font/audio | Write whether Godot needed an import step for the comparison demo and whether audio was included or intentionally omitted. |
| Packaging | MonoGame self-contained publish created a runnable directory | Write the exact Godot run/export command attempted and the result. |

## Decision

Choose Godot for quick playable prototypes if the editor and built-in workflow reduce effort. Choose MonoGame when the goal is C# code-first control and learning engine-shaped systems.
```

- [ ] **Step 2: Verify every Godot evidence cell is concrete**

Run:

```bash
rg "Write elapsed|Write whether|Write the exact" docs/comparison-report.md
```

Expected after editing: no matches.

- [ ] **Step 3: Commit**

```bash
git add docs/comparison-report.md
git commit -m "docs: compare godot and monogame collector demos"
```

## Task 6: Final Verification

**Files:**
- Modify: none unless verification reveals documentation gaps

- [ ] **Step 1: Run model test**

```bash
godot --headless --path /Users/panghu/code/rsearch/godot_collector_comparison -s tests/test_game_state.gd
```

Expected: `Godot game_state tests passed.`

- [ ] **Step 2: Run the game manually or with a short recording**

```bash
godot --path /Users/panghu/code/rsearch/godot_collector_comparison
```

Expected: one complete run reaches win or loss and restarts.

- [ ] **Step 3: Confirm clean git state**

```bash
git status --short --untracked-files=all
```

Expected: no output.

## Stop Rule

Stop after this parity demo. Do not expand it into a full game. The output is a decision: Godot next, MonoGame next, or pause game work.
