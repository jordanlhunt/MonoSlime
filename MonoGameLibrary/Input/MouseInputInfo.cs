using Microsoft.Xna.Framework;
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

    /// <summary>
    /// Gets a value that indicates if the mouse cursor moved between the previous and current frames.
    /// </summary>
    public bool WasMouseMoved
    {
        get { return PositionDelta != Point.Zero; }
    }
    #endregion
}
