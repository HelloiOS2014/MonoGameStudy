namespace E03Input;

public sealed class ButtonEdgeState
{
    private bool _wasDown;

    public bool Held { get; private set; }

    public bool PressedThisFrame { get; private set; }

    public bool ReleasedThisFrame { get; private set; }

    public void Update(bool isDown)
    {
        PressedThisFrame = isDown && !_wasDown;
        ReleasedThisFrame = !isDown && _wasDown;
        Held = isDown;
        _wasDown = isDown;
    }
}
