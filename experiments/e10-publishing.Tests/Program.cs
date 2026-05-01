using E10Publishing;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

var disabledSmoke = PublishSmokeSettings.FromValues(null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled without frame value.");

var smoke = PublishSmokeSettings.FromValues("75");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}

AssertTrue(!smoke.ShouldExit(74), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(75), "Smoke should exit at the configured frame.");
AssertTrue(PublishMetadata.RuntimeIdentifier == "osx-x64", "Documented runtime identifier should match the publish command.");

Console.WriteLine("E10 tests passed.");
