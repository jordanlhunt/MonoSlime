using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MonoGameLibrary.Input;

namespace DungeonSlime;

/// <summary>
/// Provides a game-specific input abstraction that maps physical inputs
/// to game actions, bridging our input system with game-specific functionality.
/// </summary>
public static class GameController
{
    #region Properties

    private static KeyboardInputInfo Keyboard => Core.Input.Keyboard;
    private static GamePadInputInfo Player1GamePad => Core.Input.GamePads[(int)PlayerIndex.One];

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns true if the player has triggered the "move up" action.
    /// </summary>
    public static bool MoveUp()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Up)
            || Keyboard.WasKeyJustPressed(Keys.W)
            || Player1GamePad.WasButtonJustPressed(Buttons.DPadUp)
            || Player1GamePad.WasButtonJustPressed(Buttons.LeftThumbstickUp)
        );
    }

    /// <summary>
    /// Returns true if the player has triggered the "move down" action.
    /// </summary>
    public static bool MoveDown()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Down)
            || Keyboard.WasKeyJustPressed(Keys.S)
            || Player1GamePad.WasButtonJustPressed(Buttons.DPadDown)
            || Player1GamePad.WasButtonJustPressed(Buttons.LeftThumbstickDown)
        );
    }

    /// <summary>
    /// Returns true if the player has triggered the "move left" action.
    /// </summary>
    public static bool MoveLeft()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Left)
            || Keyboard.WasKeyJustPressed(Keys.A)
            || Player1GamePad.WasButtonJustPressed(Buttons.DPadLeft)
            || Player1GamePad.WasButtonJustPressed(Buttons.LeftThumbstickLeft)
        );
    }

    /// <summary>
    /// Returns true if the player has triggered the "move right" action.
    /// </summary>
    public static bool MoveRight()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Right)
            || Keyboard.WasKeyJustPressed(Keys.D)
            || Player1GamePad.WasButtonJustPressed(Buttons.DPadRight)
            || Player1GamePad.WasButtonJustPressed(Buttons.LeftThumbstickRight)
        );
    }

    /// <summary>
    /// Returns true if the player has triggered the "pause" action.
    /// </summary>
    public static bool Pause()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Escape)
            || Player1GamePad.WasButtonJustPressed(Buttons.Start)
        );
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" button,
    /// typically used for menu confirmation.
    /// </summary>
    public static bool Action()
    {
        return (
            Keyboard.WasKeyJustPressed(Keys.Enter) || Player1GamePad.WasButtonJustPressed(Buttons.A)
        );
    }

    #endregion
}
