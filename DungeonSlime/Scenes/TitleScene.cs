using System;
using DungeonSlime.UI;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Scenes;

namespace DungeonSlime.Scenes;

public class TitleScene : Scene
{
    #region Constants

    private const string DUNGEON_TEXT = "Dungeon";
    private const string SLIME_TEXT = "Slime";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";
    private const string EQUIPMENT_PRO_LOCATION = "Fonts/EquipmentPro";
    private const string COMPASS_PRO_LOCATION = "Fonts/CompassPro";
    private const string REPEATING_BACKGROUND_LOCATION = "Images/background-pattern";
    private const string UI_SOUND_EFFECT_LOCATION = "Sounds/Click_15";
    private const string CUSTOM_FONT_FILE = "Fonts/04b_30.fnt";
    private const string ATLAS_DEFINITION_LOCATION = "Images/atlas-definition.xml";
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
    private Rectangle repeatingBackgroundPatternDestination;
    private Vector2 repeatingBackgroundPatternOffset;
    private Texture2D repeatingBackgroundPattern;
    private float repeatingBackgroundScrollSpeed = 50.0f;
    private SoundEffect uiSoundEffect;
    private Panel titleScreenButtonPanel;
    private Panel optionsPanel;
    private AnimatedButton optionsButton;
    private AnimatedButton optionsBackButton;
    private TextureAtlas textureAtlas;
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
        pressEnterOrigin = textSize * 0.5f;
        pressEnterPosition = new Vector2(PRESS_ENTER_X, PRESS_ENTER_Y);
        repeatingBackgroundPatternOffset = Vector2.Zero;
        repeatingBackgroundPatternDestination = Core.GraphicsDevice.PresentationParameters.Bounds;
        InitializeUi();
    }

    public override void LoadContent()
    {
        equipmentProFont = Core.Content.Load<SpriteFont>(EQUIPMENT_PRO_LOCATION);
        compassProSpriteFont = Core.Content.Load<SpriteFont>(COMPASS_PRO_LOCATION);
        repeatingBackgroundPattern = Core.Content.Load<Texture2D>(REPEATING_BACKGROUND_LOCATION);
        uiSoundEffect = Core.Content.Load<SoundEffect>(UI_SOUND_EFFECT_LOCATION);
        textureAtlas = TextureAtlas.FromFile(Core.Content, ATLAS_DEFINITION_LOCATION);
    }

    public override void Update(GameTime gameTime)
    {
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter))
        {
            Core.ChangeScene(new GameScene());
        }

        UpdateRepeatingBackground(gameTime);
        GumService.Default.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(new Color(32, 40, 78, 255));

        Core.SpriteBatch.Begin(samplerState: SamplerState.PointWrap);
        Core.SpriteBatch.Draw(
            repeatingBackgroundPattern,
            repeatingBackgroundPatternDestination,
            new Rectangle(
                repeatingBackgroundPatternOffset.ToPoint(),
                repeatingBackgroundPatternDestination.Size
            ),
            Color.White * 0.5f
        );
        Core.SpriteBatch.End();
        if (titleScreenButtonPanel.IsVisible)
        {
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

        GumService.Default.Draw();
    }

    #endregion

    #region Private Methods

    void UpdateRepeatingBackground(GameTime gameTime)
    {
        float wrappingOffset =
            repeatingBackgroundScrollSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        repeatingBackgroundPatternOffset.X -= wrappingOffset;
        repeatingBackgroundPatternOffset.Y -= wrappingOffset;
        repeatingBackgroundPatternOffset.X %= repeatingBackgroundPattern.Width;
        repeatingBackgroundPatternOffset.Y %= repeatingBackgroundPattern.Height;
    }

    private void CreateTitlePanel()
    {
        titleScreenButtonPanel = new Panel();
        titleScreenButtonPanel.Dock(Gum.Wireframe.Dock.Fill);
        titleScreenButtonPanel.AddToRoot();

        AnimatedButton startButton = new AnimatedButton(textureAtlas);
        startButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        startButton.X = 50;
        startButton.Y = -12;
        startButton.Text = "Start";
        startButton.Click += HandleStartButtonClicked;

        optionsButton = new AnimatedButton(textureAtlas);
        optionsButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        optionsButton.X = -50;
        optionsButton.Y = -12;
        optionsButton.Text = "Options";

        optionsButton.Click += HandleOptionsButtonClicked;

        titleScreenButtonPanel.AddChild(startButton);
        titleScreenButtonPanel.AddChild(optionsButton);

        startButton.IsFocused = true;
    }

    private void CreateOptionsPanel()
    {
        optionsPanel = new Panel();
        optionsPanel.Dock(Gum.Wireframe.Dock.Fill);
        optionsPanel.IsVisible = false;
        optionsPanel.AddToRoot();

        TextRuntime optionsText = new TextRuntime();
        optionsText.X = 10;
        optionsText.Y = 10;
        optionsText.Text = "OPTIONS";
        optionsText.UseCustomFont = true;
        optionsText.CustomFontFile = CUSTOM_FONT_FILE;
        optionsText.FontScale = .5f;
        optionsPanel.AddChild(optionsText);

        OptionsSlider musicSlider = new OptionsSlider(textureAtlas);
        musicSlider.Name = "MusicSlider";
        musicSlider.Text = "MUSIC";
        musicSlider.Anchor(Gum.Wireframe.Anchor.Top);
        musicSlider.Y = 30.0f;
        musicSlider.Minimum = 0.0f;
        musicSlider.Maximum = 1.0f;
        musicSlider.Value = Core.Audio.CurrentSongVolume;
        musicSlider.SmallChange = .1;
        musicSlider.LargeChange = .2;
        musicSlider.ValueChanged += HandleMusicSliderValueChanged;
        musicSlider.ValueChangeCompleted += HandleMusicSliderValueChangeCompleted;
        optionsPanel.AddChild(musicSlider);

        OptionsSlider soundEffectsSlider = new OptionsSlider(textureAtlas);
        soundEffectsSlider.Name = "SfxSlider";
        soundEffectsSlider.Text = "SFX";
        soundEffectsSlider.Anchor(Gum.Wireframe.Anchor.Top);
        soundEffectsSlider.Y = 93;
        soundEffectsSlider.Minimum = 0.0f;
        soundEffectsSlider.Maximum = 1.0f;
        soundEffectsSlider.Value = Core.Audio.CurrentSoundEffectVolume;
        soundEffectsSlider.SmallChange = .1;
        soundEffectsSlider.LargeChange = .2;
        soundEffectsSlider.ValueChanged += HandleSoundEffectSliderValueChanged;
        soundEffectsSlider.ValueChangeCompleted += HandleSoundEffectsSliderChangeCompleted;
        optionsPanel.AddChild(soundEffectsSlider);
        optionsBackButton = new AnimatedButton(textureAtlas);
        optionsBackButton.Text = "Back";
        optionsBackButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        optionsBackButton.X = -28f;
        optionsBackButton.Y = -10f;
        optionsBackButton.Click += HandleOptionsBackButtonClicked;
        optionsPanel.AddChild(optionsBackButton);
    }

    private void HandleStartButtonClicked(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        Core.ChangeScene(new GameScene());
    }

    private void HandleMusicSliderValueChanged(object sender, EventArgs eventArgs)
    {
        Slider slider = (Slider)sender;
        Core.Audio.CurrentSongVolume = (float)slider.Value;
    }

    private void HandleMusicSliderValueChangeCompleted(object sender, EventArgs eventsArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
    }

    private void HandleSoundEffectSliderValueChanged(object sender, EventArgs eventArgs)
    {
        Slider slider = (Slider)sender;
        Core.Audio.CurrentSoundEffectVolume = (float)slider.Value;
    }

    private void HandleSoundEffectsSliderChangeCompleted(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
    }

    private void HandleOptionsButtonClicked(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        titleScreenButtonPanel.IsVisible = false;
        optionsPanel.IsVisible = true;
        optionsBackButton.IsFocused = true;
    }

    private void HandleOptionsBackButtonClicked(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        titleScreenButtonPanel.IsVisible = true;
        optionsPanel.IsVisible = false;
        optionsButton.IsFocused = true;
    }

    private void InitializeUi()
    {
        if (GumService.Default.Root.Children != null)
        {
            GumService.Default.Root.Children.Clear();
        }
        CreateTitlePanel();
        CreateOptionsPanel();
    }
    #endregion
}
