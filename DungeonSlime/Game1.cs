using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Input;
using MonoGameLibrary.Shapes;

namespace DungeonSlime;

public class Game1 : Core
{
    #region Constants
    public const int DEFAULT_WINDOW_WIDTH = 1280;
    public const int DEFAULT_WINDOW_HEIGHT = 720;
    public const string LOGO_LOCATION = "Images/NewAvatar";
    public const string ATLAS_LOCATION = "Images/atlas";
    public const string ATLAS_DEFINITION_LOCATION = "Images/atlas-definition.xml";
    private const float MOVEMENT_SPEED = 5.0f;
    #endregion
    #region Member Variables
    private AnimatedSprite slime;
    private Vector2 slimePosition;
    private Vector2 batPosition;
    private Vector2 batVelocity;
    private AnimatedSprite bat;
    private Rectangle screenBounds;
    private Circle slimeBoundsCircle;
    private Circle batBoundsCircle;
    #endregion
    #region Constructor
    public Game1()
        : base("Dungeon Slime", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, false) { }
    #endregion
    #region Public Methods
    protected override void Initialize()
    {
        base.Initialize();
        AssignRandomBatVelocity();
        screenBounds = new Rectangle(
            0,
            0,
            GraphicsDevice.PresentationParameters.BackBufferWidth,
            GraphicsDevice.PresentationParameters.BackBufferHeight
        );
    }

    protected override void LoadContent()
    {
        TextureAtlas textureAtlas = TextureAtlas.FromFile(Content, ATLAS_DEFINITION_LOCATION);
        slime = textureAtlas.CreateAnimatedSprite("slime-animation");
        slime.Scale = new Vector2(4.0f, 4.0f);
        bat = textureAtlas.CreateAnimatedSprite("bat-animation");
        bat.Scale = new Vector2(4.0f, 4.0f);
        batPosition = new Vector2(slime.Width + 10, 0);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
        {
            Exit();
        }
        slime.Update(gameTime);
        bat.Update(gameTime);
        HandleKeyboardInput();
        HandleGamePadInput();
        HandleSlimeBounds();
        Vector2 newBatPosition = batPosition + batVelocity;
        HandleBatBounds(newBatPosition);
        batPosition = newBatPosition;
        HandleSlimeBatIntersection();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        slime.Draw(SpriteBatch, slimePosition);
        bat.Draw(SpriteBatch, batPosition);
        SpriteBatch.End();
        base.Draw(gameTime);
    }
    #endregion
    #region Private Methods
    private void HandleKeyboardInput()
    {
        float speed = MOVEMENT_SPEED;
        if (Input.Keyboard.IsKeyDown(Keys.Space))
        {
            speed *= 1.75f;
        }
        if (Input.Keyboard.IsKeyDown(Keys.W) || Input.Keyboard.IsKeyDown(Keys.Up))
        {
            slimePosition.Y -= speed;
        }
        if (Input.Keyboard.IsKeyDown(Keys.S) || Input.Keyboard.IsKeyDown(Keys.Down))
        {
            slimePosition.Y += speed;
        }
        if (Input.Keyboard.IsKeyDown(Keys.D) || Input.Keyboard.IsKeyDown(Keys.Right))
        {
            slimePosition.X += speed;
        }
        if (Input.Keyboard.IsKeyDown(Keys.A) || Input.Keyboard.IsKeyDown(Keys.Left))
        {
            slimePosition.X -= speed;
        }
    }

    private void HandleGamePadInput()
    {
        GamePadInputInfo gamePadZero = Input.GamePads[(int)PlayerIndex.One];
        float speed = MOVEMENT_SPEED;
        if (gamePadZero.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }
        if (gamePadZero.LeftThumbStick != Vector2.Zero)
        {
            slimePosition.X += gamePadZero.LeftThumbStick.X * speed;
            slimePosition.Y -= gamePadZero.LeftThumbStick.Y * speed;
        }
        else
        {
            if (gamePadZero.IsButtonDown(Buttons.DPadUp))
            {
                slimePosition.Y -= speed;
            }
            if (gamePadZero.IsButtonDown(Buttons.DPadDown))
            {
                slimePosition.Y += speed;
            }
            if (gamePadZero.IsButtonDown(Buttons.DPadLeft))
            {
                slimePosition.X -= speed;
            }
            if (gamePadZero.IsButtonDown(Buttons.DPadRight))
            {
                slimePosition.X += speed;
            }
        }
    }

    private void HandleSlimeBounds()
    {
        slimeBoundsCircle = new Circle(
            (int)(slimePosition.X + (slime.Width * 0.5f)),
            (int)(slimePosition.Y + (slime.Height * .5f)),
            (int)(slime.Width * .5f)
        );
        if (slimeBoundsCircle.Left < screenBounds.Left)
        {
            slimePosition.X = screenBounds.Left;
        }
        else if (slimeBoundsCircle.Right > screenBounds.Right)
        {
            slimePosition.X = screenBounds.X - slime.Width;
        }
        if (slimeBoundsCircle.Top < slimeBoundsCircle.Top)
        {
            slimePosition.Y = screenBounds.Top;
        }
        else if (slimeBoundsCircle.Bottom > screenBounds.Bottom)
        {
            slimePosition.Y = screenBounds.Bottom - slime.Height;
        }
    }

    private void HandleBatBounds(Vector2 newBatPosition)
    {
        batBoundsCircle = new Circle(
            (int)(newBatPosition.X + (bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (bat.Height * .5f)),
            (int)(bat.Width * 0.5f)
        );
        Vector2 normal = Vector2.Zero;
        if (batBoundsCircle.Left < screenBounds.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Left;
        }
        else if (batBoundsCircle.Right > screenBounds.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = screenBounds.Right - bat.Width;
        }
        if (batBoundsCircle.Top < screenBounds.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Top;
        }
        else if (batBoundsCircle.Bottom > screenBounds.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = screenBounds.Bottom - bat.Height;
        }
        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            batVelocity = Vector2.Reflect(batVelocity, normal);
        }
    }

    private void HandleSlimeBatIntersection()
    {
        if (slimeBoundsCircle.IsIntersecting(batBoundsCircle))
        {
            int totalColumns =
                GraphicsDevice.PresentationParameters.BackBufferWidth / (int)bat.Width;
            int totalRows =
                GraphicsDevice.PresentationParameters.BackBufferHeight / (int)bat.Height;
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);
            batPosition = new Vector2(column * bat.Width, row * bat.Height);
            AssignRandomBatVelocity();
        }
    }

    private void AssignRandomBatVelocity()
    {
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);
        batVelocity = direction * MOVEMENT_SPEED;
    }
    #endregion
}
