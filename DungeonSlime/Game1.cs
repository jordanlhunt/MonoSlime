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
    #endregion
    public Game1()
        : base("Dungeon Slime", DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, false) { }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent() { }

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
        base.Draw(gameTime);
    }
}
