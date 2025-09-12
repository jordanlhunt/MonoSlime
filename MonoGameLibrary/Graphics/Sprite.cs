using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class Sprite
{
    #region Properties
    public TextureRegion TextureRegion { get; set; }
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0.0f;
    public Vector2 Scale { get; set; } = Vector2.One;
    public Vector2 Origin { get; set; } = Vector2.Zero;
    public float LayerDepth { get; set; } = 0.0f;
    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;
    public float Width
    {
        get { return TextureRegion.Width * Scale.X; }
    }
    public float Height
    {
        get { return TextureRegion.Height * Scale.Y; }
    }
    #endregion
    #region Constructor
    public Sprite() { }

    public Sprite(TextureRegion textureRegion)
    {
        this.TextureRegion = textureRegion;
    }
    #endregion

    #region Public Methods
    public void SetOriginToCenter()
    {
        Origin = new Vector2(TextureRegion.Width, TextureRegion.Height) * 0.5f;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 renderLocation)
    {
        TextureRegion.Draw(
            spriteBatch,
            renderLocation,
            Color,
            Rotation,
            Origin,
            Scale,
            SpriteEffects,
            LayerDepth
        );
    }

    #endregion
}
