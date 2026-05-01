using E05ContentPipeline;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

AssertTrue(ContentAssets.TextureName == "Images/pipeline_tile", "Texture asset name should match Content.mgcb.");
AssertTrue(ContentAssets.FontName == "PipelineStatus", "Font asset name should match Content.mgcb.");
AssertTrue(ContentAssets.SoundName == "Audio/beep", "Sound asset name should match Content.mgcb.");

var disabledSmoke = ContentSmokeSettings.FromValues(null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled without frame value.");

var smoke = ContentSmokeSettings.FromValues("90");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}
AssertTrue(!smoke.ShouldExit(89), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(90), "Smoke should exit at the configured frame.");

Console.WriteLine("E05 tests passed.");
