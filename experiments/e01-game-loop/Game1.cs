using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace E01GameLoop;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private readonly GameLoopState _loopState = new();
    private readonly LoopSmokeSettings? _smokeSettings = LoopSmokeSettings.FromEnvironment();
    private int _framesThisSecond;
    private int _totalFrames;
    private int _updateFrames;
    private double _secondsSinceLastLog;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
        _graphics.PreferredBackBufferWidth = 960;
        _graphics.PreferredBackBufferHeight = 540;
        _graphics.ApplyChanges();
        ApplyLoopMode();
        Console.WriteLine("Initialize: fixed timestep at 60 Hz. Press F1 to toggle fixed/variable timestep. Press Escape to exit.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: toggle at update frame {_smokeSettings.ToggleAfterFrames}; exit at update frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        Console.WriteLine("LoadContent: no content loaded for e01.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        var smokeTogglePressed = _smokeSettings?.ShouldPressToggle(_updateFrames) == true;
        if (_loopState.Update(keyboard.IsKeyDown(Keys.F1) || smokeTogglePressed))
        {
            ApplyLoopMode();
            Console.WriteLine($"Update: timestep mode changed to {_loopState.ModeLabel}.");
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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _framesThisSecond++;
        _totalFrames++;
        _secondsSinceLastLog += gameTime.ElapsedGameTime.TotalSeconds;
        if (_secondsSinceLastLog >= 1.0)
        {
            Console.WriteLine($"Draw: totalFrames={_totalFrames}, fps={_framesThisSecond}, mode={_loopState.ModeLabel}");
            _framesThisSecond = 0;
            _secondsSinceLastLog = 0;
        }

        base.Draw(gameTime);
    }

    private void ApplyLoopMode()
    {
        IsFixedTimeStep = _loopState.IsFixedTimeStep;
        Window.Title = $"E01 Game Loop - {_loopState.ModeLabel}";
    }
}
