using System;
using DungeonSlime.GameObjects;
using DungeonSlime.UI;
using Gum.Forms.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Graphics.Tiles;
using MonoGameLibrary.Scenes;
using MonoGameLibrary.Shapes;

namespace DungeonSlime.Scenes;

public class GameScene : Scene
{
    #region Constants
    private enum GameState
    {
        Playing,
        Paused,
        GameOver,
    }

    private const string DUNGEON_TEXT = "Dungeon";
    private const string SLIME_TEXT = "Slime";
    private const string PRESS_ENTER_TEXT = "Press Enter To Start";
    private const string EQUIPMENT_PRO_LOCATION = "Fonts/EquipmentPro";
    private const string COMPASS_PRO_LOCATION = "Fonts/CompassPro";
    private const string REPEATING_BACKGROUND_LOCATION = "Images/background-pattern";
    private const string UI_SOUND_EFFECT_LOCATION = "Sounds/Click_15";
    private const string CUSTOM_FONT_FILE = "Fonts/04b_30.fnt";
    private const string ATLAS_DEFINITION_LOCATION = "Images/atlas-definition.xml";
    private const string TilemapDefinitionLocation = "Images/tilemap-definition.xml";
    private const string TileMapSlimeAnimation = "slime-animation";
    private const string TileMapBatAnimation = "bat-animation";
    private const float TileMapScale = 4.0f;
    private const string BounceSoundEffectLocation = "Sounds/bounce";
    private const string CollectSoundEffectLocation = "Sounds/collect";
    #endregion

    #region Fields
    // Reference to the slime.
    private Slime _slime;

    // Reference to the bat.
    private Bat _bat;

    // Defines the tilemap to draw.
    private Tilemap _tilemap;

    // Defines the bounds of the room that the slime and bat are contained within.
    private Rectangle _roomBounds;

    // The sound effect to play when the slime eats a bat.
    private SoundEffect _collectSoundEffect;

    // Tracks the players score.
    private int _score;
    private GameSceneUI _ui;
    private GameState _state;
    #endregion

    #region Public Methods


    public override void Initialize()
    {
        // LoadContent is during the base.Initialize()
        base.Initialize();
        Core.ExitOnEscape = false;
        _roomBounds = Core.GraphicsDevice.PresentationParameters.Bounds;
        _roomBounds.Inflate(-_tilemap.TileWidth, -_tilemap.TileHeight);
        _slime.BodyCollision += OnSlimeBodyCollision;
        GumService.Default.Root.Children.Clear();
        InitializeUI();
        InitializeNewGame();
    }

    public override void LoadContent()
    {
        base.LoadContent();
        TextureAtlas textureAtlas = TextureAtlas.FromFile(Core.Content, ATLAS_DEFINITION_LOCATION);
        _tilemap = Tilemap.LoadFromFile(ContentManager, TilemapDefinitionLocation);
        _tilemap.Scale = new Vector2(TileMapScale, TileMapScale);
        AnimatedSprite slimeAnimation = textureAtlas.CreateAnimatedSprite(TileMapSlimeAnimation);
        slimeAnimation.Scale = new Vector2(TileMapScale, TileMapScale);
        _slime = new Slime(slimeAnimation);
        AnimatedSprite batAnimation = textureAtlas.CreateAnimatedSprite(TileMapBatAnimation);
        batAnimation.Scale = new Vector2(TileMapScale, TileMapScale);
        SoundEffect bounceSoundEffect = ContentManager.Load<SoundEffect>(BounceSoundEffectLocation);
        _collectSoundEffect = ContentManager.Load<SoundEffect>(CollectSoundEffectLocation);
        _bat = new Bat(batAnimation, bounceSoundEffect);
    }

    public override void Update(GameTime gameTime)
    {
        _ui.Update(gameTime);
        if (_state == GameState.GameOver)
        {
            return;
        }

        if (GameController.Pause())
        {
            TogglePause();
        }

        if (_state == GameState.Paused)
        {
            return;
        }
        _slime.Update(gameTime);
        _bat.Update(gameTime);
        CollisionChecks();
    }

    public override void Draw(GameTime gameTime)
    {
        // Clear the back buffer
        Core.GraphicsDevice.Clear(Color.MonoGameOrange);
        Core.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tilemap.Draw(Core.SpriteBatch);
        _slime.Draw();
        _bat.Draw();
        Core.SpriteBatch.End();
        _ui.Draw();
    }

    #endregion
    #region Private Methods

    private void InitializeUI()
    { // Clear out any previous UI element just in case we came from a different scene
        GumService.Default.Root.Children.Clear();
        // Create a new UI instance
        _ui = new GameSceneUI();
        // Subscribe to the events from the GameSceneUI
        _ui.ResumeButtonClick += OnResumeButtonClicked;
        _ui.RetryButtonClick += OnRetryButtonClicked;
        _ui.QuitButtonClick += OnQuitButtonClicked;
    }

