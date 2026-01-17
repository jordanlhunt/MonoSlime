using System;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Graphics.Animation;
using Gum.Managers;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.GueDeriving;
using MonoGameLibrary.Graphics;

namespace DungeonSlime.UI;

public class AnimatedButton : Button
{
    #region Constants
    private const string FONT_FILE_STRING = "Fonts/04b_30.fnt";
    private const string START = "START";
    private const string UNFOCUSED_BUTTON = "unfocused-button";
    private const string FOCUSED_BUTTON_ANIMATION = "focused-button-animation";
    #endregion
    #region Constructor
    public AnimatedButton(TextureAtlas textureAtlas)
    {
        // Each Forms control has a general Visual property that
        // has properties shared by all control types. This Visual
        // type matches the Forms type. It can be cast to access controls-specific properties.
        ButtonVisual buttonVisual = (ButtonVisual)Visual;
        // Width is relative to children with extra padding, height is fixed
        buttonVisual.Height = 14f;
        buttonVisual.HeightUnits = DimensionUnitType.Absolute;
        buttonVisual.Width = 21f;
        buttonVisual.WidthUnits = DimensionUnitType.RelativeToChildren;
        // Get a reference to the nine-slice background to display the button graphics
        // A nine-slice allows the button to stretch while preserving corner appearance
        NineSliceRuntime background = buttonVisual.Background;
        background.Texture = textureAtlas.Texture;
        background.TextureAddress = TextureAddress.Custom;
        background.Color = Microsoft.Xna.Framework.Color.White;
        // texture coordinates for the background are set down below
        TextRuntime textInstance = buttonVisual.TextInstance;
        textInstance.Text = START;
        textInstance.Red = 70;
        textInstance.Green = 86;
        textInstance.Blue = 130;
        textInstance.UseCustomFont = true;
        textInstance.CustomFontFile = FONT_FILE_STRING;
        textInstance.FontScale = .25f;
        textInstance.Anchor(Gum.Wireframe.Anchor.Center);
        textInstance.Width = 0;
        textInstance.WidthUnits = DimensionUnitType.RelativeToChildren;
        // Get the texture region for the unfocused button state from the textureAtlas
        TextureRegion unfocusedTextureRegion = textureAtlas.GetRegion(UNFOCUSED_BUTTON);
        // Create an animation chain for the unfocused state with a single frame
        AnimationChain unfocusedAnimationChain = new AnimationChain();
        unfocusedAnimationChain.Name = nameof(unfocusedAnimationChain);
        AnimationFrame unfocusedAnimationFrame = new AnimationFrame
        {
            TopCoordinate = unfocusedTextureRegion.TopTextureCoordinate,
            BottomCoordinate = unfocusedTextureRegion.BottomTextureCoordinate,
            LeftCoordinate = unfocusedTextureRegion.LeftTextureCoordinate,
            RightCoordinate = unfocusedTextureRegion.RightTextureCoordinate,
            FrameLength = .3f,
            Texture = unfocusedTextureRegion.Texture,
        };
        unfocusedAnimationChain.Add(unfocusedAnimationFrame);
        // Get the multi-frame animation for the focused button state from the atlas
        Animation focusedTextureAtlasAnimation = textureAtlas.GetAnimation(
            FOCUSED_BUTTON_ANIMATION
        );
        // Create an Animation Chain for the focused state using all the frames from the atlas animation
        AnimationChain focusedAnimationChain = new AnimationChain();
        focusedAnimationChain.Name = nameof(focusedAnimationChain);
        foreach (TextureRegion textureRegion in focusedTextureAtlasAnimation.Frames)
        {
            AnimationFrame animationFrame = new AnimationFrame
            {
                TopCoordinate = textureRegion.TopTextureCoordinate,
                BottomCoordinate = textureRegion.BottomTextureCoordinate,
                LeftCoordinate = textureRegion.LeftTextureCoordinate,
                RightCoordinate = textureRegion.RightTextureCoordinate,
                FrameLength = (float)focusedTextureAtlasAnimation.TimeBetweenFrames.TotalSeconds,
                Texture = textureRegion.Texture,
            };
            focusedAnimationChain.Add(animationFrame);
        }
        // Assign both animation chains to the nine-slice background
        background.AnimationChains = new AnimationChainList
        {
            unfocusedAnimationChain,
            focusedAnimationChain,
        };
        // Reset all state to default so we don't have unexpected variable assignments
        buttonVisual.ButtonCategory.ResetAllStates();
        // Get teh enabled (default/unfocused) state
        StateSave enabledSaveState = buttonVisual.States.Enabled;
        enabledSaveState.Apply = () =>
        {
            // When enabled but not focused, use the unfocused animation
            background.CurrentChainName = unfocusedAnimationChain.Name;
        };
        // Create the focused state
        StateSave focusedStateSave = buttonVisual.States.Focused;
        focusedStateSave.Apply = () =>
        {
            // When focused, use the focused animation and enable animation playback
            background.CurrentChainName = focusedStateSave.Name;
            background.Animate = true;
        };
        // Create the highlighted+focused state (for mouse hover while focused)
        StateSave highlightStateSaveFocused = buttonVisual.States.HighlightedFocused;
        highlightStateSaveFocused.Apply = focusedStateSave.Apply;
        // Create the highlighted state for (for mouse hover) by cloning the enabled state since they appear the same
        StateSave highlightedStateSave = buttonVisual.States.Highlighted;
        highlightedStateSave.Apply = enabledSaveState.Apply;
        // Add event handlers for keyboard input
        KeyDown += HandleKeyDown;
        // Add event handler for the mouse hover focus
        buttonVisual.RollOn += HandleRollOn;
    }

    private void HandleKeyDown(object sender, KeyEventArgs keyEventArgs)
    {
        if (keyEventArgs.Key == Keys.Left)
        {
            HandleTab(TabDirection.Up, loop: true);
        }
        if (keyEventArgs.Key == Keys.Right)
        {
            HandleTab(TabDirection.Down, loop: true);
        }
    }

    private void HandleRollOn(object sender, EventArgs eventArgs)
    {
        IsFocused = true;
    }
    #endregion
}
