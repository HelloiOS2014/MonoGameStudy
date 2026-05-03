# Expected State: 03 Game Loop

## Visual State

The e01 window starts in fixed timestep mode with a title matching `E01 Game Loop - Fixed 60 Hz`. Toggling with `F1` switches the title to the variable timestep state.

## Command Evidence

The smoke run should print `Update: timestep mode changed to Variable.` and then `Smoke: exit.` before terminating.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
