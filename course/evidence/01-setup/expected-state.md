# Expected State: 01 Setup

## Visual State

No GUI window; command output is the expected state.

## Command Evidence

`./tools/check-env.sh` should print the .NET SDK version, confirm MonoGame template availability, confirm required local paths, and exit with status 0 on a prepared macOS DesktopGL machine.

## Verification Boundary

This file describes the expected setup signal. A reviewer must still report the exact command output and exit code from the current machine.
