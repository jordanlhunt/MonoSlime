using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class GamePadInputInfo
{
    #region Private Members
    private TimeSpan vibrationTimeRemaining = TimeSpan.Zero;
    #endregion
    #region Properties
    public PlayerIndex PlayerIndex { get; }
    public GamePadState PreviousState { get; private set; }
    public GamePadState CurrentState { get; private set; }
    public bool IsGamePadConnected
    {
        get { return CurrentState.IsConnected; }
    }
    #endregion
}
