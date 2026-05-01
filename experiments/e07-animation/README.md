# E07 Animation

## How to run

```bash
dotnet run --project experiments/e07-animation/E07Animation.csproj
```

Automated smoke:

```bash
E07_SMOKE_EXIT_AFTER_FRAMES=170 dotnet run --project experiments/e07-animation/E07Animation.csproj
```

## What you should see

A DesktopGL window opens with a simple animated character on a ground line. The character changes frame while idle, walking, and jumping. State changes are logged to stdout.

Controls:

- `A` / `D` or left / right arrows walk.
- `Space` jumps.
- `Escape` exits.

The smoke run idles briefly, walks right, jumps, lands back into walk, and exits automatically.

## What was learned

Frame animation is just accumulated time plus a current frame index. The state machine stays clearer when it emits explicit transitions and leaves rendering to a separate animation controller. Logging transitions makes it easy to verify whether input caused idle, walk, and jump changes at the expected frame.
