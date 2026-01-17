using System;
using Gum.DataTypes;
using Gum.DataTypes.Variables;
using Gum.Forms.Controls;
using Gum.Managers;
using MonoGameGum.GueDeriving;
using MonoGameLibrary.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace DungeonSlime.UI;

public class OptionsSlider : Slider
{
    #region Constants
    private const string PANEL_BACKGROUND_ATLAS_STRING = "panel-background";
    private const string CUSTOM_FONT_FILE = "fonts/04b_30.fnt";
    private const string DEFAULT_UNINITIALIZED_TEXT = "DEFAULT_UNINITIALIZED_TEXT";
    private const string OFF_BACKGROUND_ATLAS_STRING = "slider-off-background";
    private const string MIDDLE_BACKGROUND_ATLAS_STRING = "slider-middle-background";
    private const string MAX_BACKGROUND_ATLAS_STRING = "slider-max-background";
    private const float DEFAULT_FILLRECTANGLE_WIDTH = 90f;
    private const string OFF_TEXT_STRING = "OFF";
    private const string MAX_TEXT_STRING = "MAX";
    #endregion
    #region Readonly Variables
    private readonly float containerHeight = 55f;
    private readonly float containerWidth = 264f;
    private readonly float textInstanceFontScale = .5f;
    private readonly float textInstanceX = 10f;
    private readonly float textInstanceY = 10f;
    private readonly float innerContainerRuntimeHeight = 13f;
    private readonly float innerContainerRuntimeWidth = 241f;
    private readonly float innerContainerRuntimeX = 10f;
    private readonly float innerContainerRuntimeY = 33f;
    private readonly float offBackgroundTextureRegionWidth = 28f;
    private readonly float middleBackgroundX = 27f;
    private readonly float middleBackgroundWidth = 179f;
    private readonly float maxBackgroundWidth = 36f;
    private readonly string specialNameTrackInstance = "TrackInstance";
    private readonly float trackInstanceWidth = -2f;
    private readonly float trackInstanceHeight = -2f;
    private readonly int offAndMaxTextRed = 70;
    private readonly int offAndMaxTextBlue = 86;
    private readonly int offAndMaxTextGreen = 130;
    private readonly float offAndMaxTextFontScale = .5f;
    #endregion
    #region Member Variables
    // Reference to the text label that displays the slider's title
    private TextRuntime textInstance;

    // Reference to the rectangle that visually represents the current value
    private ColoredRectangleRuntime currentValueFillRectangle;

