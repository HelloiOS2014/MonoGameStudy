using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace IntegratedDemo;

public class Game1 : Game
{
    private readonly DemoState _demo = DemoState.CreateDefault();
    private readonly DemoSmokeSettings? _smokeSettings = DemoSmokeSettings.FromEnvironment();
    private readonly ButtonEdgeState _startEdge = new();
    private readonly ButtonEdgeState _restartEdge = new();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixel;
    private SpriteFont? _font;
    private SoundEffect? _collectSound;
    private int _updateFrames;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.SynchronizeWithVerticalRetrace = false;
        Content.RootDirectory = "Content";
        IsFixedTimeStep = false;
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 960;
        _graphics.PreferredBackBufferHeight = 540;
        _graphics.ApplyChanges();
        ApplyWindowTitle();
        Console.WriteLine("Initialize: integrated demo. Enter starts/restarts; WASD/arrows move; R restarts after win/loss; Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: start at frame 1, move right through pickups, restart at frame 140, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        _font = Content.Load<SpriteFont>("Status");
        _collectSound = Content.Load<SoundEffect>("Audio/collect");
        Console.WriteLine("LoadContent: font and collect sound loaded.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        var input = _smokeSettings?.InputForFrame(_updateFrames) ?? ReadLiveInput(keyboard);
        var elapsedSeconds = _smokeSettings is not null
            ? 1f / 60f
            : (float)gameTime.ElapsedGameTime.TotalSeconds;
        var result = _demo.Update(input, elapsedSeconds);
        LogResult(result);

        if (result.Collected)
        {
            _collectSound?.Play(volume: 0.8f, pitch: 0f, pan: 0f);
        }

        if (_smokeSettings is not null && (_updateFrames == 1 || _updateFrames == 90 || _updateFrames == 140))
        {
            Console.WriteLine($"Smoke: frame={_updateFrames}, phase={_demo.Phase}, score={_demo.Score}, player={_demo.PlayerCenter}.");
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
        GraphicsDevice.Clear(new Color(18, 21, 28));

        if (_spriteBatch is null || _pixel is null || _font is null)
        {
            return;
        }

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        DrawArena();

        foreach (var hazard in _demo.Hazards)
        {
            DrawFilledCircle(hazard.Center, hazard.Radius, new Color(230, 80, 75));
        }

        foreach (var pickup in _demo.Collectibles)
        {
            if (!pickup.Collected)
            {
                DrawFilledCircle(pickup.Center, pickup.Radius, new Color(250, 210, 80));
            }
        }

        DrawFilledCircle(_demo.PlayerCenter, DemoState.PlayerRadius, new Color(80, 210, 150));
        DrawHud();
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private DemoInput ReadLiveInput(KeyboardState keyboard)
    {
        _startEdge.Update(keyboard.IsKeyDown(Keys.Enter));
        _restartEdge.Update(keyboard.IsKeyDown(Keys.R) || keyboard.IsKeyDown(Keys.Enter));

        var axis = Vector2.Zero;
        if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        {
            axis.X -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        {
            axis.X += 1f;
        }
        if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
        {
            axis.Y -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
        {
            axis.Y += 1f;
        }

        return new DemoInput(axis, _startEdge.PressedThisFrame, _restartEdge.PressedThisFrame);
    }

    private void LogResult(DemoUpdateResult result)
    {
        if (result.Started)
        {
            Console.WriteLine("Phase: started.");
        }
        if (result.Restarted)
        {
            Console.WriteLine("Phase: restarted.");
        }
        if (result.Collected)
        {
            Console.WriteLine($"Collect: score={_demo.Score}/{DemoState.TargetScore}.");
        }
        if (result.Won)
        {
            Console.WriteLine("Phase: won.");
        }
        if (result.Lost)
        {
            Console.WriteLine("Phase: lost.");
        }
    }

    private void DrawArena()
    {
        DrawRect(new Rectangle(40, 70, 840, 360), new Color(34, 40, 50));
        DrawRect(new Rectangle(40, 70, 840, 4), new Color(90, 105, 122));
        DrawRect(new Rectangle(40, 426, 840, 4), new Color(90, 105, 122));
        DrawRect(new Rectangle(40, 70, 4, 360), new Color(90, 105, 122));
        DrawRect(new Rectangle(876, 70, 4, 360), new Color(90, 105, 122));
    }

    private void DrawHud()
    {
        var phaseText = _demo.Phase switch
        {
            DemoPhase.Start => "MICRO COLLECTOR - Press Enter",
            DemoPhase.Won => "YOU WIN - Press Enter or R",
            DemoPhase.Lost => "YOU LOST - Press Enter or R",
            _ => $"Score {_demo.Score}/{DemoState.TargetScore} - Collect yellow, avoid red"
        };

        _spriteBatch?.DrawString(_font, phaseText, new Vector2(52, 28), Color.White);
        _spriteBatch?.DrawString(_font, "Move: WASD/arrows   Quit: Escape", new Vector2(52, 456), new Color(190, 205, 220));
    }

    private void DrawFilledCircle(Vector2 center, float radius, Color color)
    {
        var diameter = (int)(radius * 2f);
        DrawRect(new Rectangle((int)(center.X - radius), (int)(center.Y - radius), diameter, diameter), color);
    }

    private void DrawRect(Rectangle rectangle, Color color)
    {
        _spriteBatch?.Draw(_pixel, rectangle, color);
    }

    private void ApplyWindowTitle()
    {
        Window.Title = $"Integrated Demo - {_demo.Phase} - score {_demo.Score}/{DemoState.TargetScore}";
    }
}
