namespace E07Animation;

public enum CharacterAnimationState
{
    Idle,
    Walk,
    Jump
}

public sealed class AnimationController
{
    private readonly FrameAnimation _idle = new(frameCount: 4, secondsPerFrame: 0.25f);
    private readonly FrameAnimation _walk = new(frameCount: 6, secondsPerFrame: 0.10f);
    private readonly FrameAnimation _jump = new(frameCount: 4, secondsPerFrame: 0.12f);

    public CharacterAnimationState CurrentState { get; private set; } = CharacterAnimationState.Idle;
    public int CurrentFrame => CurrentAnimation.CurrentFrame;

    public void Update(CharacterAnimationState state, float elapsedSeconds)
    {
        if (state != CurrentState)
        {
            CurrentState = state;
            CurrentAnimation.Reset();
            return;
        }

        CurrentAnimation.Update(elapsedSeconds);
    }

    private FrameAnimation CurrentAnimation => CurrentState switch
    {
        CharacterAnimationState.Walk => _walk,
        CharacterAnimationState.Jump => _jump,
        _ => _idle
    };
}
