# Verification

## Rule

No completion claim without fresh verification evidence from the current task.

Report the command, exit code, pass/fail reading, skipped command, and reason.

## Fresh Workspace Commands

Use this path when dependencies may not be restored:

```bash
./tools/check-env.sh
dotnet restore GameDemo.sln
dotnet build GameDemo.sln -m:1
git diff --check
```

## Warmed Workspace Commands

Use `--no-restore` only after restore has succeeded in this workspace or there is evidence dependencies are already restored:

```bash
dotnet build GameDemo.sln --no-restore -m:1
git diff --check
```

## Command Matrix

| Type | Required Commands |
| --- | --- |
| `Docs` | `git diff --check`; unresolved-marker grep on changed docs; `bash -n tools/check-tutorial.sh` when tutorial commands change |
| `Fix` | Reproduce the failure when feasible; run the smallest command proving the fix; run broader build or smoke when shared behavior changed |
| `Experiment` | Targeted test project; experiment smoke; `dotnet build GameDemo.sln --no-restore -m:1`; `git diff --check` |
| `Demo` | `dotnet run --project demo/integrated-demo.Tests/IntegratedDemo.Tests.csproj --no-restore`; `env DEMO_SMOKE_EXIT_AFTER_FRAMES=160 dotnet run --project demo/integrated-demo/IntegratedDemo.csproj --no-restore`; `dotnet build GameDemo.sln --no-restore -m:1` |
| `Tooling` | Shell syntax; focused script behavior; failure-path command when script handles failures |
| `Research` | Local file evidence or primary-source citations; assumptions; unknowns; no implementation completion claim |

Docs unresolved-marker check:

```bash
git grep -n -E "UN""RESOLVED|NEEDS""_DECISION|FIX""ME" -- README.md AGENTS.md docs/agents docs/tutorial
```

Expected when docs are complete: exit 1 with no matches.

Shell syntax:

```bash
bash -n tools/check-env.sh
bash -n tools/check-tutorial.sh
```

## GUI Smoke Notes

DesktopGL smoke commands open short-lived windows.

GUI smoke may require local desktop access or tool escalation. If GUI access is unavailable, report the blocker and do not claim the smoke passed.

## Failure Handling

- Prefer reproducing failures before fixing them.
- If reproduction is impossible, state why and use the closest proving command.
- If verification fails repeatedly, stop and report the failing command and output.
- Do not broaden scope to make verification pass without updating the task type or plan.

## Evidence To Report

For each verification command, report:

- command,
- exit code,
- pass/fail reading,
- skipped command and reason.
