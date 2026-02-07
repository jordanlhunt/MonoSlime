using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class KeyboardInputInfo
{
    #region Properties

    public KeyboardState PreviousState { get; private set; }
    public KeyboardState CurrentState { get; private set; }

    #endregion

    #region Constructor

    public KeyboardInputInfo()
    {
        PreviousState = new KeyboardState();
        CurrentState = PreviousState;
    }

    #endregion

    #region Public Methods

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    public bool WasKeyJustPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    public bool WasKeyJustReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }

    #endregion
}