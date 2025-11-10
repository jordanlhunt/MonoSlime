using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Scenes;

public class TitleScene : Scene
{
    #region Constants
    private const string DUNGEON_TEXT = "Dungeon";
    private const string SLIME_TEXT = "Slime";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";
    private const string EQUIPMENT_PRO_LOCATION = "Fonts/EquipmentPro";
    private const string COMPASS_PRO_LOCATION = "Fonts/CompassPro";
    private const int DUNGEON_TEXT_X = 640;
    private const int DUNGEON_TEXT_Y = 100;
    private const int SLIME_TEXT_X = 755;
    private const int SLIME_TEXT_Y = 285;
    private const int PRESS_ENTER_X = 640;
    private const int PRESS_ENTER_Y = 620;
    #endregion
    #region Member Variables
    private SpriteFont equipmentProFont;
    private SpriteFont compassProSpriteFont;
    private Vector2 dungeonTextPosition;
    private Vector2 dungeonTextOrigin;
    private Vector2 slimeTextPosition;
    private Vector2 slimeTextOrigin;
    private Vector2 pressEnterPosition;
    private Vector2 pressEnterOrigin;
    #endregion

    #region Public Methods


    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = true;
        Vector2 textSize = compassProSpriteFont.MeasureString(DUNGEON_TEXT);
        dungeonTextPosition = new Vector2(DUNGEON_TEXT_X, DUNGEON_TEXT_Y);
        dungeonTextOrigin = textSize * 0.5f;
        textSize = compassProSpriteFont.MeasureString(SLIME_TEXT);
        slimeTextPosition = new Vector2(SLIME_TEXT_X, SLIME_TEXT_Y);
        slimeTextOrigin = textSize * 0.5f;
        textSize = equipmentProFont.MeasureString(PRESS_ENTER_TEXT);
        pressEnterPosition = new Vector2(PRESS_ENTER_X, PRESS_ENTER_Y);
        pressEnterOrigin = textSize * 0.5f;
    }

    public override void LoadContent()
    {
        equipmentProFont = Core.Content.Load<SpriteFont>(EQUIPMENT_PRO_LOCATION);
        compassProSpriteFont = Core.Content.Load<SpriteFont>(COMPASS_PRO_LOCATION);
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Color dropShadowColor = Color.Black * 0.5f;
        // Draw Text Twice for simple shadow effect
        Core.SpriteBatch.DrawString(
            compassProSpriteFont,
            DUNGEON_TEXT,
            dungeonTextPosition + new Vector2(10, 10),
            dropShadowColor,
            0.0f,
            dungeonTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
        Core.SpriteBatch.DrawString(
            compassProSpriteFont,
            DUNGEON_TEXT,
            dungeonTextPosition,
            Color.White,
            0.0f,
            dungeonTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
        Core.SpriteBatch.DrawString(
            compassProSpriteFont,
            SLIME_TEXT,
            slimeTextPosition + new Vector2(10, 10),
            dropShadowColor,
            0.0f,
            slimeTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
        Core.SpriteBatch.DrawString(
            compassProSpriteFont,
            SLIME_TEXT,
            slimeTextPosition,
            Color.White,
            0.0f,
            slimeTextOrigin,
            1.0f,
            SpriteEffects.None,
            1.0f
        );
        Core.SpriteBatch.DrawString(
            equipmentProFont,
            PRESS_ENTER_TEXT,
            pressEnterPosition,
            Color.White,
            0.0f,
            pressEnterOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );
        Core.SpriteBatch.End();
    }

    #endregion
}
