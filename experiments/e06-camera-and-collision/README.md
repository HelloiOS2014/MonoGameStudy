# E06 Camera And Collision

## How to run

```bash
dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj
```

Automated smoke:

```bash
E06_SMOKE_EXIT_AFTER_FRAMES=180 dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj
```

## What you should see

A DesktopGL window opens on a simple world grid. The camera can pan and zoom. A static AABB and static circle are outlined in blue; the moving AABB and moving circle are green when clear and red when colliding.

Controls:

- Arrow keys pan the camera.
- `Q` / `E` zoom out / in.
- `WASD` moves the dynamic collision shapes.
- `Escape` exits.

The smoke run pans the camera, zooms in, then moves both dynamic shapes into their static targets until both collision outlines turn red.

## What was learned

A small camera abstraction is enough for basic 2D translation and zoom: keep world-space positions in game logic and pass a view matrix into `SpriteBatch.Begin`. Collision logic should stay separate from drawing; AABB and circle tests are deterministic and easy to cover with console tests before wiring them into MonoGame rendering.
