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

    public Vector2 LeftThumbStick
    {
        get { return CurrentState.ThumbSticks.Left; }
    }

    public Vector2 RightThumbStick
    {
        get { return CurrentState.ThumbSticks.Right; }
    }

    public float LeftTrigger
    {
        get { return CurrentState.Triggers.Left; }
    }

    public float RightTrigger
    {
        get { return CurrentState.Triggers.Right; }
    }

    #endregion

    #region Constructor

    public GamePadInputInfo(PlayerIndex playerIndex)
    {
        PlayerIndex = playerIndex;
        PreviousState = new GamePadState();
        CurrentState = PreviousState;
    }

    #endregion

    #region Public Methods

    public void Update(GameTime gameTime)
    {
        PreviousState = CurrentState;
        CurrentState = GamePad.GetState(PlayerIndex);
        if (vibrationTimeRemaining > TimeSpan.Zero)
        {
            vibrationTimeRemaining -= gameTime.ElapsedGameTime;
            if (vibrationTimeRemaining <= TimeSpan.Zero)
            {
                StopVibration();
            }
        }
    }

    public bool IsButtonDown(Buttons button)
    {
        return CurrentState.IsButtonDown(button);
    }

    public bool IsButtonUp(Buttons button)
    {
        return CurrentState.IsButtonUp(button);
    }

    public bool WasButtonJustPressed(Buttons button)
    {
        return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
    }

    public bool WasButtonJustReleased(Buttons button)
    {
        return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
    }

    public void SetVibration(float strength, TimeSpan time)
    {
        vibrationTimeRemaining = time;
        GamePad.SetVibration(PlayerIndex, strength, strength);
    }

    public void StopVibration()
    {
        GamePad.SetVibration(PlayerIndex, 0.0f, 0.0f);
    }

    #endregion
}