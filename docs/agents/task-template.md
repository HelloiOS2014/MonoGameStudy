# Task Template

## Minimal Format

```text
<Type>: <goal>
验收: <observable result, command, or output>
```

The type must be one of `Docs`, `Fix`, `Experiment`, `Demo`, `Tooling`, or `Research`.

## Chinese Acceptance

Use `验收:` when writing tasks in Chinese or mixed Chinese/English.

```text
Docs: Tighten the setup tutorial wording for the MGCB restore step
验收: The changed chapter names the exact command and git diff has no trailing whitespace.
```

## English Acceptance

Use `Acceptance:` when writing tasks in English.

```text
Demo: Fix the restart prompt copy after win state
Acceptance: Integrated demo tests pass and the smoke exits after the configured frame limit.
```

## Examples

```text
Docs: Tighten the setup tutorial wording for the MGCB restore step
验收: The changed chapter names the exact command and git diff has no trailing whitespace.
```

```text
Fix: Repair the e05 content smoke after an asset path rename
验收: The failing smoke is reproduced first, then the targeted smoke and GameDemo.sln build pass.
```

```text
Experiment: Add a mouse-drag input variant to e03
验收: New smoke simulates a drag path and exits; GameDemo.sln builds with 0 errors.
```

```text
Demo: Fix the restart prompt copy after win state
Acceptance: Integrated demo tests pass and the smoke exits after the configured frame limit.
```

```text
Tooling: Add link checks to tutorial verification
验收: The script reports a broken internal tutorial link with a non-zero exit code.
```

```text
Research: Evaluate whether an e08 shader experiment belongs in v2
Acceptance: The report lists local evidence, tradeoffs, and a recommendation without runtime changes.
```

## When Free-Form Input Is Allowed

Free-form input is allowed as task intake, not as permission to ignore scope.

- Classify the request before editing.
- If the classification is low risk, state the assumed type and proceed.
- If classification changes the planning gate or scope, ask one concise question or draft the required spec or plan.
- If acceptance is subjective, such as `feels better`, stop and propose observable acceptance before implementation.
- For `Experiment` and `Demo`, free-form input triggers planning before runtime edits.
