namespace E01GameLoop;

public sealed class GameLoopState
{
    private bool _wasTogglePressed;

    public bool IsFixedTimeStep { get; private set; } = true;

    public string ModeLabel => IsFixedTimeStep ? "Fixed 60 Hz" : "Variable";

    public bool Update(bool togglePressed)
    {
        var toggled = togglePressed && !_wasTogglePressed;
        if (toggled)
        {
            IsFixedTimeStep = !IsFixedTimeStep;
        }

        _wasTogglePressed = togglePressed;
        return toggled;
    }
}
