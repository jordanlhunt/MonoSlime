using System;
using DungeonSlime.UI;
using Gum.DataTypes;
using Gum.Forms.Controls;
using Gum.Forms.DefaultVisuals;
using Gum.Managers;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.Tiles;
using MonoGameLibrary.Input;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Shapes;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    #region Constants

    private const string ScoreString = "SCORE";
    private const float MovementSpeed = 5.0f;
    private const string BounceSoundEffectLocation = "Sounds/bounce";
    private const string CollectSoundEffectLocation = "Sounds/collect";
    private const string AtlasDefinitionLocation = "Images/atlas-definition.xml";
    private const string AtlasSlimeAnimation = "slime-animation";
    private const string AtlasBatAnimation = "bat-animation";
    private const string TilemapDefinitionLocation = "Images/tilemap-definition.xml";
    private const string EquipmentProSpriteFontLocation = "Fonts/EquipmentPro";
    private const string UI_SOUND_EFFECT_LOCATION = "Sounds/Click_15";
    private const string CUSTOM_FONT_FILE = "Fonts/04b_30.fnt";
    private const string AtlasPanelBackground = "panel-background";

    #endregion

    #region Member Variables

    private AnimatedSprite player;
    private AnimatedSprite bat;
    private Vector2 playerPosition;
    private Vector2 batPosition;
    private Vector2 batVelocity;
    private Vector2 scoreTextPosition;
    private Vector2 scoreTextOrigin;
    private Tilemap tileMap;
    private Rectangle roomBoundsRectangle;
    private SoundEffect bounceSoundEffect;
    private SoundEffect collectSoundEffect;
    private SpriteFont spriteFont;
    private Circle playerBoundsCircle;
    private Circle batBoundsCircle;
    private int currentScore;
    private Panel pausePanel;
    private AnimatedButton resumeButton;
    private TextureAtlas textureAtlas;
    private SoundEffect uiSoundEffect;

    #endregion

    #region Public Methods

    public override void Initialize()
    {
        base.Initialize();
        Core.ExitOnEscape = false;
        Rectangle screenBoundsRectangle = Core.GraphicsDevice.PresentationParameters.Bounds;
        roomBoundsRectangle = new Rectangle(
            (int)tileMap.TileWidth,
            (int)tileMap.TileHeight,
            screenBoundsRectangle.Width - (int)tileMap.TileWidth * 2,
            screenBoundsRectangle.Height - (int)tileMap.TileHeight * 2
        );
        int centerRow = tileMap.Rows / 2;
        int centerColumn = tileMap.Columns / 2;
        playerPosition = new Vector2(
            centerColumn * tileMap.TileWidth,
            centerRow * tileMap.TileHeight
        );
        batPosition = new Vector2(roomBoundsRectangle.Left, roomBoundsRectangle.Top);
        scoreTextPosition = new Vector2(roomBoundsRectangle.Left, tileMap.TileHeight * .5f);
        float scoreTextYOrigin = spriteFont.MeasureString(ScoreString).Y * .5f;
        scoreTextOrigin = new Vector2(0, scoreTextYOrigin);
        AssignRandomBatVelocity();
        InitializeUI();
    }

    public override void LoadContent()
    {
        textureAtlas = TextureAtlas.FromFile(Core.Content, AtlasDefinitionLocation);
        player = textureAtlas.CreateAnimatedSprite(AtlasSlimeAnimation);
        bat = textureAtlas.CreateAnimatedSprite(AtlasBatAnimation);
        tileMap = Tilemap.LoadFromFile(ContentManager, TilemapDefinitionLocation);
        player.Scale = new Vector2(4.0f, 4.0f);
        bat.Scale = new Vector2(4.0f, 4.0f);
        tileMap.Scale = new Vector2(4.0f, 4.0f);
        bounceSoundEffect = ContentManager.Load<SoundEffect>(BounceSoundEffectLocation);
        collectSoundEffect = ContentManager.Load<SoundEffect>(CollectSoundEffectLocation);
        spriteFont = ContentManager.Load<SpriteFont>(EquipmentProSpriteFontLocation);
        uiSoundEffect = ContentManager.Load<SoundEffect>(UI_SOUND_EFFECT_LOCATION);
    }

    public override void Update(GameTime gameTime)
    {
        GumService.Default.Update(gameTime);
        if (pausePanel.IsVisible)
        {
            return;
        }
        player.Update(gameTime);
        bat.Update(gameTime);
        HandleKeyboardInput();
        HandleGamePadInput();
        HandleSlimeBounds();
        Vector2 newBatPosition = batPosition + batVelocity;
        newBatPosition = HandleBatBounds(newBatPosition);
        batPosition = newBatPosition;
        HandleSlimeBatIntersection();
    }

    public override void Draw(GameTime gameTime)
    {
        Core.GraphicsDevice.Clear(Color.MonoGameOrange);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        tileMap.Draw(Core.SpriteBatch);
        player.Draw(Core.SpriteBatch, playerPosition);
        bat.Draw(Core.SpriteBatch, batPosition);
        Core.SpriteBatch.DrawString(
            spriteFont,
            ScoreString + $": {currentScore}",
            scoreTextPosition,
            Color.White,
            0.0f,
            scoreTextOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );
        Core.SpriteBatch.End();
        GumService.Default.Draw();
    }

    #endregion

    #region Private Methods

    private void AssignRandomBatVelocity()
    {
        float angle = (float)(Random.Shared.NextDouble() * Math.PI * 2);
        float x = (float)Math.Cos(angle);
        float y = (float)Math.Sin(angle);
        Vector2 direction = new Vector2(x, y);
        batVelocity = direction * MovementSpeed;
    }

    private void HandleSlimeBounds()
    {
        playerBoundsCircle = new Circle(
            (int)(playerPosition.X + (player.Width * 0.5f)),
            (int)(playerPosition.Y + (player.Height * .5f)),
            (int)(player.Width * .5f)
        );
        if (playerBoundsCircle.Left < roomBoundsRectangle.Left)
        {
            playerPosition.X = roomBoundsRectangle.Left;
        }
        else if (playerBoundsCircle.Right > roomBoundsRectangle.Right)
        {
            playerPosition.X = roomBoundsRectangle.Right - player.Width;
        }

        if (playerBoundsCircle.Top < roomBoundsRectangle.Top)
        {
            playerPosition.Y = roomBoundsRectangle.Top;
        }
        else if (playerBoundsCircle.Bottom > roomBoundsRectangle.Bottom)
        {
            playerPosition.Y = roomBoundsRectangle.Bottom - player.Height;
        }
    }

    private Vector2 HandleBatBounds(Vector2 newBatPosition)
    {
        batBoundsCircle = new Circle(
            (int)(newBatPosition.X + (bat.Width * 0.5f)),
            (int)(newBatPosition.Y + (bat.Height * .5f)),
            (int)(bat.Width * 0.5f)
        );
        Vector2 normal = Vector2.Zero;
        if (batBoundsCircle.Left < roomBoundsRectangle.Left)
        {
            normal.X = Vector2.UnitX.X;
            newBatPosition.X = roomBoundsRectangle.Left;
        }
        else if (batBoundsCircle.Right > roomBoundsRectangle.Right)
        {
            normal.X = -Vector2.UnitX.X;
            newBatPosition.X = roomBoundsRectangle.Right - bat.Width;
        }

        if (batBoundsCircle.Top < roomBoundsRectangle.Top)
        {
            normal.Y = Vector2.UnitY.Y;
            newBatPosition.Y = roomBoundsRectangle.Top;
        }
        else if (batBoundsCircle.Bottom > roomBoundsRectangle.Bottom)
        {
            normal.Y = -Vector2.UnitY.Y;
            newBatPosition.Y = roomBoundsRectangle.Bottom - bat.Height;
        }

        if (normal != Vector2.Zero)
        {
            normal.Normalize();
            batVelocity = Vector2.Reflect(batVelocity, normal);
            Core.Audio.PlaySoundEffect(bounceSoundEffect);
        }

        return newBatPosition;
    }

    private void HandleSlimeBatIntersection()
    {
        if (playerBoundsCircle.IsIntersecting(batBoundsCircle))
        {
            int totalColumns =
                Core.GraphicsDevice.PresentationParameters.BackBufferWidth / (int)bat.Width;
            int totalRows =
                Core.GraphicsDevice.PresentationParameters.BackBufferHeight / (int)bat.Height;
            int column = Random.Shared.Next(0, totalColumns);
            int row = Random.Shared.Next(0, totalRows);
            batPosition = new Vector2(column * bat.Width, row * bat.Height);
            AssignRandomBatVelocity();
            Core.Audio.PlaySoundEffect(collectSoundEffect);
            currentScore += 100;
        }
    }

    private void HandleKeyboardInput()
    {
        KeyboardInputInfo keyboardInputInfo = Core.Input.Keyboard;
        // if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        // {
        //     Core.ChangeScene(new TitleScene());
        // }
        if (Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape))
        {
            PauseGame();
        }

        float speed = MovementSpeed;
        if (keyboardInputInfo.IsKeyDown(Keys.Space))
        {
            speed *= 1.75f;
        }

        if (keyboardInputInfo.IsKeyDown(Keys.W) || keyboardInputInfo.IsKeyDown(Keys.Up))
        {
            playerPosition.Y -= speed;
        }

        if (keyboardInputInfo.IsKeyDown(Keys.S) || keyboardInputInfo.IsKeyDown(Keys.Down))
        {
            playerPosition.Y += speed;
        }

        if (keyboardInputInfo.IsKeyDown(Keys.D) || keyboardInputInfo.IsKeyDown(Keys.Right))
        {
            playerPosition.X += speed;
        }

        if (keyboardInputInfo.IsKeyDown(Keys.A) || keyboardInputInfo.IsKeyDown(Keys.Left))
        {
            playerPosition.X -= speed;
        }

        if (keyboardInputInfo.WasKeyJustPressed(Keys.M))
        {
            Core.Audio.ToggleMute();
        }

        if (keyboardInputInfo.WasKeyJustPressed((Keys.OemMinus)))
        {
            Core.Audio.CurrentSongVolume -= 0.1f;
            Core.Audio.CurrentSoundEffectVolume -= 0.1f;
        }
    }

    private void HandleGamePadInput()
    {
        // Get the gamepad info for gamepad one.
        GamePadInputInfo gamePadZero = Core.Input.GamePads[(int)PlayerIndex.One];
        float speed = MovementSpeed;
        if (gamePadZero.WasButtonJustPressed(Buttons.Start))
        {
            PauseGame();
        }
        if (gamePadZero.IsButtonDown(Buttons.A))
        {
            speed *= 1.5f;
            GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);
        }
        else
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }

        if (gamePadZero.LeftThumbStick != Vector2.Zero)
        {
            playerPosition.X += gamePadZero.LeftThumbStick.X * speed;
            playerPosition.Y -= gamePadZero.LeftThumbStick.Y * speed;
        }
        else
        {
            if (gamePadZero.IsButtonDown(Buttons.DPadUp))
            {
                playerPosition.Y -= speed;
            }

            if (gamePadZero.IsButtonDown(Buttons.DPadDown))
            {
                playerPosition.Y += speed;
            }

            if (gamePadZero.IsButtonDown(Buttons.DPadLeft))
            {
                playerPosition.X -= speed;
            }

            if (gamePadZero.IsButtonDown(Buttons.DPadRight))
            {
                playerPosition.X += speed;
            }
        }
    }

    private void PauseGame()
    {
        pausePanel.IsVisible = true;
        resumeButton.IsFocused = true;
    }

    private void CreatePausePanel()
    {
        pausePanel = new Panel();
        pausePanel.Anchor(Anchor.Center);
        pausePanel.Visual.WidthUnits = DimensionUnitType.Absolute;
        pausePanel.Visual.HeightUnits = DimensionUnitType.Absolute;
        pausePanel.Visual.Height = 70;
        pausePanel.Visual.Width = 264;
        pausePanel.IsVisible = false;
        pausePanel.AddToRoot();

        TextureRegion backgroundRegion = textureAtlas.GetRegion(AtlasPanelBackground);
        NineSliceRuntime background = new NineSliceRuntime
        {
            Texture = backgroundRegion.Texture,
            TextureAddress = TextureAddress.Custom,
            TextureHeight = backgroundRegion.Height,
            TextureLeft = backgroundRegion.SourceRectangle.Left,
            TextureTop = backgroundRegion.SourceRectangle.Top,
            TextureWidth = backgroundRegion.Width,
        };
        background.Dock(Dock.Fill);
        pausePanel.AddChild(background);
        TextRuntime textRuntime = new TextRuntime
        {
            Text = "Paused",
            X = 10f,
            Y = 10f,
            UseCustomFont = true,
            CustomFontFile = CUSTOM_FONT_FILE,
            FontScale = .05f,
        };
        pausePanel.AddChild(textRuntime);
        resumeButton = new AnimatedButton(textureAtlas);
        resumeButton.Anchor(Anchor.BottomLeft);
        resumeButton.Text = "Resume";
        resumeButton.Visual.X = 9f;
        resumeButton.Visual.Y = -9f;
        resumeButton.Click += HandleResumeButtonClicked;
        pausePanel.AddChild(resumeButton);
        AnimatedButton quitButton = new AnimatedButton(textureAtlas);
        quitButton.Text = "QUIT";
        quitButton.Anchor(Anchor.BottomRight);
        quitButton.Visual.X = -9f;
        quitButton.Visual.Y = -9f;
        quitButton.Click += HandleQuitButtonClicked;
        pausePanel.AddChild(quitButton);
    }

    private void HandleResumeButtonClicked(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        pausePanel.IsVisible = false;
    }

    private void HandleQuitButtonClicked(object sender, EventArgs eventArgs)
    {
        Core.Audio.PlaySoundEffect(uiSoundEffect);
        Core.ChangeScene(new TitleScene());
    }

    private void InitializeUI()
    {
        GumService.Default.Root.Children.Clear();
        CreatePausePanel();
    }
    #endregion
}
