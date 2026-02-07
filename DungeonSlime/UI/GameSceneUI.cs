using System;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Managers;
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

    #endregion

    #region Fields

    private static readonly string scoreFormat = "SCORE: {0:D6}";
    private SoundEffect uiSoundEffect;
    private Panel pausePanel;
    private Panel gameOverPanel;
    private AnimatedButton resumeButton;
    private AnimatedButton retyButton;
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
        pausePanel = CreatePausePanel();
        AddChild(pausePanel.Visual);
        // Create the Game Over Panel that is displayed when a game over occurs and add it as a child to this container
        gameOverPanel = CreateGameOver();
        AddChild(gameOverPanel.Visual);
    }

    #endregion
}
