namespace E07Animation;

public sealed class ButtonEdgeState
{
    private bool _wasDown;

    public bool PressedThisFrame { get; private set; }

    public void Update(bool isDown)
    {
        PressedThisFrame = isDown && !_wasDown;
        _wasDown = isDown;
    }
}
