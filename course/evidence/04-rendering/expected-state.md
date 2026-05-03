# Expected State: 04 Rendering

## Visual State

Window shows 1000 sprites plus overlay text.

## Command Evidence

Smoke toggles from batched to unbatched mode, proving the overlay and `RenderModeState` changed together.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
