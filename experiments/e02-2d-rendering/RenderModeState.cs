namespace E02Rendering;

public sealed class RenderModeState
{
    private bool _wasTogglePressed;

    public bool IsBatched { get; private set; } = true;

    public string ModeLabel => IsBatched ? "Batched" : "Unbatched";

    public bool Update(bool togglePressed)
    {
        var toggled = togglePressed && !_wasTogglePressed;
        if (toggled)
        {
            IsBatched = !IsBatched;
        }

        _wasTogglePressed = togglePressed;
        return toggled;
    }
}
