using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E07Animation;

public class Game1 : Game
{
    private const float MoveSpeed = 180f;
    private const float GroundY = 360f;

    private readonly CharacterStateMachine _state = new();
    private readonly AnimationController _animation = new();
    private readonly ButtonEdgeState _jumpEdge = new();
    private readonly AnimationSmokeSettings? _smokeSettings = AnimationSmokeSettings.FromEnvironment();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixel;
    private Vector2 _playerPosition = new(440f, GroundY);
    private float _jumpVisualRemaining;
    private int _updateFrames;

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
        Console.WriteLine("Initialize: animation experiment. A/D walk; Space jumps; Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: walk frames 40-90, jump frame 95, continue walking through 145, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        Console.WriteLine("LoadContent: created runtime sprite texture.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var input = _smokeSettings?.InputForFrame(_updateFrames) ?? ReadLiveInput(keyboard);
        var transition = _state.Update(input, elapsedSeconds);
        if (transition is not null)
        {
            Console.WriteLine($"Transition: {transition.Value.From} -> {transition.Value.To} at frame {_updateFrames}.");
            if (transition.Value.To == CharacterAnimationState.Jump)
            {
                _jumpVisualRemaining = CharacterStateMachine.JumpDurationSeconds;
            }
        }

        _animation.Update(_state.State, elapsedSeconds);
        _playerPosition.X = MathHelper.Clamp(
            _playerPosition.X + input.HorizontalAxis * MoveSpeed * elapsedSeconds,
            80f,
            _graphics.PreferredBackBufferWidth - 120f);

        if (_jumpVisualRemaining > 0f)
        {
            _jumpVisualRemaining = MathF.Max(0f, _jumpVisualRemaining - elapsedSeconds);
        }

        if (_smokeSettings is not null && (_updateFrames == 50 || _updateFrames == 100 || _updateFrames == 150))
        {
            Console.WriteLine($"Smoke: frame={_updateFrames}, state={_state.State}, animationFrame={_animation.CurrentFrame}, x={_playerPosition.X:0}.");
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
        GraphicsDevice.Clear(new Color(19, 22, 28));

        if (_spriteBatch is null || _pixel is null)
        {
            return;
        }

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        DrawRect(new Rectangle(0, (int)GroundY + 58, 960, 8), new Color(70, 78, 88));

        var jumpOffset = CalculateJumpOffset();
        var frame = _animation.CurrentFrame;
        var body = new Rectangle(
            (int)_playerPosition.X,
            (int)(_playerPosition.Y + jumpOffset),
            56 + frame % 2 * 6,
            72 - frame % 2 * 4);
        DrawRect(body, ColorForState(_state.State, frame));
        DrawRect(new Rectangle(body.X + 10 + frame * 3 % 24, body.Y + 50, 10, 30), new Color(24, 28, 34));
        DrawRect(new Rectangle(body.X + 34 - frame * 2 % 18, body.Y + 50, 10, 30), new Color(24, 28, 34));
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private CharacterInput ReadLiveInput(KeyboardState keyboard)
    {
        _jumpEdge.Update(keyboard.IsKeyDown(Keys.Space));

        var axis = 0f;
        if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
        {
            axis -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
        {
            axis += 1f;
        }

        return new CharacterInput(axis, _jumpEdge.PressedThisFrame);
    }

    private float CalculateJumpOffset()
    {
        if (_jumpVisualRemaining <= 0f)
        {
            return 0f;
        }

        var progress = 1f - _jumpVisualRemaining / CharacterStateMachine.JumpDurationSeconds;
        return -MathF.Sin(progress * MathHelper.Pi) * 110f;
    }

    private static Color ColorForState(CharacterAnimationState state, int frame)
    {
        return state switch
        {
            CharacterAnimationState.Walk => new Color(80, 220 - frame * 10, 150),
            CharacterAnimationState.Jump => new Color(250, 190, 70 + frame * 20),
            _ => new Color(90 + frame * 20, 140, 230)
        };
    }

    private void DrawRect(Rectangle rectangle, Color color)
    {
        _spriteBatch?.Draw(_pixel, rectangle, color);
    }

    private void ApplyWindowTitle()
    {
        Window.Title = $"E07 Animation - {_state.State} frame {_animation.CurrentFrame}";
    }
}
