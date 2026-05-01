# E04 Audio

## How to run

```bash
dotnet run --project experiments/e04-audio/E04Audio.csproj
```

Automated smoke:

```bash
E04_SMOKE_EXIT_AFTER_FRAMES=150 dotnet run --project experiments/e04-audio/E04Audio.csproj
```

## What you should see

A DesktopGL window opens with two large status blocks. The left block is green when the looping `Song` is playing and gray when music is stopped. The right block flashes yellow when the `SoundEffect` plays.

Controls:

- `Space` plays the short sound effect.
- `M` toggles the looping music.
- `Escape` exits.

The smoke run starts music, plays the effect, toggles music off, plays the effect again while music is stopped, then toggles music back on while also playing the effect.

## What was learned

MonoGame separates short, latency-sensitive clips from longer music: use `SoundEffect` for immediate effects and `MediaPlayer`/`Song` for music. Stopping or starting `MediaPlayer` does not need to block `SoundEffect.Play`, so input handling should emit independent actions for effect playback and music state changes.

The music loop currently reuses the same small WAV source as the effect so the experiment stays focused on API behavior instead of asset production. That is acceptable for learning the routing difference, but a real game should use a proper loopable music asset.
