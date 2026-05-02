# Camera, Collision, And Animation

Previous: [Audio](07-audio.md) | Next: [Publishing](09-publishing.md)

## Goal

Add camera transforms, collision checks, and simple animation state while keeping game logic separate from MonoGame drawing.

## What You Will Run

```bash
env E06_SMOKE_EXIT_AFTER_FRAMES=180 dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj --no-restore
env E07_SMOKE_EXIT_AFTER_FRAMES=170 dotnet run --project experiments/e07-animation/E07Animation.csproj --no-restore
```

## Key Files

- [experiments/e06-camera-and-collision/Camera2D.cs](../../experiments/e06-camera-and-collision/Camera2D.cs) - 2D camera position, zoom, and transform matrix.
- [experiments/e06-camera-and-collision/Collision2D.cs](../../experiments/e06-camera-and-collision/Collision2D.cs) - AABB and circle overlap tests.
- [experiments/e06-camera-and-collision/CollisionScene.cs](../../experiments/e06-camera-and-collision/CollisionScene.cs) - deterministic collision scene state.
- [experiments/e07-animation/FrameAnimation.cs](../../experiments/e07-animation/FrameAnimation.cs) - frame timer and current-frame index.
- [experiments/e07-animation/CharacterStateMachine.cs](../../experiments/e07-animation/CharacterStateMachine.cs) - idle/walk/jump transitions.
- [experiments/e07-animation/AnimationController.cs](../../experiments/e07-animation/AnimationController.cs) - maps character state to animation frames.

## Walkthrough

MonoGame gives you drawing primitives and matrices; it does not give you a built-in 2D camera, collision system, or animation graph.

`Camera2D` keeps world-space `Position` and `Zoom`. Game logic stores shapes in world coordinates. Drawing passes the camera matrix to `SpriteBatch.Begin`:

```csharp
spriteBatch.Begin(..., transformMatrix: view);
```

That means collision and movement can stay in world coordinates while rendering shifts the view.

`Collision2D` contains two deterministic tests:

- AABB overlap compares rectangle edges.
- Circle overlap compares squared distance between centers against squared radius sum.

The debug outlines are user code built from a 1x1 runtime texture and line drawing. MonoGame is not providing a collision-debug renderer here.

Animation is also regular game code. `FrameAnimation` accumulates elapsed time and advances a frame index. `CharacterStateMachine` emits explicit transitions between `Idle`, `Walk`, and `Jump`. `AnimationController` resets or advances the animation for the active state.

## Expected Output

The camera/collision smoke pans, zooms, moves shapes into collision, then exits. Stdout includes:

```text
Collision: aabb=True, circle=True.
Smoke: exit.
```

The animation smoke logs state changes:

```text
Transition: Idle -> Walk
Transition: Idle -> Jump
Smoke: exit.
```

## Common Problems

- Collision debug drawing is user code; MonoGame does not provide these outlines automatically.
- Animation graphs are not built in. State transitions, frame timing, and reset behavior are your responsibility.
- Smoke mode replaces manual input so camera, collision, and animation can be checked without key events.

## Checkpoint

You are ready when you can explain which parts MonoGame provides and which parts this repo wrote itself.
