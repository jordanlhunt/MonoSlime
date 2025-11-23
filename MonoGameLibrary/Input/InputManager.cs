using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class InputManager
{
    #region Properties

    public KeyboardInputInfo Keyboard { get; private set; }
    public MouseInputInfo Mouse { get; private set; }
    public GamePadInputInfo[] GamePads { get; private set; }

    #endregion

    #region Constructor

    public InputManager()
    {
        Keyboard = new KeyboardInputInfo();
        Mouse = new MouseInputInfo();
        GamePads = new GamePadInputInfo[4];
        for (int i = 0; i < GamePads.Length; i++)
        {
            GamePads[i] = new GamePadInputInfo((PlayerIndex)i);
        }
    }

    #endregion

    #region Public Methods

    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();
        for (int i = 0; i < GamePads.Length; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }

    #endregion
}
