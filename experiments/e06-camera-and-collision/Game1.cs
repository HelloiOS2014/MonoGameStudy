using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace E06CameraAndCollision;

public class Game1 : Game
{
    private const float ShapeSpeed = 160f;

    private readonly Camera2D _camera = new(new Vector2(220, 240), zoom: 1f);
    private readonly CollisionScene _scene = CollisionScene.CreateDefault();
    private readonly CameraCollisionSmokeSettings? _smokeSettings = CameraCollisionSmokeSettings.FromEnvironment();
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private Texture2D? _pixel;
    private CollisionFlags _lastFlags;
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
        _lastFlags = _scene.Evaluate();
        ApplyWindowTitle(_lastFlags);
        Console.WriteLine("Initialize: camera + collision experiment. Arrows pan camera; Q/E zoom; WASD moves shapes; Escape exits.");
        if (_smokeSettings is not null)
        {
            Console.WriteLine($"Smoke: pan frames 1-40, zoom frames 60-90, move shapes frames 100-125, exit at frame {_smokeSettings.ExitAfterFrames}.");
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        Console.WriteLine("LoadContent: created runtime pixel texture for outline drawing.");
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
        var input = _smokeSettings?.InputForFrame(_updateFrames) ?? ReadLiveInput(keyboard, elapsedSeconds);
        _camera.Apply(input.Camera, elapsedSeconds);
        _scene.MoveShapes(input.ShapeDelta);

        var flags = _scene.Evaluate();
        if (flags != _lastFlags)
        {
            Console.WriteLine($"Collision: aabb={flags.AabbHit}, circle={flags.CircleHit}.");
            _lastFlags = flags;
        }

        if (_smokeSettings is not null && (_updateFrames == 40 || _updateFrames == 90 || _updateFrames == 130))
        {
            Console.WriteLine($"Smoke: frame={_updateFrames}, camera={_camera.Position}, zoom={_camera.Zoom:0.00}, aabb={flags.AabbHit}, circle={flags.CircleHit}.");
        }

        ApplyWindowTitle(flags);

        if (_smokeSettings?.ShouldExit(_updateFrames) == true)
        {
            Console.WriteLine("Smoke: exit.");
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(16, 19, 24));

        if (_spriteBatch is null || _pixel is null)
        {
            return;
        }

        var flags = _scene.Evaluate();
        var view = _camera.GetViewMatrix(new Point(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: view);
        DrawWorldGrid();
        DrawRectangleOutline(_scene.StaticAabb, new Color(120, 150, 220), thickness: 4);
        DrawRectangleOutline(_scene.MovingAabb, flags.AabbHit ? Color.Red : new Color(80, 230, 140), thickness: 4);
        DrawCircleOutline(_scene.StaticCircle, new Color(120, 150, 220), thickness: 4);
        DrawCircleOutline(_scene.MovingCircle, flags.CircleHit ? Color.Red : new Color(80, 230, 140), thickness: 4);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private static CameraCollisionInput ReadLiveInput(KeyboardState keyboard, float elapsedSeconds)
    {
        var pan = Vector2.Zero;
        if (keyboard.IsKeyDown(Keys.Left))
        {
            pan.X -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.Right))
        {
            pan.X += 1f;
        }
        if (keyboard.IsKeyDown(Keys.Up))
        {
            pan.Y -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.Down))
        {
            pan.Y += 1f;
        }

        var zoom = 0f;
        if (keyboard.IsKeyDown(Keys.E))
        {
            zoom += 1f;
        }
        if (keyboard.IsKeyDown(Keys.Q))
        {
            zoom -= 1f;
        }

        var shapeAxis = Vector2.Zero;
        if (keyboard.IsKeyDown(Keys.A))
        {
            shapeAxis.X -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.D))
        {
            shapeAxis.X += 1f;
        }
        if (keyboard.IsKeyDown(Keys.W))
        {
            shapeAxis.Y -= 1f;
        }
        if (keyboard.IsKeyDown(Keys.S))
        {
            shapeAxis.Y += 1f;
        }

        return new CameraCollisionInput(new CameraInput(pan, zoom), shapeAxis * ShapeSpeed * elapsedSeconds);
    }

    private void DrawWorldGrid()
    {
        for (var x = -200; x <= 700; x += 100)
        {
            DrawLine(new Vector2(x, -100), new Vector2(x, 520), new Color(45, 52, 64), 2f);
        }

        for (var y = -100; y <= 520; y += 100)
        {
            DrawLine(new Vector2(-200, y), new Vector2(700, y), new Color(45, 52, 64), 2f);
        }
    }

    private void DrawRectangleOutline(Rectangle rectangle, Color color, int thickness)
    {
        DrawLine(new Vector2(rectangle.Left, rectangle.Top), new Vector2(rectangle.Right, rectangle.Top), color, thickness);
        DrawLine(new Vector2(rectangle.Right, rectangle.Top), new Vector2(rectangle.Right, rectangle.Bottom), color, thickness);
        DrawLine(new Vector2(rectangle.Right, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Bottom), color, thickness);
        DrawLine(new Vector2(rectangle.Left, rectangle.Bottom), new Vector2(rectangle.Left, rectangle.Top), color, thickness);
    }

    private void DrawCircleOutline(Circle circle, Color color, int thickness)
    {
        const int Segments = 48;
        var previous = circle.Center + new Vector2(circle.Radius, 0f);
        for (var i = 1; i <= Segments; i++)
        {
            var angle = MathHelper.TwoPi * i / Segments;
            var current = circle.Center + new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * circle.Radius;
            DrawLine(previous, current, color, thickness);
            previous = current;
        }
    }

    private void DrawLine(Vector2 start, Vector2 end, Color color, float thickness)
    {
        if (_spriteBatch is null || _pixel is null)
        {
            return;
        }

        var delta = end - start;
        _spriteBatch.Draw(
            _pixel,
            start,
            sourceRectangle: null,
            color,
            rotation: MathF.Atan2(delta.Y, delta.X),
            origin: Vector2.Zero,
            scale: new Vector2(delta.Length(), thickness),
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }

    private void ApplyWindowTitle(CollisionFlags flags)
    {
        Window.Title = $"E06 Camera/Collision - zoom {_camera.Zoom:0.00} - AABB {flags.AabbHit} - Circle {flags.CircleHit}";
    }
}
