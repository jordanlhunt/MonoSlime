using System;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.UI;

public class GameSceneUI : ContainerRuntime
{
    #region Constants

    private const string UiSoundEffectLocation = "Sounds/Click_15";
    private const string AtlasDefinitionLocation = "Images/atlas-definition.xml";
    private const string FontFileString = "Fonts/04b_30.fnt";
    private const string AtlasPanelBackground = "panel-background";

    #endregion

    #region Fields

    private static readonly string scoreFormat = "SCORE: {0:D6}";
    private SoundEffect uiSoundEffect;
    private Panel pausePanel;
    private Panel gameOverPanel;
    private AnimatedButton resumeButton;
    private AnimatedButton retryButton;
    private TextRuntime scoreText;

    #endregion

    #region Events

    /// <summary>
    /// Event invoked when the Resume button on the Pause panel is clicked.
    /// </summary>
    public event EventHandler ResumeButtonClick;

    /// <summary>
    /// Event invoked when the Quit button on either the Pause panel or the
    /// Game Over panel is clicked.
    /// </summary>
    public event EventHandler QuitButtonClick;

    /// <summary>
    /// Event invoked when the Retry button on the Game Over panel is clicked.
    /// </summary>
    public event EventHandler RetryButtonClick;

    #endregion

    #region Constructor

    public GameSceneUI()
    {
        // The game scene UI inherits from ContainerRuntime, set its Dock to fill so it fills the entire screen
        Dock(Gum.Wireframe.Dock.Fill);
        // Add it to the root element
        this.AddToRoot();
        // Get a reference to the content manager that was registered with the GumService when it was original initialized
        ContentManager contentManager = GumService.Default.ContentLoader.XnaContentManager;
        // Use the content manager to load the sound effect and atlas for the user interface elements
        uiSoundEffect = contentManager.Load<SoundEffect>(UiSoundEffectLocation);
        TextureAtlas textureAtlas = TextureAtlas.FromFile(contentManager, AtlasDefinitionLocation);
        // Create the text that will display the player's score and add it as a child to this container
        scoreText = CreateScoreText();
        AddChild(scoreText);
        // Create the Pause Panel that is displayed when the game is paused and add it as a child to this container
        pausePanel = CreatePausePanel(textureAtlas);
        AddChild(pausePanel.Visual);
        // Create the Game Over Panel that is displayed when a game over occurs and add it as a child to this container
        gameOverPanel = CreateGameOverPanel(textureAtlas);
        AddChild(gameOverPanel.Visual);
    }

    #endregion

    #region Private Methods

    private TextRuntime CreateScoreText()
    {
        TextRuntime textRuntime = new TextRuntime();
        textRuntime.Anchor(Gum.Wireframe.Anchor.TopLeft);
        textRuntime.WidthUnits = DimensionUnitType.RelativeToChildren;
        textRuntime.X = 20.0f;
        textRuntime.Y = 5.0f;
        textRuntime.UseCustomFont = true;
        textRuntime.CustomFontFile = FontFileString;
        textRuntime.FontScale = .25f;
        textRuntime.Text = string.Format(scoreFormat, 0);
        return textRuntime;
    }

