using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
    }

    protected override void Initialize()
    {
        base.Initialize();
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }
    #endregion
}
