using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

public class MouseInputInfo
{
    #region Properties

    public MouseState PreviousState { get; private set; }
    public MouseState CurrentState { get; private set; }

    public Point Position
    {
        get { return CurrentState.Position; }
        set { SetPosition(value.X, value.Y); }
    }

    public int X
    {
        get { return CurrentState.Position.X; }
        set { SetPosition(value, CurrentState.Y); }
    }

    public int Y
    {
        get { return CurrentState.Position.Y; }
        set { SetPosition(CurrentState.X, value); }
    }

    public Point PositionDelta
    {
        get { return CurrentState.Position - PreviousState.Position; }
    }

    public int XDelta
    {
        get { return CurrentState.Position.X - PreviousState.X; }
    }

    public int YDelta
    {
        get { return CurrentState.Y - PreviousState.Y; }
    }

    public bool WasMouseMoved
    {
        get { return PositionDelta != Point.Zero; }
    }

    public int ScrollWheel
    {
        get { return CurrentState.ScrollWheelValue; }
    }

    public int ScrollWheelDelta
    {
        get { return CurrentState.ScrollWheelValue - PreviousState.ScrollWheelValue; }
    }

    #endregion

    #region Constructor

    public MouseInputInfo()
    {
        PreviousState = new MouseState();
        CurrentState = Mouse.GetState();
    }

    #endregion

    #region Public Methods

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Mouse.GetState();
    }

    public bool IsMouseButtonDown(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public bool IsMouseButtonUp(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public bool WasMouseButtonJustPressed(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Pressed
                       && PreviousState.LeftButton == ButtonState.Released;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Pressed
                       && PreviousState.MiddleButton == ButtonState.Released;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Pressed
                       && PreviousState.RightButton == ButtonState.Released;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Pressed
                       && PreviousState.XButton1 == ButtonState.Released;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Pressed
                       && PreviousState.XButton2 == ButtonState.Released;
            default:
                return false;
        }
    }

    public bool WasMouseButtonJustReleased(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return CurrentState.LeftButton == ButtonState.Released
                       && PreviousState.LeftButton == ButtonState.Pressed;
            case MouseButton.Middle:
                return CurrentState.MiddleButton == ButtonState.Released
                       && PreviousState.MiddleButton == ButtonState.Pressed;
            case MouseButton.Right:
                return CurrentState.RightButton == ButtonState.Released
                       && PreviousState.RightButton == ButtonState.Pressed;
            case MouseButton.XButton1:
                return CurrentState.XButton1 == ButtonState.Released
                       && PreviousState.XButton1 == ButtonState.Pressed;
            case MouseButton.XButton2:
                return CurrentState.XButton2 == ButtonState.Released
                       && PreviousState.XButton2 == ButtonState.Pressed;
            default:
                return false;
        }
    }

    public void SetPosition(int x, int y)
    {
        Mouse.SetPosition(x, y);
        CurrentState = new MouseState(
            x,
            y,
            CurrentState.ScrollWheelValue,
            CurrentState.LeftButton,
            CurrentState.MiddleButton,
            CurrentState.RightButton,
            CurrentState.XButton1,
            CurrentState.XButton2
        );
    }

    #endregion
}