using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

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
    private AnimatedSprite bat;
    #endregion
    #region Constructor
    public Game1()
        : base("Dungeon Slime", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, false) { }
    #endregion
    #region Public Methods
    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        TextureAtlas textureAtlas = TextureAtlas.FromFile(Content, ATLAS_DEFINITION_LOCATION);
        slime = textureAtlas.CreateAnimatedSprite("slime-animation");
        slime.Scale = new Vector2(4.0f, 4.0f);
        bat = textureAtlas.CreateAnimatedSprite("bat-animation");
        bat.Scale = new Vector2(4.0f, 4.0f);
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
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        slime.Draw(SpriteBatch, slimePosition);
        bat.Draw(SpriteBatch, new Vector2(slime.Width + 15, 0));
        SpriteBatch.End();
        base.Draw(gameTime);
    }
    #endregion

    #region Private Methods
    private void HandleKeyboardInput()
    {
        KeyboardState keyboardState = Keyboard.GetState();
        float speed = MOVEMENT_SPEED;
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            speed *= 1.75f;
        }
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            slimePosition.Y -= speed;
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            slimePosition.Y += speed;
        }
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            slimePosition.X += speed;
        }
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            slimePosition.X -= speed;
        }
    }

    private void HandleGamePadInput()
    {
        GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
        float speed = MOVEMENT_SPEED;
        if (gamePadState.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }
        if (gamePadState.ThumbSticks.Left != Vector2.Zero)
        {
            slimePosition.X += gamePadState.ThumbSticks.Left.X * speed;
            slimePosition.Y -= gamePadState.ThumbSticks.Left.Y * speed;
        }
        else
        {
            if (gamePadState.IsButtonDown(Buttons.DPadUp))
            {
                slimePosition.Y -= speed;
            }
            if (gamePadState.IsButtonDown(Buttons.DPadDown))
            {
                slimePosition.Y += speed;
            }
            if (gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                slimePosition.X -= speed;
            }
            if (gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                slimePosition.X += speed;
            }
        }
    }
    #endregion
}
