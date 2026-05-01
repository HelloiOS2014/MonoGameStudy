using E02Rendering;
using Microsoft.Xna.Framework;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var mode = new RenderModeState();
AssertTrue(mode.IsBatched, "Rendering should start in batched mode.");
AssertTrue(mode.ModeLabel == "Batched", "Initial label should describe batched mode.");

mode.Update(togglePressed: false);
AssertTrue(mode.IsBatched, "Mode should not change without an F1 press.");

mode.Update(togglePressed: true);
AssertTrue(!mode.IsBatched, "F1 press should switch to unbatched mode.");
AssertTrue(mode.ModeLabel == "Unbatched", "Label should describe unbatched mode.");

mode.Update(togglePressed: true);
AssertTrue(!mode.IsBatched, "Holding F1 should not toggle repeatedly.");

mode.Update(togglePressed: false);
mode.Update(togglePressed: true);
AssertTrue(mode.IsBatched, "Second distinct F1 press should switch back to batched mode.");

var sprites = SpriteField.Create(spriteCount: 1000, viewportWidth: 960, viewportHeight: 540, spriteSize: 16);
AssertTrue(sprites.Length == 1000, "Sprite field should create exactly 1000 sprites.");
AssertTrue(sprites[0] == new Vector2(16, 48), "First sprite should start at the expected margin.");
AssertTrue(sprites[^1].X <= 960 - 16, "Last sprite should fit inside the viewport width.");
AssertTrue(sprites[^1].Y <= 540 - 16, "Last sprite should fit inside the viewport height.");

var smoke = RenderingSmokeSettings.FromValues(toggleAfterFrames: "30", exitAfterFrames: "90");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse valid frame values.");
}
AssertTrue(smoke.ShouldPressToggle(updateFrame: 30), "Smoke mode should toggle on the configured frame.");
AssertTrue(!smoke.ShouldPressToggle(updateFrame: 31), "Smoke mode should only toggle once.");
AssertTrue(smoke.ShouldExit(updateFrame: 90), "Smoke mode should exit on the configured frame.");

Console.WriteLine("E02 tests passed.");
