using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Scenes;

public abstract class Scene : IDisposable
{
    #region Constructor

    public Scene()
    {
        ContentManager = new ContentManager(Core.Content.ServiceProvider);
        ContentManager.RootDirectory = Core.Content.RootDirectory;
    }

    #endregion

    #region Finalizer

    ~Scene()
    {
        Dispose(false);
    }

    #endregion

    #region Properties

    protected ContentManager ContentManager { get; }
    protected bool IsDisposed { get; private set; }

    #endregion

    #region Public Methods

    public virtual void Initialize()
    {
        LoadContent();
    }

    public virtual void LoadContent() { }

    public virtual void UnloadContent()
    {
        ContentManager.Unload();
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(GameTime gameTime) { }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool isBeingDisposed)
    {
        if (IsDisposed)
        {
            return;
        }

        if (isBeingDisposed)
        {
            UnloadContent();
            ContentManager.Dispose();
        }

        IsDisposed = true;
    }

    #endregion
}
