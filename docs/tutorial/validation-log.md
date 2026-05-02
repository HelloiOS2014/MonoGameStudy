# Tutorial Validation Log

## 2026-05-02

Machine:

- macOS DesktopGL target
- Architecture reported by `check-env.sh`: `x86_64`
- .NET SDK: `10.0.107`
- MonoGame template: `mgdesktopgl`

Commands validated:

- `dotnet --version`
- `dotnet new list mgdesktopgl`
- `./tools/check-env.sh`
- `dotnet build GameDemo.sln --no-restore -m:1`
- `env E01_SMOKE_TOGGLE_AFTER_FRAMES=30 E01_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj --no-restore`
- `env E02_SMOKE_TOGGLE_AFTER_FRAMES=180 E02_SMOKE_EXIT_AFTER_FRAMES=340 dotnet run --project experiments/e02-2d-rendering/E02Rendering.csproj --no-restore`
- `env E03_SMOKE_EXIT_AFTER_FRAMES=120 dotnet run --project experiments/e03-input/E03Input.csproj --no-restore`
- `env E04_SMOKE_EXIT_AFTER_FRAMES=150 dotnet run --project experiments/e04-audio/E04Audio.csproj --no-restore`
- `env E05_SMOKE_EXIT_AFTER_FRAMES=90 dotnet run --project experiments/e05-content-pipeline/E05ContentPipeline.csproj --no-restore`
- `dotnet mgcb /quiet /@:docs/broken-content.mgcb` from `experiments/e05-content-pipeline`
- `env E06_SMOKE_EXIT_AFTER_FRAMES=180 dotnet run --project experiments/e06-camera-and-collision/E06CameraAndCollision.csproj --no-restore`
- `env E07_SMOKE_EXIT_AFTER_FRAMES=170 dotnet run --project experiments/e07-animation/E07Animation.csproj --no-restore`
- `dotnet publish experiments/e10-publishing/E10Publishing.csproj -c Release -r osx-x64 --self-contained true -p:PublishReadyToRun=false`
- `env -i HOME="$HOME" E10_SMOKE_EXIT_AFTER_FRAMES=60 PATH="/usr/bin:/bin" experiments/e10-publishing/bin/Release/net10.0/osx-x64/publish/E10Publishing`
- `env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore`

Evidence:

- Environment check ended with `Environment ready.`
- Solution build ended with `0 个警告` and `0 个错误`.
- E01 smoke printed `Update: timestep mode changed to Variable.` and `Smoke: exit.`
- E02 smoke printed `mode=Batched`, `Update: render mode changed to Unbatched.`, and `Smoke: exit.`
- E03 smoke printed frame lines for `input=axis`, `input=mouse`, and `Smoke: exit.`
- E04 smoke printed `Audio: music loop started.`, `Audio: music stopped.`, `Audio: sound effect played at frame 120.`, and `Smoke: exit.`
- E05 smoke printed `LoadContent: texture=Images/pipeline_tile`, `Smoke: played Content-loaded sound.`, and `Smoke: exit.`
- The deliberate MGCB failure exited non-zero and mentioned `missing_texture.png`.
- E06 smoke printed `Collision: aabb=True, circle=True.` and `Smoke: exit.`
- E07 smoke printed `Transition: Idle -> Walk`, `Transition: Idle -> Jump`, and `Smoke: exit.`
- E10 published smoke printed `Smoke: rendered 30 update frames.` and `Smoke: exit.`
- Integrated demo smoke printed `Phase: won.`, `Phase: restarted.`, and `Smoke: exit.`

The consolidated command is:

```bash
./tools/check-tutorial.sh
```

Expected final line:

```text
Tutorial dry run passed.
```
