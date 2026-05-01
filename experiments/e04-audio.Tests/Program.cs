using E04Audio;

static void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new InvalidOperationException(message);
    }
}

AssertTrue(AudioAssets.EffectName == "Audio/click", "Effect asset name should match Content.mgcb.");
AssertTrue(AudioAssets.MusicName == "Audio/music_loop", "Music asset name should match Content.mgcb.");

var audio = new AudioControlState();
var firstFrame = audio.Update(playEffectPressed: false, toggleMusicPressed: false);
AssertTrue(firstFrame.MusicAction == MusicAction.StartLoop, "Music should start looping on the first update.");
AssertTrue(audio.MusicEnabled, "Music should be enabled by default.");

var effectOnly = audio.Update(playEffectPressed: true, toggleMusicPressed: false);
AssertTrue(effectOnly.PlayEffect, "Effect key should request a SoundEffect play.");
AssertTrue(effectOnly.MusicAction == MusicAction.None, "Effect play should not restart or stop music.");

var toggleOffAndEffect = audio.Update(playEffectPressed: true, toggleMusicPressed: true);
AssertTrue(toggleOffAndEffect.PlayEffect, "Effect should still play on the same frame music is toggled.");
AssertTrue(toggleOffAndEffect.MusicAction == MusicAction.Stop, "Music toggle should request a stop when currently enabled.");
AssertTrue(!audio.MusicEnabled, "Music should be disabled after toggling off.");

var toggleOn = audio.Update(playEffectPressed: false, toggleMusicPressed: true);
AssertTrue(!toggleOn.PlayEffect, "Music-only toggle should not play a sound effect.");
AssertTrue(toggleOn.MusicAction == MusicAction.StartLoop, "Music toggle should restart the loop when disabled.");
AssertTrue(audio.MusicEnabled, "Music should be enabled after toggling on.");

var disabledSmoke = AudioSmokeSettings.FromValues(null);
AssertTrue(disabledSmoke is null, "Smoke settings should be disabled without frame value.");

var smoke = AudioSmokeSettings.FromValues("150");
if (smoke is null)
{
    throw new InvalidOperationException("Smoke settings should parse a valid exit frame.");
}

AssertTrue(smoke.InputForFrame(30).PlayEffectPressed, "Smoke should play the short effect early.");
AssertTrue(smoke.InputForFrame(60).ToggleMusicPressed, "Smoke should toggle music off.");
AssertTrue(smoke.InputForFrame(90).PlayEffectPressed, "Smoke should play the effect while music is stopped.");
AssertTrue(smoke.InputForFrame(120).PlayEffectPressed, "Smoke should play the effect on the same frame music restarts.");
AssertTrue(smoke.InputForFrame(120).ToggleMusicPressed, "Smoke should toggle music on while requesting the effect.");
AssertTrue(!smoke.ShouldExit(149), "Smoke should not exit before the configured frame.");
AssertTrue(smoke.ShouldExit(150), "Smoke should exit at the configured frame.");

Console.WriteLine("E04 tests passed.");
