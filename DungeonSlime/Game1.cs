using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime;

public class Game1 : Core
{
    #region Static Classes

    public static class TextureAtlasConstants
    {
        public const int SLIME_TEXTURE_WIDTH = 20;
        public const int SLIME_TEXTURE_HEIGHT = 20;
        public const int BAT_TEXTURE_WIDTH = 20;
        public const int BAT_TEXTURE_HEIGHT = 20;
        public const string ATLAS_LOCATION = "Images/atlas";
    }
    #endregion
    #region Constants
    public const int DEFAULT_WINDOW_WIDTH = 1280;
    public const int DEFAULT_WINDOW_HEIGHT = 720;
    public const string LOGO_LOCATION = "Images/NewAvatar";

    #endregion

    #region Member Variables
    private Texture2D logo;
    private TextureRegion slime;
    private TextureRegion bat;
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
        Texture2D atlasTexture = Content.Load<Texture2D>(TextureAtlasConstants.ATLAS_LOCATION);
        TextureAtlas textureAtlas = new TextureAtlas(atlasTexture);
        textureAtlas.AddRegion(
            "slime",
            0,
            0,
            TextureAtlasConstants.SLIME_TEXTURE_WIDTH,
            TextureAtlasConstants.SLIME_TEXTURE_HEIGHT
        );
        textureAtlas.AddRegion(
            "bat",
            20,
            0,
            TextureAtlasConstants.SLIME_TEXTURE_WIDTH,
            TextureAtlasConstants.SLIME_TEXTURE_HEIGHT
        );
        slime = textureAtlas.GetRegion("slime");
        bat = textureAtlas.GetRegion("bat");
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
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        slime.Draw(
            SpriteBatch,
            Vector2.Zero,
            Color.White,
            0.0f,
            Vector2.One,
            4.0f,
            SpriteEffects.None,
            0.0f
        );
        bat.Draw(
            SpriteBatch,
            new Vector2(slime.Width * 4.0f + 10, 0),
            Color.White,
            0.0f,
            Vector2.One,
            4.0f,
            SpriteEffects.None,
            1.0f
        );

        SpriteBatch.End();
        base.Draw(gameTime);
    }
}
