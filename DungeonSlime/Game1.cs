using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.Tiles;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Shapes;

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
        ChangeScene(new TitleScene());
    }

    protected override void LoadContent()
    {
        themeSong = Content.Load<Song>(ThemeSongLocation);
    }
    #endregion
}
