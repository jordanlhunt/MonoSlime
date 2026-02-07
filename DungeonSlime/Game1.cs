using DungeonSlime.Scenes;
using Gum.Forms;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework.Media;
using MonoGameGum;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    #region Constants

    private const int DefaultWindowWidth = 1280;
    private const int DefaultWindowHeight = 720;
    private const string ThemeSongLocation = "Sounds/theme";

    #endregion

    #region Member Variables

    private Song themeSong;

    #endregion

    #region Constructor

    public Game1()
        : base("Dungeon Slime", DefaultWindowWidth, DefaultWindowHeight, false) { }

    #endregion

    #region Public Methods

    protected override void Initialize()
    {
        base.Initialize();

        Audio.PlaySong(themeSong);
        InitializeGum();
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
        themeSong = Content.Load<Song>(ThemeSongLocation);
    }

    #endregion

    #region Private Methods

    public void InitializeGum()
    {
        GumService.Default.Initialize(this, DefaultVisualsVersion.V3);
        if (GumService.Default.ContentLoader != null)
        {
            GumService.Default.ContentLoader.XnaContentManager = Core.Content;
        }

        FrameworkElement.KeyboardsForUiControl.Add(GumService.Default.Keyboard);
        FrameworkElement.GamePadsForUiControl.AddRange(GumService.Default.Gamepads);
        FrameworkElement.TabReverseKeyCombos.Add(
            new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Up }
        );
        FrameworkElement.TabKeyCombos.Add(
            new KeyCombo() { PushedKey = Microsoft.Xna.Framework.Input.Keys.Down }
        );
        GumService.Default.CanvasWidth =
            GraphicsDevice.PresentationParameters.BackBufferWidth / 4.0f;
        GumService.Default.CanvasHeight =
            GraphicsDevice.PresentationParameters.BackBufferHeight / 4.0f;
        GumService.Default.Renderer.Camera.Zoom = 4.0f;
    }

    #endregion
}
