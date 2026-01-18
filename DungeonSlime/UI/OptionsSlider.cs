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
    private const string CUSTOM_FONT_FILE = "Fonts/04b_30.fnt";
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
    private readonly float offAndMaxTextFontScale = .25f;
    #endregion

    #region Member Variables
    private TextRuntime textInstance;
    private ColoredRectangleRuntime currentValueFillRectangle;
    private Color unfocusedStateColor = Color.Gray;
    private Color focusedStateColor = Color.White;
    #endregion

    #region Properties
    public string Text
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
    public OptionsSlider(TextureAtlas textureAtlas)
    {
        ContainerRuntime topLevelContainerRuntime = CreateTopLevelContainer();
        NineSliceRuntime background = CreateBackground(textureAtlas);
        topLevelContainerRuntime.AddChild(background);

        textInstance = CreateTitleText();
        topLevelContainerRuntime.AddChild(textInstance);

        ContainerRuntime innerContainerRuntime = CreateInnerContainer();
        topLevelContainerRuntime.AddChild(innerContainerRuntime);

        PopulateInnerContainer(innerContainerRuntime, textureAtlas);
        CreateSliderStates(topLevelContainerRuntime, background, innerContainerRuntime);

        Visual = topLevelContainerRuntime;
        IsMoveToPointEnabled = true;

        AttachEventHandlers();
    }
    #endregion

    #region Container Creation Methods
    private ContainerRuntime CreateTopLevelContainer()
    {
        return new ContainerRuntime { Height = containerHeight, Width = containerWidth };
    }

    private ContainerRuntime CreateInnerContainer()
    {
        return new ContainerRuntime
        {
            Height = innerContainerRuntimeHeight,
            Width = innerContainerRuntimeWidth,
            X = innerContainerRuntimeX,
            Y = innerContainerRuntimeY,
        };
    }
    #endregion

    #region Background Creation Methods
    private NineSliceRuntime CreateBackground(TextureAtlas textureAtlas)
    {
        TextureRegion backgroundTextureRegion = textureAtlas.GetRegion(
            PANEL_BACKGROUND_ATLAS_STRING
        );

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

        return background;
    }

    private NineSliceRuntime CreateOffBackground(TextureAtlas textureAtlas)
    {
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

        return offBackground;
    }

    private NineSliceRuntime CreateMiddleBackground(TextureAtlas textureAtlas)
    {
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

        return middleBackground;
    }

    private NineSliceRuntime CreateMaxBackground(TextureAtlas textureAtlas)
    {
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

        return maxBackground;
    }
    #endregion

    #region Text Creation Methods
    private TextRuntime CreateTitleText()
    {
        return new TextRuntime
        {
            CustomFontFile = CUSTOM_FONT_FILE,
            UseCustomFont = true,
            FontScale = textInstanceFontScale,
            Text = DEFAULT_UNINITIALIZED_TEXT,
            X = textInstanceX,
            Y = textInstanceY,
            WidthUnits = DimensionUnitType.RelativeToChildren,
        };
    }

    private TextRuntime CreateOffText()
    {
        return new TextRuntime
        {
            Red = offAndMaxTextRed,
            Green = offAndMaxTextGreen,
            Blue = offAndMaxTextBlue,
            CustomFontFile = CUSTOM_FONT_FILE,
            FontScale = offAndMaxTextFontScale,
            UseCustomFont = true,
            Text = OFF_TEXT_STRING,
        };
    }

    private TextRuntime CreateMaxText()
    {
        return new TextRuntime
        {
            Red = offAndMaxTextRed,
            Green = offAndMaxTextGreen,
            Blue = offAndMaxTextBlue,
            CustomFontFile = CUSTOM_FONT_FILE,
            FontScale = offAndMaxTextFontScale,
            UseCustomFont = true,
            Text = MAX_TEXT_STRING,
        };
    }
    #endregion

    #region Track and Fill Creation Methods
    private ContainerRuntime CreateTrackInstance()
    {
        ContainerRuntime trackInstance = new ContainerRuntime
        {
            Name = specialNameTrackInstance,
            Height = trackInstanceHeight,
            Width = trackInstanceWidth,
        };
        trackInstance.Dock(Gum.Wireframe.Dock.Fill);

        return trackInstance;
    }

    private ColoredRectangleRuntime CreateCurrentValueFillRectangle()
    {
        ColoredRectangleRuntime fillRectangle = new ColoredRectangleRuntime
        {
            Width = DEFAULT_FILLRECTANGLE_WIDTH,
            WidthUnits = DimensionUnitType.PercentageOfParent,
        };
        fillRectangle.Dock(Gum.Wireframe.Dock.Left);

        return fillRectangle;
    }
    #endregion

    #region Inner Container Population
    private void PopulateInnerContainer(
        ContainerRuntime innerContainerRuntime,
        TextureAtlas textureAtlas
    )
    {
        NineSliceRuntime offBackground = CreateOffBackground(textureAtlas);
        innerContainerRuntime.AddChild(offBackground);

        NineSliceRuntime middleBackground = CreateMiddleBackground(textureAtlas);
        innerContainerRuntime.AddChild(middleBackground);

        NineSliceRuntime maxBackground = CreateMaxBackground(textureAtlas);
        innerContainerRuntime.AddChild(maxBackground);

        ContainerRuntime trackInstance = CreateTrackInstance();
        middleBackground.AddChild(trackInstance);

        currentValueFillRectangle = CreateCurrentValueFillRectangle();
        trackInstance.AddChild(currentValueFillRectangle);

        AddEndLabels(offBackground, maxBackground);
    }

    private void AddEndLabels(NineSliceRuntime offBackground, NineSliceRuntime maxBackground)
    {
        TextRuntime offText = CreateOffText();
        offText.Anchor(Gum.Wireframe.Anchor.Center);
        offBackground.AddChild(offText);

        TextRuntime maxText = CreateMaxText();
        maxText.Anchor(Gum.Wireframe.Anchor.Center);
        maxBackground.AddChild(maxText);
    }
    #endregion

    #region State Management
    private void CreateSliderStates(
        ContainerRuntime topLevelContainerRuntime,
        NineSliceRuntime background,
        ContainerRuntime innerContainerRuntime
    )
    {
        StateSaveCategory sliderCategory = new StateSaveCategory
        {
            Name = Slider.SliderCategoryName,
        };
        topLevelContainerRuntime.AddCategory(sliderCategory);

        // Get references to all backgrounds from inner container
        NineSliceRuntime offBackground = innerContainerRuntime.Children[0] as NineSliceRuntime;
        NineSliceRuntime middleBackground = innerContainerRuntime.Children[1] as NineSliceRuntime;
        NineSliceRuntime maxBackground = innerContainerRuntime.Children[2] as NineSliceRuntime;

        StateSave enabledStateSave = CreateEnabledState(
            background,
            offBackground,
            middleBackground,
            maxBackground
        );
        sliderCategory.States.Add(enabledStateSave);

        StateSave focusedStateSave = CreateFocusedState(
            background,
            offBackground,
            middleBackground,
            maxBackground
        );
        sliderCategory.States.Add(focusedStateSave);

        StateSave highlighted = enabledStateSave.Clone();

        StateSave highlightedPlusFocusedStateSave = focusedStateSave.Clone();
        highlighted.Name = FrameworkElement.HighlightedFocusedStateName;
        highlightedPlusFocusedStateSave.Name = FrameworkElement.HighlightedFocusedStateName;
        sliderCategory.States.Add(highlighted);
        sliderCategory.States.Add(highlightedPlusFocusedStateSave);
    }

    private StateSave CreateEnabledState(
        NineSliceRuntime background,
        NineSliceRuntime offBackground,
        NineSliceRuntime middleBackground,
        NineSliceRuntime maxBackground
    )
    {
        return new StateSave
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
    }

    private StateSave CreateFocusedState(
        NineSliceRuntime background,
        NineSliceRuntime offBackground,
        NineSliceRuntime middleBackground,
        NineSliceRuntime maxBackground
    )
    {
        return new StateSave
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
    }
    #endregion

    #region Event Handler Management
    private void AttachEventHandlers()
    {
        Visual.RollOn += HandleRollOn;
        ValueChanged += HandleValueChanged;
        ValueChangedByUi += HandleValueChangedByUi;
    }

    private void HandleValueChangedByUi(object sender, EventArgs e)
    {
        IsFocused = true;
    }

    private void HandleRollOn(object sender, EventArgs e)
    {
        IsFocused = true;
    }

    private void HandleValueChanged(object sender, EventArgs e)
    {
        double ratio = (Value - Minimum) / (Maximum - Minimum);
        currentValueFillRectangle.Width = 100 * (float)ratio;
    }
    #endregion
}
