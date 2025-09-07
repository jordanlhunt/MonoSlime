using System.Collections.Generic;
using System.Globalization;
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
    #region Constructors
    public TextureAtlas()
    {
        textureRegionsDictionary = new Dictionary<string, TextureRegion>();
    }

    public TextureAtlas(Texture2D texture2D)
    {
        Texture = texture2D;
        textureRegionsDictionary = new Dictionary<string, TextureRegion>();
    }
    #endregion
    #region Public Methods
    public void AddRegion(string name, int x, int y, int width, int height)
    {
        TextureRegion textureRegion = new TextureRegion(Texture, x, y, width, height);
        textureRegionsDictionary.Add(name, textureRegion);
    }

    public TextureRegion GetRegion(string name)
    {
        return textureRegionsDictionary[name];
    }

    public bool RemoveRegion(string name)
    {
        return textureRegionsDictionary.Remove(name);
    }

    public void RemoveAllRegions()
    {
        textureRegionsDictionary.Clear();
    }

    public static TextureAtlas FromFile(ContentManager contentManager, string fileName)
    {
        TextureAtlas textureAtlas = new TextureAtlas();
        string filePath = Path.Combine(contentManager.RootDirectory, fileName);
        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            using (XmlReader xmlReader = XmlReader.Create(stream))
            {
                XDocument xDocument = XDocument.Load(xmlReader);
                XElement root = xDocument.Root;
                string texturePath = root.Element("Texture").Value;
                textureAtlas.Texture = contentManager.Load<Texture2D>(texturePath);
                XElement regionsContainer = root.Element("Regions");
                if (regionsContainer != null)
                {
                    IEnumerable<XElement> regions = regionsContainer.Elements("Region");
                    foreach (var region in regions)
                    {
                        string name = null;
                        XAttribute nameAttribute = region.Attribute("name");
                        if (nameAttribute != null)
                        {
                            name = nameAttribute.Value;
                        }
                        int x = 0;
                        XAttribute xAttribute = region.Attribute("x");
                        if (xAttribute != null)
                        {
                            x = int.Parse(xAttribute.Value);
                        }
                        int y = 0;
                        XAttribute yAttribute = region.Attribute("y");
                        if (yAttribute != null)
                        {
                            y = int.Parse(yAttribute.Value);
                        }
                        int width = 0;
                        XAttribute widthAttribute = region.Attribute("width");
                        if (widthAttribute != null)
                        {
                            width = int.Parse(widthAttribute.Value);
                        }
                        int height = 0;
                        XAttribute heightAttribute = region.Attribute("height");
                        if (heightAttribute != null)
                        {
                            height = int.Parse(heightAttribute.Value);
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            textureAtlas.AddRegion(name, x, y, width, height);
                        }
                    }
                }
                return textureAtlas;
            }
        }
    }
    #endregion
}
