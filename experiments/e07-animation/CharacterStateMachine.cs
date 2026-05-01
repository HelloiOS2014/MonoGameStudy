using System;

namespace E07Animation;

public readonly record struct CharacterInput
{
    public float HorizontalAxis { get; }
    public bool JumpPressed { get; }

    public CharacterInput(float horizontalAxis, bool jumpPressed)
    {
        HorizontalAxis = horizontalAxis;
        JumpPressed = jumpPressed;
    }
}

public readonly record struct CharacterTransition(CharacterAnimationState From, CharacterAnimationState To);

public sealed class CharacterStateMachine
{
    public const float JumpDurationSeconds = 0.45f;

    private float _jumpRemaining;

    public CharacterAnimationState State { get; private set; } = CharacterAnimationState.Idle;

    public CharacterTransition? Update(CharacterInput input, float elapsedSeconds)
    {
        if (State == CharacterAnimationState.Jump)
        {
            _jumpRemaining -= elapsedSeconds;
            if (_jumpRemaining > 0f)
            {
                return null;
            }

            return TransitionTo(MathF.Abs(input.HorizontalAxis) > 0.01f
                ? CharacterAnimationState.Walk
                : CharacterAnimationState.Idle);
        }

        if (input.JumpPressed)
        {
            _jumpRemaining = JumpDurationSeconds;
            return TransitionTo(CharacterAnimationState.Jump);
        }

        if (MathF.Abs(input.HorizontalAxis) > 0.01f && State == CharacterAnimationState.Idle)
        {
            return TransitionTo(CharacterAnimationState.Walk);
        }

        if (MathF.Abs(input.HorizontalAxis) <= 0.01f && State == CharacterAnimationState.Walk)
        {
            return TransitionTo(CharacterAnimationState.Idle);
        }

        return null;
    }

    private CharacterTransition TransitionTo(CharacterAnimationState next)
    {
        var transition = new CharacterTransition(State, next);
        State = next;
        return transition;
    }
}
