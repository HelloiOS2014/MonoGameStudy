# Expected State: 06 Content Pipeline

## Visual State

Window shows content-loaded texture/font/sound state; broken MGCB command fails with missing texture path.

## Command Evidence

The working smoke should log loaded logical asset names for the texture, font, and sound. The broken-content command is expected to fail on the missing texture path.

## Verification Boundary

This file describes the expected observable state. A reviewer must still report whether they verified it by GUI run, smoke run, or tests.
