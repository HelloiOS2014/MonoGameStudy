# Expected State: 02 First Window

## Visual State

960x540 blue DesktopGL window with title `E01 Game Loop - Fixed 60 Hz`.

## Command Evidence

Stdout logs `Initialize` and `LoadContent` before frame logs. The learner can point to `Program.cs`, `Game1.cs`, and the `.csproj` as the minimum MonoGame DesktopGL project shape.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
