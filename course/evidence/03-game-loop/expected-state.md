# Expected State: 03 Game Loop

## Visual State

The e01 window starts fixed timestep with title `E01 Game Loop - Fixed 60 Hz`.

## Command Evidence

Smoke toggles to variable timestep. Stdout includes `Update: timestep mode changed to Variable.` and then `Smoke: exit.` before terminating.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
