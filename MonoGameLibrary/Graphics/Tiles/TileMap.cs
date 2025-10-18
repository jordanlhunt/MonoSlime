using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics.Tiles;

public class Tilemap
{
    #region Member Variables
    private readonly TileSet tileSet;
    private readonly int[] tileIds;
    #endregion
    #region Properties
    public int Rows { get; }
    public int Columns { get; }
    public int Count { get; }
    public Vector2 Scale { get; set; }
    public float TileWidth
    {
        get { return tileSet.TileWidth * Scale.X; }
    }
    public float TileHeight
    {
        get { return tileSet.TileHeight * Scale.Y; }
    }
    #endregion
    #region Constructor
    public Tilemap(TileSet tileSet, int columns, int rows)
    {
        this.tileSet = tileSet;
        Rows = rows;
        Columns = columns;
    }
    #endregion
}