    private void InitializeNewGame()
    {
        Vector2 slimePosition = new Vector2();
        slimePosition.X = (_tilemap.Columns / 2) * _tilemap.TileWidth;
        slimePosition.Y = (_tilemap.Rows / 2) * _tilemap.TileHeight;
        _slime.Initialize(slimePosition, _tilemap.TileWidth);
        _bat.RandomizeVelocity();
        PositionBatAwayFromSlime();
        _score = 0;
        _ui.UpdateScoreText(_score);
        _state = GameState.Playing;
    }

    private void OnQuitButtonClicked(object sender, EventArgs e)
    {
        Core.ChangeScene(new TitleScene());
    }

    private void OnRetryButtonClicked(object sender, EventArgs e)
    {
        InitializeNewGame();
    }

    private void OnResumeButtonClicked(object sender, EventArgs e)
    {
        _state = GameState.Playing;
    }

    private void TogglePause()
    {
        if (_state == GameState.Paused)
        {
            _ui.HidePausePanel();
            _state = GameState.Playing;
        }
        else
        {
            _ui.ShowPausePanel();
            _state = GameState.Paused;
        }
    }

    private void CollisionChecks()
    {
        Circle slimeBounds = _slime.GetBounds();
        Circle batBounds = _bat.GetBounds();

        if (slimeBounds.IsIntersecting(batBounds))
        {
            PositionBatAwayFromSlime();
            _bat.RandomizeVelocity();
            _slime.Grow();
            _score += 100;
            _ui.UpdateScoreText(_score);
            Core.Audio.PlaySoundEffect(_collectSoundEffect);
        }
        if (
            slimeBounds.Top < _roomBounds.Top
            || slimeBounds.Bottom > _roomBounds.Bottom
            || slimeBounds.Left < _roomBounds.Left
            || slimeBounds.Right > _roomBounds.Right
        )
        {
            GameOver();
            return;
        }
        if (batBounds.Top < _roomBounds.Top)
        {
            _bat.Bounce(Vector2.UnitY);
        }
        else if (batBounds.Bottom > _roomBounds.Bottom)
        {
            _bat.Bounce(-Vector2.UnitY);
        }

        if (batBounds.Left < _roomBounds.Left)
        {
            _bat.Bounce(Vector2.UnitX);
        }
        else if (batBounds.Right > _roomBounds.Right)
        {
            _bat.Bounce(-Vector2.UnitX);
        }
    }

    private void PositionBatAwayFromSlime()
    {
        // Calculate the position that is in the center of the bounds
        // of the room.
        float roomCenterX = _roomBounds.X + _roomBounds.Width * 0.5f;
        float roomCenterY = _roomBounds.Y + _roomBounds.Height * 0.5f;
        Vector2 roomCenter = new Vector2(roomCenterX, roomCenterY);

        // Get the bounds of the slime and calculate the center position.
        Circle slimeBounds = _slime.GetBounds();
        Vector2 slimeCenter = new Vector2(slimeBounds.X, slimeBounds.Y);

        // Calculate the distance vector from the center of the room to the
        // center of the slime.
        Vector2 centerToSlime = slimeCenter - roomCenter;

        // Get the bounds of the bat.
        Circle batBounds = _bat.GetBounds();

        // Calculate the amount of padding we will add to the new position of
        // the bat to ensure it is not sticking to walls
        int padding = batBounds.Radius * 2;

        // Calculate the new position of the bat by finding which component of
        // the center to slime vector (X or Y) is larger and in which direction.
        Vector2 newBatPosition = Vector2.Zero;
        if (Math.Abs(centerToSlime.X) > Math.Abs(centerToSlime.Y))
        {
            // The slime is closer to either the left or right wall, so the Y
            // position will be a random position between the top and bottom
            // walls.
            newBatPosition.Y = Random.Shared.Next(
                _roomBounds.Top + padding,
                _roomBounds.Bottom - padding
            );

            if (centerToSlime.X > 0)
            {
                // The slime is closer to the right side wall, so place the
                // bat on the left side wall.
                newBatPosition.X = _roomBounds.Left + padding;
            }
            else
            {
                // The slime is closer ot the left side wall, so place the
                // bat on the right side wall.
                newBatPosition.X = _roomBounds.Right - padding * 2;
            }
        }
        else
        {
            // The slime is closer to either the top or bottom wall, so the X
            // position will be a random position between the left and right
            // walls.
            newBatPosition.X = Random.Shared.Next(
                _roomBounds.Left + padding,
                _roomBounds.Right - padding
            );

            if (centerToSlime.Y > 0)
            {
                // The slime is closer to the top wall, so place the bat on the
                // bottom wall.
                newBatPosition.Y = _roomBounds.Top + padding;
            }
            else
            {
                // The slime is closer to the bottom wall, so place the bat on
                // the top wall.
                newBatPosition.Y = _roomBounds.Bottom - padding * 2;
            }
        }

        // Assign the new bat position.
        _bat.Position = newBatPosition;
    }

    private void OnSlimeBodyCollision(object send, EventArgs args)
    {
        GameOver();
    }

    private void GameOver()
    {
        _ui.ShowGameOverPanel();
        _state = GameState.GameOver;
    }

    #endregion
}
