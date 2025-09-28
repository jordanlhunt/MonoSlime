using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Input;

namespace MonoGameLibrary;

public class Core : Game
{
    #region Member Variables
    internal static Core staticInstance;
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

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        Input = new InputManager();
    }

    protected override void Update(GameTime gameTime)
    {
        Input.Update(gameTime);
        base.Update(gameTime);
    }
    #endregion
}
