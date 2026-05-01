# Integrated Demo

Genre: tiny 2D collector. This keeps the Week 3 demo honest: it uses the parts already tested in isolation, including the game loop, 2D rendering, input polling, content-loaded text/audio, collision checks, simple animation/presentation, and publishable DesktopGL project shape. It does not pull in optional 3D or shader work because those were deferred.

## How to run

```bash
dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

Automated smoke:

```bash
DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj
```

## What you should see

The first screen prompts for Enter. In play, collect the yellow pickups, avoid red hazards, and reach the win screen after three pickups. Hitting a red hazard shows the loss screen. From win or loss, press Enter or `R` to restart.

Controls:

- `Enter` starts and restarts.
- `WASD` or arrow keys move.
- `R` restarts after win or loss.
- `Escape` quits.

## Known limitations

The demo uses simple runtime shapes and one reused beep sound. There is no camera, menu stack, save data, level loader, controller UI, packaging polish, or asset pipeline beyond one font and one sound. That is deliberate: Phase 1 is evaluating MonoGame friction, not making a polished game.