    // Define colors for focused and unfocused states
    private Color unfocusedStateColor = Color.Gray;
    private Color focusedStateColor = Color.White;
    #endregion
    #region Properties
    public string TextInstanceText
    {
        get => textInstance.Text;
        set => textInstance.Text = value;
    }
    #endregion
    #region Constructor
    /// <summary>
    /// Creates a new OptionsSlider instance using graphics from the specified texture atlas
    /// </summary>
    /// <param name="textureAtlas">The texture atlas containing slider graphics.</param>
    ///
    ///
    ///
    public OptionsSlider(TextureAtlas textureAtlas)
    {
        // Create the top-level container for all the visual elements
        ContainerRuntime topLevelContainerRuntime = new ContainerRuntime
        {
            Height = containerHeight,
            Width = containerWidth,
        };
        TextureRegion backgroundTextureRegion = textureAtlas.GetRegion(
            PANEL_BACKGROUND_ATLAS_STRING
        );
        // Create the background panel that contains everything
        NineSliceRuntime background = new NineSliceRuntime
        {
            Texture = textureAtlas.Texture,
            TextureAddress = TextureAddress.Custom,
            TextureHeight = backgroundTextureRegion.Height,
            TextureLeft = backgroundTextureRegion.SourceRectangle.Left,
            TextureTop = backgroundTextureRegion.SourceRectangle.Top,
            TextureWidth = backgroundTextureRegion.Width,
        };
        background.Dock(Gum.Wireframe.Dock.Fill);
        topLevelContainerRuntime.AddChild(background);
        // Create the title text element
        textInstance = new TextRuntime
        {
            CustomFontFile = CUSTOM_FONT_FILE,
            UseCustomFont = true,
            FontScale = textInstanceFontScale,
            Text = DEFAULT_UNINITIALIZED_TEXT,
            X = textInstanceX,
            Y = textInstanceY,
            WidthUnits = DimensionUnitType.RelativeToChildren,
        };
        topLevelContainerRuntime.AddChild(textInstance);
        // Create the container for the slider track and the decorative elements
        ContainerRuntime innerContainerRuntime = new ContainerRuntime
        {
            Height = innerContainerRuntimeHeight,
            Width = innerContainerRuntimeWidth,
            X = innerContainerRuntimeX,
            Y = innerContainerRuntimeY,
        };
        topLevelContainerRuntime.AddChild(innerContainerRuntime);
        // Create the "OFF" side of the slider (left side)
        TextureRegion offBackgroundTextureRegion = textureAtlas.GetRegion(
            OFF_BACKGROUND_ATLAS_STRING
        );
        NineSliceRuntime offBackground = new NineSliceRuntime
        {
            Texture = textureAtlas.Texture,
            TextureAddress = TextureAddress.Custom,
            TextureHeight = offBackgroundTextureRegion.Height,
            TextureLeft = offBackgroundTextureRegion.SourceRectangle.Left,
            TextureTop = offBackgroundTextureRegion.SourceRectangle.Top,
            TextureWidth = offBackgroundTextureRegion.Width,
            Width = offBackgroundTextureRegionWidth,
            WidthUnits = DimensionUnitType.Absolute,
        };
        offBackground.Dock(Gum.Wireframe.Dock.Left);
        innerContainerRuntime.AddChild(offBackground);
        // Create the middle track portion of the slider
        TextureRegion middleBackgroundTextureRegion = textureAtlas.GetRegion(
            MIDDLE_BACKGROUND_ATLAS_STRING
        );
        NineSliceRuntime middleBackground = new NineSliceRuntime
        {
            Texture = middleBackgroundTextureRegion.Texture,
            TextureAddress = TextureAddress.Custom,
            TextureHeight = middleBackgroundTextureRegion.Height,
            TextureLeft = middleBackgroundTextureRegion.SourceRectangle.Left,
            TextureTop = middleBackgroundTextureRegion.SourceRectangle.Top,
            TextureWidth = middleBackgroundTextureRegion.Width,
            Width = middleBackgroundWidth,
            WidthUnits = DimensionUnitType.Absolute,
        };
        middleBackground.Dock(Gum.Wireframe.Dock.Left);
        middleBackground.X = middleBackgroundX;
        innerContainerRuntime.AddChild(middleBackground);
        // Create the "MAX" side of the slider (right end)
        TextureRegion maxBackgroundTextureRegion = textureAtlas.GetRegion(
            MAX_BACKGROUND_ATLAS_STRING
        );
        NineSliceRuntime maxBackground = new NineSliceRuntime
        {
            Texture = maxBackgroundTextureRegion.Texture,
            TextureAddress = TextureAddress.Custom,
            TextureHeight = maxBackgroundTextureRegion.Height,
            TextureLeft = maxBackgroundTextureRegion.SourceRectangle.Left,
            TextureTop = maxBackgroundTextureRegion.SourceRectangle.Top,
            TextureWidth = maxBackgroundTextureRegion.Width,
            Width = maxBackgroundWidth,
            WidthUnits = DimensionUnitType.Absolute,
        };
        maxBackground.Dock(Gum.Wireframe.Dock.Right);
        innerContainerRuntime.AddChild(maxBackground);
        // Create the interactive track that responds to clicks
        // The special name "TrackInstance" is required for slider functionality
        ContainerRuntime trackInstance = new ContainerRuntime
        {
            Name = specialNameTrackInstance,
            Height = trackInstanceHeight,
            Width = trackInstanceWidth,
        };
        trackInstance.Dock(Gum.Wireframe.Dock.Fill);
        middleBackground.AddChild(trackInstance);
        // Create a fill rectangle that visually displays the current value
        currentValueFillRectangle = new ColoredRectangleRuntime
        {
            Width = DEFAULT_FILLRECTANGLE_WIDTH,
            WidthUnits = DimensionUnitType.PercentageOfParent,
        };
        currentValueFillRectangle.Dock(Gum.Wireframe.Dock.Left);
        trackInstance.AddChild(currentValueFillRectangle);
        // Add "OFF" text to the left end
        TextRuntime offText = new TextRuntime
        {
            Red = offAndMaxTextRed,
            Green = offAndMaxTextGreen,
            Blue = offAndMaxTextRed,
            CustomFontFile = CUSTOM_FONT_FILE,
            FontScale = offAndMaxTextFontScale,
            Text = OFF_TEXT_STRING,
        };
        offText.Anchor(Gum.Wireframe.Anchor.Center);
        offBackground.AddChild(offText);
        // Add "MAX" text to the right end
        TextRuntime maxText = new TextRuntime
        {
            Red = offAndMaxTextRed,
            Green = offAndMaxTextGreen,
            Blue = offAndMaxTextBlue,
            CustomFontFile = CUSTOM_FONT_FILE,
            FontScale = offAndMaxTextFontScale,
            Text = MAX_TEXT_STRING,
        };
        maxText.Anchor(Gum.Wireframe.Anchor.Center);
        maxBackground.AddChild(maxText);
        // Create a Slider state category - SliderSliderCategoryName is required
        StateSaveCategory sliderCategory = new StateSaveCategory
        {
            Name = Slider.SliderCategoryName,
        };
        topLevelContainerRuntime.AddCategory(sliderCategory);
        // Create the enabled (default/unfocused) state
        StateSave enabledStateSave = new StateSave
        {
            Name = FrameworkElement.EnabledStateName,
            Apply = () =>
            {
                background.Color = unfocusedStateColor;
                textInstance.Color = unfocusedStateColor;
                offBackground.Color = unfocusedStateColor;
                middleBackground.Color = unfocusedStateColor;
                maxBackground.Color = unfocusedStateColor;
                currentValueFillRectangle.Color = unfocusedStateColor;
            },
        };
        sliderCategory.States.Add(enabledStateSave);
        // Create the focused state
        StateSave focusedStateSave = new StateSave
        {
            Name = FrameworkElement.FocusedStateName,
            Apply = () =>
            {
                background.Color = focusedStateColor;
                textInstance.Color = focusedStateColor;
                offBackground.Color = focusedStateColor;
                middleBackground.Color = focusedStateColor;
                maxBackground.Color = focusedStateColor;
                currentValueFillRectangle.Color = focusedStateColor;
            },
        };
        sliderCategory.States.Add(focusedStateSave);
        // Create the highlighted+focused state by cloning the focused state
        StateSave highlightedPlusFocusedStateSave = focusedStateSave.Clone();
        highlightedPlusFocusedStateSave.Name = FrameworkElement.HighlightedFocusedStateName;
        sliderCategory.States.Add(highlightedPlusFocusedStateSave);
        // Assign the configured container as this slider's visual
        Visual = topLevelContainerRuntime;
        // Enable click-to-point functionality for the slider
        IsMoveToPointEnabled = true;
        // Add event handlers
        Visual.RollOn += HandleRollOn;
        ValueChanged += HandleValueChanged;
        ValueChangedByUi += HandleValueChangedByUi;
    }

    /// <summary>
    /// Automatically focuses the slider when the user interacts with it
    /// </summary>
    private void HandleValueChangedByUi(object sender, EventArgs e)
    {
        IsFocused = true;
    }

    /// <summary>
    /// Automatically focuses the slider when the mouse hovers over it
    /// </summary>
    private void HandleRollOn(object sender, EventArgs e)
    {
        IsFocused = true;
    }

    /// <summary>
    /// Updates the fill rectangle width to visually represent the current value
    /// </summary>
    private void HandleValueChanged(object sender, EventArgs e)
    {
        // Calculate the ratio of the current value within its range
        double ratio = (Value - Minimum) / (Maximum - Minimum);
        // Update the fill rectangle width as a percentage
        // _fillRectangle uses percentage width units, so we multiply by 100
        currentValueFillRectangle.Width = 100 * (float)ratio;
    }
    #endregion
}
