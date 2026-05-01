using E07Animation;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var animation = new FrameAnimation(frameCount: 4, secondsPerFrame: 0.1f);
animation.Update(0.11f);
AssertTrue(animation.CurrentFrame == 1, "Animation should advance after one frame duration.");
animation.Update(0.30f);
AssertTrue(animation.CurrentFrame == 0, "Animation should loop after consuming the fourth frame.");
animation.Reset();
AssertTrue(animation.CurrentFrame == 0, "Animation reset should return to the first frame.");

var controller = new AnimationController();
AssertTrue(controller.CurrentState == CharacterAnimationState.Idle, "Controller should start idle.");
AssertTrue(controller.CurrentFrame == 0, "Controller should start on frame zero.");
controller.Update(CharacterAnimationState.Walk, 0.2f);
AssertTrue(controller.CurrentState == CharacterAnimationState.Walk, "Controller should switch to walk.");
AssertTrue(controller.CurrentFrame == 0, "Controller should reset frame when state changes.");
controller.Update(CharacterAnimationState.Walk, 0.12f);
AssertTrue(controller.CurrentFrame == 1, "Walk animation should advance frame-by-frame.");

var state = new CharacterStateMachine();
var transition = state.Update(new CharacterInput(horizontalAxis: 1f, jumpPressed: false), elapsedSeconds: 0.016f);
AssertTrue(transition == new CharacterTransition(CharacterAnimationState.Idle, CharacterAnimationState.Walk), "Horizontal input should transition idle to walk.");
AssertTrue(state.State == CharacterAnimationState.Walk, "State should now be walk.");

transition = state.Update(new CharacterInput(horizontalAxis: 0f, jumpPressed: false), elapsedSeconds: 0.016f);
AssertTrue(transition == new CharacterTransition(CharacterAnimationState.Walk, CharacterAnimationState.Idle), "Stopping horizontal input should transition walk to idle.");

transition = state.Update(new CharacterInput(horizontalAxis: 0f, jumpPressed: true), elapsedSeconds: 0.016f);
AssertTrue(transition == new CharacterTransition(CharacterAnimationState.Idle, CharacterAnimationState.Jump), "Jump press should transition idle to jump.");
AssertTrue(state.State == CharacterAnimationState.Jump, "State should now be jump.");

transition = state.Update(new CharacterInput(horizontalAxis: 1f, jumpPressed: false), CharacterStateMachine.JumpDurationSeconds);
AssertTrue(transition == new CharacterTransition(CharacterAnimationState.Jump, CharacterAnimationState.Walk), "Jump should land into walk if horizontal input is held.");
AssertTrue(state.State == CharacterAnimationState.Walk, "State should be walk after landing with horizontal input.");

var disabledSmoke = AnimationSmokeSettings.FromValues(null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled without frame value.");

var smoke = AnimationSmokeSettings.FromValues("170");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}

AssertTrue(smoke.InputForFrame(50).HorizontalAxis > 0f, "Smoke should walk after the idle opening.");
AssertTrue(smoke.InputForFrame(95).JumpPressed, "Smoke should press jump.");
AssertTrue(smoke.InputForFrame(130).HorizontalAxis > 0f, "Smoke should keep walking after jump starts.");
AssertTrue(!smoke.ShouldExit(169), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(170), "Smoke should exit at the configured frame.");

Console.WriteLine("E07 tests passed.");
