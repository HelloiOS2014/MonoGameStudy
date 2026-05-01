using E01GameLoop;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var state = new GameLoopState();

AssertTrue(state.IsFixedTimeStep, "New loop state should start in fixed timestep mode.");
AssertTrue(state.ModeLabel == "Fixed 60 Hz", "Fixed mode label should describe the default mode.");

state.Update(togglePressed: false);
AssertTrue(state.IsFixedTimeStep, "Loop state should not change without an F1 press.");

state.Update(togglePressed: true);
AssertTrue(!state.IsFixedTimeStep, "F1 press should switch to variable timestep mode.");
AssertTrue(state.ModeLabel == "Variable", "Variable mode label should describe the active mode.");

state.Update(togglePressed: true);
AssertTrue(!state.IsFixedTimeStep, "Holding F1 should not toggle repeatedly.");

state.Update(togglePressed: false);
state.Update(togglePressed: true);
AssertTrue(state.IsFixedTimeStep, "A second distinct F1 press should switch back to fixed timestep mode.");

var disabledSmoke = LoopSmokeSettings.FromValues(toggleAfterFrames: null, exitAfterFrames: null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled when no frame values are provided.");

var smoke = LoopSmokeSettings.FromValues(toggleAfterFrames: "3", exitAfterFrames: "5");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse valid frame values.");
}
AssertTrue(!smoke.ShouldPressToggle(updateFrame: 2), "Smoke mode should not toggle before the configured frame.");
AssertTrue(smoke.ShouldPressToggle(updateFrame: 3), "Smoke mode should toggle on the configured frame.");
AssertTrue(!smoke.ShouldPressToggle(updateFrame: 4), "Smoke mode should only toggle for one update frame.");
AssertTrue(!smoke.ShouldExit(updateFrame: 4), "Smoke mode should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(updateFrame: 5), "Smoke mode should exit on the configured frame.");

Console.WriteLine("E01 tests passed.");
