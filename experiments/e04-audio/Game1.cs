using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace E04Audio;

public class Game1 : Game
{
    private readonly AudioControlState _audio = new();
    private readonly ButtonEdgeState _effectEdge = new();
    private readonly ButtonEdgeState _musicEdge = new();
    private readonly AudioSmokeSettings? _smokeSettings = AudioSmokeSettings.FromEnvironment();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixel;
    private SoundEffect? _effect;
    private Song? _music;
    private int _updateFrames;
    private int _effectFlashFrames;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 960;
        _graphics.PreferredBackBufferHeight = 540;
        _graphics.ApplyChanges();
        ApplyWindowTitle();
        Console.WriteLine("Initialize: audio experiment. Space plays SoundEffect; M toggles looping Song; Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: effect at frames 30/90/120, music toggles at 60/120, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _effect = Content.Load<SoundEffect>(AudioAssets.EffectName);
        _music = Content.Load<Song>(AudioAssets.MusicName);
        MediaPlayer.Volume = 0.35f;
        MediaPlayer.IsRepeating = true;
        Console.WriteLine($"LoadContent: effect={AudioAssets.EffectName} duration={_effect.Duration.TotalMilliseconds:0}ms; song={AudioAssets.MusicName} duration={_music.Duration.TotalMilliseconds:0}ms.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _effectEdge.Update(keyboard.IsKeyDown(Keys.Space));
        _musicEdge.Update(keyboard.IsKeyDown(Keys.M));

        var input = _smokeSettings?.InputForFrame(_updateFrames)
            ?? new AudioInputFrame(_effectEdge.PressedThisFrame, _musicEdge.PressedThisFrame);
        ApplyAudioActions(_audio.Update(input.PlayEffectPressed, input.ToggleMusicPressed));

        if (_effectFlashFrames > 0)
        {
            _effectFlashFrames--;
        }

        ApplyWindowTitle();

        if (_smokeSettings?.ShouldExit(_updateFrames) == true)
        {
            Console.WriteLine("Smoke: exit.");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(18, 22, 28));

        if (_spriteBatch is null || _pixel is null)
        {
            return;
        }

        var musicColor = _audio.MusicEnabled ? new Color(80, 210, 130) : new Color(90, 90, 96);
        var effectColor = _effectFlashFrames > 0 ? new Color(255, 210, 80) : new Color(80, 120, 180);

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        _spriteBatch.Draw(_pixel, new Rectangle(160, 170, 280, 160), musicColor);
        _spriteBatch.Draw(_pixel, new Rectangle(520, 170, 280, 160), effectColor);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void ApplyAudioActions(AudioActions actions)
    {
        if (actions.MusicAction == MusicAction.StartLoop && _music is not null)
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_music);
            Console.WriteLine("Audio: music loop started.");
        }
        else if (actions.MusicAction == MusicAction.Stop)
        {
            MediaPlayer.Stop();
            Console.WriteLine("Audio: music stopped.");
        }

        if (actions.PlayEffect && _effect is not null)
        {
            _effect.Play(volume: 0.85f, pitch: 0f, pan: 0f);
            _effectFlashFrames = 12;
            Console.WriteLine($"Audio: sound effect played at frame {_updateFrames}.");
        }
    }

    private void ApplyWindowTitle()
    {
        Window.Title = $"E04 Audio - music {(_audio.MusicEnabled ? "on" : "off")} - Space effect - M music";
    }
}
