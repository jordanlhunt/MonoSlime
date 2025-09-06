using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary.Graphics;

public class TextureAtlas
{
    #region Member Variables
    private Dictionary<string, TextureRegion> textureRegionsDictionary;
    #endregion

    #region Properties
    public Texture2D Texture { get; set; }
    #endregion
}
