namespace MonoGameLibrary.Graphics.Tiles
{
    public class TileSet
    {
        #region Member Variables
        private readonly TextureRegion[] tiles;
        #endregion
        #region Properties
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int Columns { get; }
        public int Rows { get; }
        public int Count { get; }
        #endregion
        #region Constructor
        public TileSet(TextureRegion textureRegion, int tileWidth, int tileHeight)
        {
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
            Columns = textureRegion.Width / tileWidth;
            Rows = textureRegion.Height / tileHeight;
            Count = Columns * Rows;
            tiles = new TextureRegion[Count];
            for (int i = 0; i < Count; i++)
            {
                int x = i % Columns * tileWidth;
                int y = i / Columns * tileHeight;
                tiles[i] = new TextureRegion(
                    textureRegion.Texture,
                    textureRegion.SourceRectangle.X + x,
                    textureRegion.SourceRectangle.Y + y,
                    tileWidth,
                    tileHeight
                );
            }
        }
        #endregion
        #region Public Methods
        public TextureRegion GetTile(int index)
        {
            return tiles[index];
        }

        public TextureRegion GetTile(int column, int row)
        {
            int index = row * Columns + column;
            return (GetTile(index));
        }
        #endregion
    }
}
