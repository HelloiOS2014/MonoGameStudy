# Engine Comparison

Initial comparison set: Unity, Godot, raylib, and hand-rolled engines on SDL or similar. This is a first-pass note based only on Week 1 evidence.

## MonoGame

Strengths:

- Very direct C# control over the loop, rendering, input, and content loading.
- Small template surface; little hidden editor state.
- `SpriteBatch` and DesktopGL are enough to build focused 2D experiments quickly.
- Good fit when the goal is learning how a game loop and rendering pipeline actually work.

Costs:

- No built-in editor workflow for scenes, inspectors, animation graphs, or UI layout.
- The content pipeline is explicit and build-time oriented; bad `.mgcb` paths fail the build.
- Common engine conveniences become user code: input actions, camera systems, collision, animation state, UI, and tooling.

## Unity

Unity provides a mature editor, asset import pipeline, scene system, inspector workflow, and broad platform support. It is heavier than MonoGame, but much more complete out of the box. For production workflows with non-programmer iteration, Unity starts far ahead.

MonoGame is better for code-first learning and small custom systems. Unity is better when editor productivity and ecosystem breadth matter more than understanding every layer.

## Godot

Godot is also editor-driven, but lighter and more approachable than Unity for many 2D projects. Its scene/node model gives structure that MonoGame does not provide by default.

MonoGame gives more control and fewer engine conventions. Godot gives faster scene assembly, UI, animation, and asset iteration.

## raylib

raylib is closer to MonoGame philosophically: small, code-first, direct. It is simpler and lower-level, with C/C++ roots and bindings for other languages.

MonoGame has stronger C# integration and an XNA-style content model. raylib likely has less ceremony for tiny experiments, while MonoGame may fit better for a C# game codebase.

## Hand-Rolled SDL-Style Engine

A custom engine over SDL or similar gives maximum control but also maximum responsibility. MonoGame removes some platform/render/audio/input plumbing while still preserving a code-first model.

For this learning project, MonoGame is a better middle ground: enough framework to build a demo, enough manual work to reveal engine tradeoffs.

## Pain Points Observed

- Template restore can fail in restricted environments and needs explicit `dotnet restore`.
- MGCB local tools must be restored per project.
- MonoGame build targets glob `**/*.mgcb` by default, so deliberately broken MGCB files must be excluded or kept outside the project.
- On-screen text requires a `SpriteFont` asset and content build; there is no immediate-mode debug text helper in the base template.
