# MonoGame Research Project Design

Date: 2026-04-26
Status: Approved for planning

## Purpose

This repository is a structured research project for learning MonoGame from zero to the point where a small, complete game demo can be built independently.

The project is not intended to claim full MonoGame mastery in the first pass. The intended first milestone is practical competence:

- Understand what MonoGame is and how it differs from editor-driven engines such as Unity and Godot.
- Build and run MonoGame projects from scratch.
- Understand the game lifecycle: `Initialize`, `LoadContent`, `Update`, and `Draw`.
- Use core systems for rendering, input, audio, resources, camera behavior, collision, basic 3D, shaders, and publishing.
- Produce a playable integrated demo.
- Write a technical evaluation explaining when MonoGame is a good fit and when it is not.

## Scope

The project will cover both 2D and basic 3D. It will not assume MonoGame is only for 2D, but the first practical path will lean on 2D because it is the fastest way to build correct intuition for the framework.

The first phase will not attempt:

- Deep MonoGame source-code analysis.
- Commercial-grade engine architecture.
- Advanced 3D rendering pipelines.
- Full production publishing across every supported platform.
- Custom editor tooling.

Those topics can become follow-up phases after the first research cycle is complete.

## Recommended Learning Strategy

The project will use a course-like structure:

1. Learn the framework concepts.
2. Build small focused experiments.
3. Combine the useful pieces into a complete demo.
4. Evaluate MonoGame as a technology choice.

This approach is preferred over starting with one large game immediately. MonoGame is a framework rather than a complete editor engine, so isolated experiments will expose the framework's real workflow and friction points faster.

## Repository Structure

```text
game_demo/
  README.md
  docs/
    00-roadmap.md
    01-monogame-overview.md
    02-engine-comparison.md
    03-content-pipeline.md
    04-platforms-and-publishing.md
    reports/
      monogame-technical-evaluation.md

  experiments/
    01-game-loop/
    02-2d-rendering/
    03-input/
    04-audio/
    05-content-pipeline/
    06-camera-and-collision/
    07-basic-3d/
    08-shader/
    09-publishing/

  demo/
    integrated-demo/
```

The `docs` directory holds learning notes and final evaluation material.

The `experiments` directory holds focused MonoGame experiments. Each experiment should answer one concrete question and include a short note describing what was learned.

The `demo/integrated-demo` directory holds the final small playable project.

## Three-Week Research Plan

### Week 1: Framework Entry

Goal: build a working mental model of MonoGame and run basic projects.

Topics:

- MonoGame positioning and ecosystem.
- DesktopGL-first project setup.
- Project structure and templates.
- Game lifecycle.
- 2D rendering with `SpriteBatch`.
- Texture and font loading.
- Keyboard, mouse, and controller input basics.
- Content Pipeline basics.

Expected output:

- Basic working MonoGame project.
- Initial notes explaining the project lifecycle and framework structure.

### Week 2: Capability Experiments

Goal: test important MonoGame subsystems in isolation.

Experiments:

- Audio playback and sound effects.
- Collision detection.
- Camera and viewport behavior.
- Animation state handling.
- Basic 3D model, camera, and lighting path.
- Minimal shader example.
- Initial publishing and packaging check.

Expected output:

- Focused experiments under `experiments/`.
- Short notes for each experiment covering purpose, implementation, friction points, and reusable takeaways.

### Week 3: Integrated Demo And Evaluation

Goal: combine the useful parts into a playable demo and write a practical evaluation.

The integrated demo should be small. Its exact genre should be chosen after the experiments reveal MonoGame's development feel. The default recommendation is a 2D or 2.5D game with at least one optional basic 3D or shader validation scene.

Expected output:

- `demo/integrated-demo` is runnable.
- `README.md` explains setup and execution.
- `docs/reports/monogame-technical-evaluation.md` explains strengths, weaknesses, risks, and fit compared with Unity, Godot, raylib, and custom engine work.

## Evaluation Criteria

The research is successful when the repository can answer these questions with working evidence:

- How do you create, run, and structure a MonoGame project?
- How does the MonoGame game loop work in practice?
- How are content assets loaded and managed?
- How much code is required for common rendering, input, audio, and camera tasks?
- What is straightforward in MonoGame?
- What becomes manual or costly compared with editor-driven engines?
- What kind of small game can be built comfortably after this learning cycle?
- Is MonoGame worth deeper study for the user's future game-development goals?

## Testing And Verification

Each code milestone should be verified by running the relevant project locally.

For experiments, verification means the sample opens and demonstrates the intended behavior. For the integrated demo, verification means the game can be launched from the command line and played through its core loop.

Documentation should be checked for consistency with the current code before the final evaluation is written.

## Follow-Up Phase Candidates

After this phase, possible deeper tracks include:

- MonoGame source-code reading.
- Advanced 3D rendering and shader work.
- ECS or custom game architecture on top of MonoGame.
- Asset tooling and editor workflow.
- Cross-platform publishing.
- Performance profiling.

