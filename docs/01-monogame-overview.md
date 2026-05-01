# MonoGame Overview

MonoGame is a code-first game framework, not an editor-driven engine. The project owns the game architecture, tools, scene structure, UI conventions, and asset workflow. MonoGame provides the runtime loop, graphics/audio/input APIs, content loading, and platform backends.

## Project Shape

The DesktopGL template creates a small C# app with:

- `Program.cs` constructing `Game1` and calling `Run()`.
- `Game1.cs` deriving from `Microsoft.Xna.Framework.Game`.
- `Content/Content.mgcb` describing assets that MGCB compiles into `.xnb` files.
- A local `.config/dotnet-tools.json` with MGCB command-line tools.

The template currently targets `net9.0`; this repo retargets experiments to `net10.0` to match the repo SDK pin.

## Lifecycle

`Initialize()` runs once before content is loaded. It is the right place for window configuration, initial state, and non-asset setup.

`LoadContent()` runs after the graphics device is ready. It is the right place to create `SpriteBatch`, load `Texture2D`, `SpriteFont`, `SoundEffect`, and allocate GPU resources.

`Update(GameTime)` runs once per simulation tick. Input is polled here through `Keyboard.GetState()`, `Mouse.GetState()`, and `GamePad.GetState()`. This is where movement, toggles, state machines, and game rules belong.

`Draw(GameTime)` runs when MonoGame renders a frame. Rendering code belongs here: clear the backbuffer, begin/end `SpriteBatch`, draw textures/text, and avoid changing gameplay state except for instrumentation counters.

For these DesktopGL experiments, lifecycle methods run on the main game thread. Do not assume background loading or editor-style event dispatch unless it is explicitly added.

## Findings So Far

- `e01-game-loop` showed `IsFixedTimeStep` can be toggled at runtime and that stdout logging is useful for loop instrumentation.
- `e02-2d-rendering` showed `SpriteBatch.Begin`/`End` placement is a core performance decision; batching 1000 sprites stayed at 60fps on this machine.
- `e03-input` showed input is polling-based and edge detection must be implemented by comparing previous and current state.
- `e05-content-pipeline` showed MGCB is usable from the CLI, but `.mgcb` globbing and relative paths need care.
