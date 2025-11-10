using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Audio;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;

namespace MonoGameLibrary;

public class Core : Game
{
    #region Member Variables
    internal static Core staticInstance;
    private static Scene staticActiveScene;
    private static Scene staticNextScene;
    #endregion
    #region Properties
    public static Core Instance
    {
        get { return staticInstance; }
    }
    public static GraphicsDeviceManager Graphics { get; private set; }
    public static new GraphicsDevice GraphicsDevice { get; private set; }
    public static SpriteBatch SpriteBatch { get; private set; }
    public static new ContentManager Content { get; private set; }
    public static InputManager Input { get; private set; }
    public static bool ExitOnEscape { get; set; }

    /// <summary>
    /// Highlight
    /// </summary>
    public static AudioController Audio { get; private set; }
    #endregion
    #region Constructor
    public Core(string title, int width, int height, bool isFullScreen)
    {
        if (staticInstance != null)
        {
            throw new InvalidOperationException(
                $"[ERROR] - Only a single Core instance can be created"
            );
        }
        staticInstance = this;
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = isFullScreen;
        Graphics.ApplyChanges();
        Window.Title = title;
        Content = base.Content;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ExitOnEscape = true;
    }
    #endregion

    #region Public Methods
    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Input = new InputManager();
        // Added
        Audio = new AudioController();
    }

    // Add
    protected override void UnloadContent()
    {
        Audio.Dispose();
        base.UnloadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);
        Audio.Update();
        if (ExitOnEscape && Input.Keyboard.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        if (staticNextScene != null)
        {
            TransitionScene();
        }

        if (staticActiveScene != null)
        {
            staticActiveScene.Update(gameTime);
        }
        base.Update(gameTime);
    }

    public static void ChangeScene(Scene nextScene)
    {
        if (staticActiveScene != nextScene)
        {
            staticNextScene = nextScene;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        if (staticActiveScene != null)
        {
            staticActiveScene.Draw(gameTime);
        }
        base.Draw(gameTime);
    }

    private static void TransitionScene()
    {
        if (staticActiveScene != null)
        {
            staticActiveScene.Dispose();
        }
        GC.Collect();
        staticActiveScene = staticNextScene;
        staticNextScene = null;
        if (staticActiveScene != null)
        {
            staticActiveScene.Initialize();
        }
    }
    #endregion
}
