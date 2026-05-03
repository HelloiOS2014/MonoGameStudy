# Expected State: 00 Intro

## Visual State

Running the integrated demo opens the collector arena. The player is visible with yellow pickups, red hazards, score/status text, and prompts for start, play, win, loss, and restart phases.

## Command Evidence

The non-interactive smoke command should advance through the demo and print the phase transition lines used by `course/lessons/00-intro.mdx`, including win and restart output before `Smoke: exit.`.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
