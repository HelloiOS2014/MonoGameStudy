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

Console.WriteLine("GameLoopState tests passed.");
