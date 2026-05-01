using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E02Rendering;

public class Game1 : Game
{
    private const int SpriteCount = 1000;
    private const int SpriteSize = 16;

    private GraphicsDeviceManager _graphics;
    private readonly RenderModeState _renderMode = new();
    private readonly RenderingSmokeSettings? _smokeSettings = RenderingSmokeSettings.FromEnvironment();
    private SpriteBatch? _spriteBatch;
    private Texture2D? _spriteTexture;
    private SpriteFont? _statusFont;
    private Vector2[] _spritePositions = Array.Empty<Vector2>();
    private int _updateFrames;
    private int _drawFramesThisSecond;
    private double _secondsSinceLastLog;
    private double _lastFrameMilliseconds;
    private double _lastFps;

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
        _spritePositions = SpriteField.Create(SpriteCount, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight, SpriteSize);
        ApplyWindowTitle();
        Console.WriteLine("Initialize: 1000-sprite rendering experiment. Press F1 to toggle batched/unbatched drawing. Press Escape to exit.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: toggle at update frame {_smokeSettings.ToggleAfterFrames}; exit at update frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spriteTexture = Content.Load<Texture2D>("SpriteTile");
        _statusFont = Content.Load<SpriteFont>("Status");
        Console.WriteLine("LoadContent: loaded SpriteTile texture and Status SpriteFont.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        var smokeTogglePressed = _smokeSettings?.ShouldPressToggle(_updateFrames) == true;
        if (_renderMode.Update(keyboard.IsKeyDown(Keys.F1) || smokeTogglePressed))
        {
            ApplyWindowTitle();
            Console.WriteLine($"Update: render mode changed to {_renderMode.ModeLabel}.");
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
        GraphicsDevice.Clear(new Color(18, 22, 28));

        if (_spriteBatch is null || _spriteTexture is null || _statusFont is null)
        {
            return;
        }

        _lastFrameMilliseconds = gameTime.ElapsedGameTime.TotalMilliseconds;
        _drawFramesThisSecond++;
        _secondsSinceLastLog += gameTime.ElapsedGameTime.TotalSeconds;
        if (_secondsSinceLastLog >= 1.0)
        {
            _lastFps = _drawFramesThisSecond / _secondsSinceLastLog;
            Console.WriteLine($"Draw: sprites={SpriteCount}, fps={_lastFps:0.0}, frameMs={_lastFrameMilliseconds:0.00}, mode={_renderMode.ModeLabel}");
            _drawFramesThisSecond = 0;
            _secondsSinceLastLog = 0;
        }

        if (_renderMode.IsBatched)
        {
            DrawSpritesBatched(_spriteBatch, _spriteTexture);
        }
        else
        {
            DrawSpritesUnbatched(_spriteBatch, _spriteTexture);
        }

        DrawOverlay(_spriteBatch, _statusFont);

        base.Draw(gameTime);
    }

    private void DrawSpritesBatched(SpriteBatch spriteBatch, Texture2D spriteTexture)
    {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        foreach (var position in _spritePositions)
        {
            spriteBatch.Draw(spriteTexture, new Rectangle((int)position.X, (int)position.Y, SpriteSize, SpriteSize), Color.White);
        }
        spriteBatch.End();
    }

    private void DrawSpritesUnbatched(SpriteBatch spriteBatch, Texture2D spriteTexture)
    {
        foreach (var position in _spritePositions)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(spriteTexture, new Rectangle((int)position.X, (int)position.Y, SpriteSize, SpriteSize), Color.White);
            spriteBatch.End();
        }
    }

    private void DrawOverlay(SpriteBatch spriteBatch, SpriteFont statusFont)
    {
        var text = $"F1: {_renderMode.ModeLabel} | sprites: {SpriteCount} | fps: {_lastFps:0.0} | frame: {_lastFrameMilliseconds:0.00} ms";
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        spriteBatch.DrawString(statusFont, text, new Vector2(16, 14), Color.Black);
        spriteBatch.DrawString(statusFont, text, new Vector2(15, 13), Color.White);
        spriteBatch.End();
    }

    private void ApplyWindowTitle()
    {
        Window.Title = $"E02 2D Rendering - {_renderMode.ModeLabel}";
    }
}
