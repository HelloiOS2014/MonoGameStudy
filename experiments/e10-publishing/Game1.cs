using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E10Publishing;

public class Game1 : Game
{
    private readonly PublishSmokeSettings? _smokeSettings = PublishSmokeSettings.FromEnvironment();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixel;
    private int _updateFrames;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 720;
        _graphics.PreferredBackBufferHeight = 405;
        _graphics.ApplyChanges();
        Window.Title = $"E10 Publishing - {PublishMetadata.RuntimeIdentifier}";
        Console.WriteLine($"Initialize: publishing smoke app for {PublishMetadata.RuntimeIdentifier}. Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        Console.WriteLine("LoadContent: runtime pixel texture ready.");
    }

    protected override void Update(GameTime gameTime)
    {
        _updateFrames++;
        var keyboard = Keyboard.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (_updateFrames == 30)
        {
            Console.WriteLine("Smoke: rendered 30 update frames.");
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
        GraphicsDevice.Clear(new Color(18, 26, 32));

        if (_spriteBatch is null || _pixel is null)
        {
            return;
        }

        var pulse = 80 + _updateFrames % 80;
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);
        _spriteBatch.Draw(_pixel, new Rectangle(120, 120, 480, 120), new Color(70, 160, 210));
        _spriteBatch.Draw(_pixel, new Rectangle(120, 270, pulse * 4, 24), new Color(240, 190, 70));
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
