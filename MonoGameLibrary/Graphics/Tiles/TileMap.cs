using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics.Tiles;

/// <summary>
/// The Tilemap will arrange the tilesets into a game level
/// </summary>
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
        Rows = rows;
        Columns = columns;
        Scale = Vector2.One;
        Count = Rows * Columns;
        this.tileSet = tileSet;
        this.tileIds = new int[Count];
    }
    #endregion
    #region Public Methods
    public void SetTile(int index, int tileSetid)
    {
        tileIds[index] = tileSetid;
    }

    public void SetTile(int column, int row, int tileSetid)
    {
        int index = (row * Columns) + column;
        SetTile(index, tileSetid);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < Count; i++)
        {
            int tilesetIndex = tileIds[i];
            TextureRegion tile = tileSet.GetTile(tilesetIndex);
            int x = i % Columns;
            int y = i / Columns;
            Vector2 position = new Vector2(x * TileWidth, y * TileHeight);
            tile.Draw(
                spriteBatch,
                position,
                Color.White,
                0.0f,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                1.0f
            );
        }
    }

    public static Tilemap LoadFromFile(ContentManager contentManager, string fileName)
    {
        string filePath = Path.Combine(contentManager.RootDirectory, fileName);
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader xmlReader = XmlReader.Create(stream))
            {
                XDocument xDocument = XDocument.Load(xmlReader);
                XElement xRoot = xDocument.Root;
                // The <Tileset> element contains the information about the tileset
                // used by the tilemap.
                //
                // Example
                // <Tileset region="0 0 100 100" tileWidth="10" tileHeight="10">contentPath</Tileset>
                //
                // The region attribute represents the x, y, width, and height
                // components of the boundary for the texture region within the
                // texture at the contentPath specified.
                //
                // the tileWidth and tileHeight attributes specify the width and
                // height of each tile in the tileset.
                //
                // the contentPath value is the contentPath to the texture to
                // load that contains the tileset
                XElement xTilesetElement = xRoot.Element("Tileset");
                string regionAttribute = xTilesetElement.Attribute("region").Value;
                string[] split = regionAttribute.Split(
                    " ",
                    System.StringSplitOptions.RemoveEmptyEntries
                );
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                int width = int.Parse(split[2]);
                int height = int.Parse(split[3]);
                int tileWidth = int.Parse(xTilesetElement.Attribute("tileWidth").Value);
                int tileHeight = int.Parse(xTilesetElement.Attribute("tileHeight").Value);
                string contentPath = xTilesetElement.Value;
                Texture2D texture = contentManager.Load<Texture2D>(contentPath);
                TextureRegion textureRegion = new TextureRegion(texture, x, y, width, height);
                TileSet tileSet = new TileSet(textureRegion, tileWidth, tileHeight);
                // The <Tiles> element contains lines of strings where each line
                // represents a row in the tilemap.  Each line is a space
                // separated string where each element represents a column in that
                // row.  The value of the column is the id of the tile in the
                // tileset to draw for that location.
                //
                // Example:
                // <Tiles>
                //      00 01 01 02
                //      03 04 04 05
                //      03 04 04 05
                //      06 07 07 08
                // </Tiles>
                XElement xTilesElement = xRoot.Element("Tiles");
                string[] rows = xTilesElement
                    .Value.Trim()
                    .Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
                int columnCount = rows[0]
                    .Split(" ", System.StringSplitOptions.RemoveEmptyEntries)
                    .Length;
                Tilemap tilemap = new Tilemap(tileSet, columnCount, rows.Length);
                for (int row = 0; row < rows.Length; row++)
                {
                    string[] columns = rows[row]
                        .Trim()
                        .Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                    for (int column = 0; column < columnCount; column++)
                    {
                        int tilesetIndex = int.Parse(columns[column]);
                        tilemap.SetTile(column, row, tilesetIndex);
                    }
                }
                return tilemap;
            }
        }
    }
    #endregion
}
