using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E03Input;

public class Game1 : Game
{
    private const float PlayerSpeed = 240f;
    private const int PlayerSize = 32;

    private GraphicsDeviceManager _graphics;
    private readonly ButtonEdgeState _spaceEdge = new();
    private readonly InputSmokeSettings? _smokeSettings = InputSmokeSettings.FromEnvironment();
    private SpriteBatch? _spriteBatch;
    private Texture2D? _playerTexture;
    private Vector2 _playerPosition = new(464, 254);
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
        ApplyWindowTitle("idle");
        Console.WriteLine("Initialize: input polling experiment. Use WASD/arrows, left mouse, optional gamepad left stick, Space edge logs, Escape to exit.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: keyboard frames 1-40, mouse frames 41-80, gamepad-style frames 81-120, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _playerTexture = new Texture2D(GraphicsDevice, 1, 1);
        _playerTexture.SetData(new[] { new Color(80, 220, 160) });
        Console.WriteLine("LoadContent: created 1x1 player texture.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        var mouse = Mouse.GetState();
        var gamePad = GamePad.GetState(PlayerIndex.One);

        if (gamePad.Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
            Exit();

        _spaceEdge.Update(keyboard.IsKeyDown(Keys.Space));
        if (_spaceEdge.PressedThisFrame)
        {
            Console.WriteLine("Input: Space pressed this frame.");
        }
        else if (_spaceEdge.ReleasedThisFrame)
        {
            Console.WriteLine("Input: Space released this frame.");
        }

        var snapshot = _smokeSettings?.SnapshotForFrame(_updateFrames) ?? ReadLiveInput(keyboard, mouse, gamePad);
        _playerPosition = PlayerMotion.Apply(_playerPosition, snapshot, PlayerSpeed, (float)gameTime.ElapsedGameTime.TotalSeconds);
        _playerPosition = Vector2.Clamp(_playerPosition, Vector2.Zero, new Vector2(_graphics.PreferredBackBufferWidth - PlayerSize, _graphics.PreferredBackBufferHeight - PlayerSize));
        ApplyWindowTitle(DescribeSnapshot(snapshot));

        if (_smokeSettings is not null && (_updateFrames == 40 || _updateFrames == 80 || _updateFrames == 120))
        {
            Console.WriteLine($"Smoke: frame={_updateFrames}, position={_playerPosition}, input={DescribeSnapshot(snapshot)}.");
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
        GraphicsDevice.Clear(new Color(22, 26, 34));

        if (_spriteBatch is null || _playerTexture is null)
        {
            return;
        }

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        _spriteBatch.Draw(_playerTexture, new Rectangle((int)_playerPosition.X, (int)_playerPosition.Y, PlayerSize, PlayerSize), Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private static InputSnapshot ReadLiveInput(KeyboardState keyboard, MouseState mouse, GamePadState gamePad)
    {
        var axis = Vector2.Zero;
        if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
        {
            axis.X -= 1;
        }
        if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
        {
            axis.X += 1;
        }
        if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
        {
            axis.Y -= 1;
        }
        if (keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S))
        {
            axis.Y += 1;
        }

        if (gamePad.IsConnected)
        {
            axis += new Vector2(gamePad.ThumbSticks.Left.X, -gamePad.ThumbSticks.Left.Y);
        }

        var mouseTarget = mouse.LeftButton == ButtonState.Pressed
            ? new Vector2(mouse.X - PlayerSize / 2f, mouse.Y - PlayerSize / 2f)
            : (Vector2?)null;

        return new InputSnapshot(axis, mouseTarget);
    }

    private static string DescribeSnapshot(InputSnapshot snapshot)
    {
        if (snapshot.MouseTarget is not null)
        {
            return "mouse";
        }

        return snapshot.MoveAxis == Vector2.Zero ? "idle" : "axis";
    }

    private void ApplyWindowTitle(string inputSource)
    {
        Window.Title = $"E03 Input - {inputSource} - ({_playerPosition.X:0}, {_playerPosition.Y:0})";
    }
}
