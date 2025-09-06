using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    #region Constants
    const int DEFAULT_WINDOW_WIDTH = 1280;
    const int DEFAULT_WINDOW_HEIGHT = 720;
    const string LOGO_LOCATION = "Images/NewAvatar";
    #endregion

    #region Member Variables
    private Texture2D logo;
    #endregion
    public Game1()
        : base("Dungeon Slime", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, false) { }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        logo = Content.Load<Texture2D>(LOGO_LOCATION);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MonoGameOrange);
        SpriteBatch.Begin();
        SpriteBatch.Draw(
            logo,
            new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height) / 2,
            null,
            Color.White * .3f,
            0.0f,
            new Vector2(logo.Width, logo.Height) / 2.0f,
            1.0f,
            SpriteEffects.None,
            0.0f
        );
        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
