# MonoGame Technical Evaluation

## Verdict

Keep using MonoGame for small, code-first 2D projects where learning and owning the engine-shaped systems is the point. Do not choose it when the project needs editor-first workflows, non-programmer content iteration, packaged macOS distribution, or built-in animation/scene tooling.

## Strengths

- Direct loop control is clear. `e01-game-loop` toggled fixed and variable timestep at runtime and logged frame cadence without hidden engine behavior.
- 2D drawing is enough for simple games. `e02-2d-rendering` rendered 1000 sprites at 60fps on the dev machine and made batching costs visible.
- Input is simple and explicit. `e03-input` showed keyboard, mouse, and gamepad-style polling can be normalized in small helper code.
- The content pipeline is scriptable. `e05-content-pipeline` loaded a PNG, `SpriteFont`, and WAV via MGCB and reproduced a deliberate bad-path failure.
- Audio APIs are usable for basic needs. `e04-audio` kept `SoundEffect` playback independent from `MediaPlayer` music toggling.

## Weaknesses

- Common game systems are user code. `e06-camera-and-collision` needed custom camera math, AABB/circle collision, and debug drawing.
- Animation is manual. `e07-animation` needed a frame timer, animation controller, and idle/walk/jump state machine.
- Packaging is not product-ready by default. `e10-publishing` produced a self-contained runnable directory, not a signed `.app`, installer, or notarized build.
- Content builds are easy to misconfigure. `e05-content-pipeline` showed default `.mgcb` globbing can accidentally build intentionally broken files unless explicitly controlled.

## Risks

- Larger games will accumulate framework code quickly. The integrated demo stayed small, but it already needed custom state, collision, input edges, content loading, and smoke hooks.
- Tooling friction will matter more with real assets. `e05-content-pipeline` and `docs/03-content-pipeline.md` show MGCB is understandable, but less forgiving than editor import workflows.
- macOS distribution needs a separate packaging phase. `e10-publishing` verified runtime independence, but signing, notarization, `.app` layout, and installer flow remain unsolved.

## Comparison

Unity remains stronger for editor workflows, asset pipelines, team iteration, and deployment polish. MonoGame is better here only because this project values code-first learning over editor productivity.

Godot would likely ship a comparable 2D prototype faster because scenes, UI, animation, and collision are built in. MonoGame gives more control but requires more local systems code.

raylib is similarly code-first and probably has less ceremony for tiny prototypes. MonoGame fits better if staying in C# and using an XNA-style content model matters.

A hand-rolled SDL-style engine gives maximum control but would add more platform/audio/input/rendering plumbing than this three-week study can justify. MonoGame is the better middle ground for this research.

## Answer

Yes, keep using MonoGame for the next small 2D learning project if the goal is to understand and own the loop, rendering, input, content, audio, collision, animation, and packaging tradeoffs. For a production game with rich tools, editor iteration, or polished distribution requirements, start with Godot or Unity instead.
