using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime;

public class Game1 : Core
{
    #region Static Classes
    public static class Game1Constants
    {
        public const float BAT_SCALE_FACTOR = 4.0f;
    }
    #endregion
    #region Constants
    public const int DEFAULT_WINDOW_WIDTH = 1280;
    public const int DEFAULT_WINDOW_HEIGHT = 720;
    public const string LOGO_LOCATION = "Images/NewAvatar";
    public const string ATLAS_LOCATION = "Images/atlas";
    public const string ATLAS_DEFINITION_LOCATION = "Images/atlas-definition.xml";
    #endregion
    #region Member Variables
    private Texture2D logo;
    private Sprite slime;
    private Sprite bat;
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
        slime = textureAtlas.CreateSprite("slime");
        slime.Scale = new Vector2(4.0f, 4.0f);
        bat = textureAtlas.CreateSprite("bat");
        bat.Scale = new Vector2(4.0f, 4.0f);
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
        slime.Draw(SpriteBatch, Vector2.One);
        bat.Draw(SpriteBatch, new Vector2(slime.Width + 15, 0));
        SpriteBatch.End();
        base.Draw(gameTime);
    }
    #endregion
}
