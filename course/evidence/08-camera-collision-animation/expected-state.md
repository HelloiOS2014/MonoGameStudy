# Expected State: 08 Camera, Collision, And Animation

## Visual State

Camera/collision smoke logs AABB and circle collision; animation smoke logs idle/walk/jump transitions.

## Command Evidence

The e06 smoke should log AABB and circle collision states. The e07 smoke should log animation transitions and terminate from its configured frame limit without manual input.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
