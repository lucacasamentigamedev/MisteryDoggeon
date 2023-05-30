using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Tileset
    {
        public int FirstGid { get; private set; }
        public string Name { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public int Spacing { get; private set; }
        public int Columns { get; private set; }
        public int TileCount { get; private set; }
        public Aiv.Fast2D.Texture Source { get; private set; }

        public List<Property> Properties;

        public Tileset(XElement _element, string _sourceDirectory)
        {
            FirstGid = (int?)_element.Attribute("firstgid") ?? 0;
            var external = (string)_element.Attribute("source");

            if (external != null)
            {
                XDocument tilesetFile = XDocument.Load(Path.Combine(_sourceDirectory, external));
                _element = tilesetFile.Element("tileset");
            }

            var image = _element.Element("image");
            var source = (string)image.Attribute("source");

            Source = new Aiv.Fast2D.Texture(Path.Combine(_sourceDirectory, source));

            Name = (string)_element.Attribute("name");
            TileWidth = (int)_element.Attribute("tilewidth");
            TileHeight = (int)_element.Attribute("tileheight");
            Spacing = (int?)_element.Attribute("spacing") ?? 0;
            Columns = (int?)_element.Attribute("columns") ?? ((Source.Width + Spacing) / (TileWidth + Spacing));
            TileCount = (int?)_element.Attribute("tilecount") ?? (Columns * (Source.Height + Spacing) / (TileHeight + Spacing));

            Properties = new List<Property>();
            var properties = _element.Element("properties");
            if (properties != null)
            {
                foreach (var e in properties.Elements("property"))
                    Properties.Add(new Property(e));
            }
        }

        public int VerticalOffset(int _gid)
        {
            return ((_gid - FirstGid) / Columns) * (TileHeight + Spacing);
        }

        public int HorizontalOffset(int _gid)
        {
            return ((_gid - FirstGid) % Columns) * (TileWidth + Spacing);
        }
    }
}
