# First Window

Previous: [Setup](01-setup.md) | Next: [Game Loop](03-game-loop.md)

## Goal

Open the first MonoGame DesktopGL window and understand the shape of a template-based MonoGame project.

## What You Will Run

```bash
dotnet run --project experiments/e01-game-loop/E01GameLoop.csproj
```

## Key Files

- [experiments/e01-game-loop/Program.cs](../../experiments/e01-game-loop/Program.cs) - creates `Game1` and starts `Game.Run()`.
- [experiments/e01-game-loop/Game1.cs](../../experiments/e01-game-loop/Game1.cs) - owns the MonoGame lifecycle methods.
- [experiments/e01-game-loop/E01GameLoop.csproj](../../experiments/e01-game-loop/E01GameLoop.csproj) - DesktopGL project file.
- [experiments/e01-game-loop/README.md](../../experiments/e01-game-loop/README.md) - experiment-specific run notes.

## Walkthrough

`Program.cs` is deliberately tiny:

```csharp
using var game = new E01GameLoop.Game1();
game.Run();
```

`Game1` inherits `Microsoft.Xna.Framework.Game`. MonoGame calls its lifecycle methods in a predictable order:

- `Initialize` configures startup state: target 60 Hz timing, window size, window title, and first stdout message.
- `LoadContent` loads resources after the graphics device exists. This first experiment logs that no content is loaded yet.
- `Update` changes simulation state. Here it polls keyboard/gamepad input, toggles timestep mode, and handles exit.
- `Draw` renders the frame. Here it clears the window to `CornflowerBlue` and prints frame information once per second.

The important mental model is that you do not write the outer while loop. MonoGame owns it after `Game.Run()`.

## Expected Output

A 960x540 window opens with a blue background and this title:

```text
E01 Game Loop - Fixed 60 Hz
```

Stdout starts with initialization information like:

```text
Initialize: fixed timestep at 60 Hz. Press F1 to toggle fixed/variable timestep. Press Escape to exit.
LoadContent: no content loaded for e01.
```

Every second, it prints frame information from `Draw`.

## Common Problems

- The first run may restore NuGet packages and project-local tools before the window appears.
- A normal run opens a GUI window and needs local desktop access.
- Press `Escape` to exit the window.
- If the command fails before launching, run `./tools/check-env.sh` from the repo root.

## Checkpoint

You are ready when you can name what `Initialize`, `LoadContent`, `Update`, and `Draw` each own.
