# Audio

## Goal

Use `SoundEffect` for short sounds and `Song`/`MediaPlayer` for looping music.

## What You Will Run

```bash
env E04_SMOKE_EXIT_AFTER_FRAMES=150 dotnet run --project experiments/e04-audio/E04Audio.csproj --no-restore
```

## Key Files

- [experiments/e04-audio/Game1.cs](../../experiments/e04-audio/Game1.cs) - loads audio and applies playback actions.
- [experiments/e04-audio/AudioControlState.cs](../../experiments/e04-audio/AudioControlState.cs) - separates sound-effect and music actions.
- [experiments/e04-audio/Content/Content.mgcb](../../experiments/e04-audio/Content/Content.mgcb) - builds WAV files as `SoundEffect` and `Song`.
- [experiments/e04-audio.Tests/Program.cs](../../experiments/e04-audio.Tests/Program.cs) - verifies action routing without audio hardware.

## Walkthrough

MonoGame has different APIs for short effects and longer music:

- `SoundEffect` is for short, latency-sensitive sounds such as clicks, pickups, shots, and impacts.
- `Song` is loaded content for music.
- `MediaPlayer` is the global player used to play, stop, and loop `Song` instances.

`Game1.LoadContent` loads both assets, then sets:

```csharp
MediaPlayer.Volume = 0.35f;
MediaPlayer.IsRepeating = true;
```

`SoundEffect.Play` is independent from `MediaPlayer.Play` and `MediaPlayer.Stop`. The experiment keeps that separation in `AudioControlState`: one frame can request a sound effect, a music toggle, or both.

The smoke run starts the loop, plays the effect, stops music, plays the effect while music is stopped, then restarts music and plays the effect on the same frame. That proves effect playback and music state are separate actions.

## Expected Output

Stdout includes:

```text
Audio: music loop started.
Audio: music stopped.
Audio: sound effect played at frame 120.
Smoke: exit.
```

Manual mode opens a window with two status blocks. Press `Space` for the effect and `M` to toggle music.

## Common Problems

- `MediaPlayer` is global process state, so do not model it like many independent sound-effect instances.
- This experiment reuses small WAV assets to focus on API behavior. A real game should use a proper loopable music source.
- The `Song` and `SoundEffect` logical names must match `Content.mgcb`.

## Checkpoint

You are ready when you can choose between `SoundEffect` and `Song` for a given sound in a game.