    private Panel CreatePausePanel(TextureAtlas textureAtlas)
    {
        Panel panel = new Panel();
        panel.Anchor(Gum.Wireframe.Anchor.Center);
        panel.WidthUnits = DimensionUnitType.Absolute;
        panel.HeightUnits = DimensionUnitType.Absolute;
        panel.Width = 264.0f;
        panel.Height = 70.0f;
        panel.IsVisible = false;
        TextureRegion backgroundTextureRegion = textureAtlas.GetRegion(AtlasPanelBackground);
        NineSliceRuntime backgroundNineSliceRunTime = new NineSliceRuntime();
        backgroundNineSliceRunTime.Dock(Gum.Wireframe.Dock.Fill);
        backgroundNineSliceRunTime.Texture = backgroundTextureRegion.Texture;
        backgroundNineSliceRunTime.TextureAddress = TextureAddress.Custom;
        backgroundNineSliceRunTime.TextureHeight = backgroundTextureRegion.Height;
        backgroundNineSliceRunTime.TextureWidth = backgroundTextureRegion.Width;
        backgroundNineSliceRunTime.TextureTop = backgroundTextureRegion.SourceRectangle.Top;
        backgroundNineSliceRunTime.TextureLeft = backgroundTextureRegion.SourceRectangle.Left;
        panel.AddChild(backgroundNineSliceRunTime);
        TextRuntime textRuntime = new TextRuntime();
        textRuntime.Text = "PAUSED";
        textRuntime.UseCustomFont = true;
        textRuntime.CustomFontFile = FontFileString;
        textRuntime.FontScale = .5f;
        textRuntime.X = 10.0f;
        textRuntime.Y = 10.0f;
        panel.AddChild(textRuntime);
        resumeButton = new AnimatedButton(textureAtlas);
        resumeButton.Text = "RESUME";
        resumeButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        resumeButton.X = 9.0f;
        resumeButton.Y = -9.0f;
        resumeButton.Click += OnResumeButtonClicked;
        resumeButton.GotFocus += OnElementGotFocus;
        panel.AddChild(resumeButton);
        AnimatedButton quitButton = new AnimatedButton(textureAtlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        quitButton.X = 9.0f;
        quitButton.Y = -9.0f;
        quitButton.Click += OnQuitButtonClicked;
        quitButton.GotFocus += OnElementGotFocus;
        panel.AddChild(quitButton);
        return panel;
    }

    private Panel CreateGameOverPanel(TextureAtlas textureAtlas)
    {
        Panel panel = new Panel();
        panel.Anchor(Gum.Wireframe.Anchor.Center);
        panel.WidthUnits = DimensionUnitType.Absolute;
        panel.HeightUnits = DimensionUnitType.Absolute;
        panel.Width = 264.0f;
        panel.Height = 70.0f;
        panel.IsVisible = false;
        TextureRegion backgroundRegion = textureAtlas.GetRegion(AtlasPanelBackground);
        NineSliceRuntime background = new NineSliceRuntime();
        background.Dock(Gum.Wireframe.Dock.Fill);
        background.Texture = backgroundRegion.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.TextureHeight = backgroundRegion.Height;
        background.TextureWidth = backgroundRegion.Width;
        background.TextureTop = backgroundRegion.SourceRectangle.Top;
        background.TextureLeft = backgroundRegion.SourceRectangle.Left;
        panel.AddChild(background);
        TextRuntime text = new TextRuntime();
        text.Text = "GAME OVER";
        text.WidthUnits = DimensionUnitType.RelativeToChildren;
        text.UseCustomFont = true;
        text.CustomFontFile = FontFileString;
        text.FontScale = 0.5f;
        text.X = 10.0f;
        text.Y = 10.0f;
        panel.AddChild(text);
        retryButton = new AnimatedButton(textureAtlas);
        retryButton.Text = "RETRY";
        retryButton.Anchor(Gum.Wireframe.Anchor.BottomLeft);
        retryButton.X = 9.0f;
        retryButton.Y = -9.0f;
        retryButton.Click += OnRetryButtonClicked;
        retryButton.GotFocus += OnElementGotFocus;
        panel.AddChild(retryButton);
        AnimatedButton quitButton = new AnimatedButton(textureAtlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Gum.Wireframe.Anchor.BottomRight);
        quitButton.X = -9.0f;
        quitButton.Y = -9.0f;
        quitButton.Click += OnQuitButtonClicked;
        quitButton.GotFocus += OnElementGotFocus;
        panel.AddChild(quitButton);
        return panel;
    }

    private void OnResumeButtonClicked(object sender, EventArgs eventArgs)
    {
        // Button was clicked, play the UI sound effect for auditory feedback
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        // Hide the Pause Panel
        HidePausePanel();
        // Invoke the ResumeButtonClick event
        if (ResumeButtonClick != null)
        {
            ResumeButtonClick(sender, eventArgs);
        }
    }

    private void OnRetryButtonClicked(object sender, EventArgs eventArgs)
    {
        // Button was clicked, play the UI sound effect for auditory feed
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        // Hide the GameOver Panel
        HideGameOverPanel();
        // Invoke the RetryButtonClick event
        if (RetryButtonClick != null)
        {
            RetryButtonClick(sender, eventArgs);
        }
    }

    private void OnQuitButtonClicked(object sender, EventArgs eventArgs)
    {
        // Button was clicked, play the UI sound effect for auditory feedback
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        // Hide BOTH panels
        HideGameOverPanel();
        HidePausePanel();
        // Invoke the QuitButtonClick event
        if (QuitButtonClick != null)
        {
            QuitButtonClick(sender, eventArgs);
        }
    }

    private void OnElementGotFocus(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
    }

    #endregion

    #region Public Methods

    public void UpdateScoreText(int score)
    {
        scoreText.Text = string.Format(scoreFormat, score);
    }

    public void ShowPausePanel()
    {
        pausePanel.IsVisible = true;
        resumeButton.IsFocused = true;
        gameOverPanel.IsVisible = false;
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.IsVisible = true;
        retryButton.IsFocused = true;
        pausePanel.IsVisible = false;
    }

    public void HideGameOverPanel()
    {
        gameOverPanel.IsVisible = false;
    }

    public void HidePausePanel()
    {
        pausePanel.IsVisible = false;
    }

    public void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
    }

    public void Draw()
    {
        GumService.Default.Draw();
    }

    #endregion
}
