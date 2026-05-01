using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E05ContentPipeline;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private readonly ContentSmokeSettings? _smokeSettings = ContentSmokeSettings.FromEnvironment();
    private SpriteBatch? _spriteBatch;
    private Texture2D? _tile;
    private SpriteFont? _font;
    private SoundEffect? _beep;
    private int _updateFrames;
    private bool _smokePlayedSound;

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
        Window.Title = "E05 Content Pipeline";
        Console.WriteLine("Initialize: content pipeline experiment. Space plays Content-loaded sound; Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: play sound at frame 30, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _tile = Content.Load<Texture2D>(ContentAssets.TextureName);
        _font = Content.Load<SpriteFont>(ContentAssets.FontName);
        _beep = Content.Load<SoundEffect>(ContentAssets.SoundName);
        Console.WriteLine($"LoadContent: texture={ContentAssets.TextureName} {_tile.Width}x{_tile.Height}; font={ContentAssets.FontName}; sound={ContentAssets.SoundName} duration={_beep.Duration.TotalMilliseconds:0}ms.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        if (keyboard.IsKeyDown(Keys.Space))
        {
            _beep?.Play();
        }

        if (_smokeSettings is not null && !_smokePlayedSound && _updateFrames >= 30)
        {
            _beep?.Play();
            _smokePlayedSound = true;
            Console.WriteLine("Smoke: played Content-loaded sound.");
        }

        if (_smokeSettings?.ShouldExit(_updateFrames) == true)
        {
            Console.WriteLine("Smoke: exit.");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(24, 26, 32));

        if (_spriteBatch is null || _tile is null || _font is null)
        {
            return;
        }

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        _spriteBatch.Draw(_tile, new Rectangle(80, 96, 128, 128), Color.White);
        _spriteBatch.DrawString(_font, "PNG + SpriteFont + WAV loaded through MGCB", new Vector2(240, 112), Color.White);
        _spriteBatch.DrawString(_font, "Press Space to play beep. Escape exits.", new Vector2(240, 144), new Color(170, 220, 255));
        _spriteBatch.DrawString(_font, $"Texture: {ContentAssets.TextureName}", new Vector2(240, 188), new Color(200, 200, 200));
        _spriteBatch.DrawString(_font, $"Font: {ContentAssets.FontName}", new Vector2(240, 216), new Color(200, 200, 200));
        _spriteBatch.DrawString(_font, $"Sound: {ContentAssets.SoundName}", new Vector2(240, 244), new Color(200, 200, 200));
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
