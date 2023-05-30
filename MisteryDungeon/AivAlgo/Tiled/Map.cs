using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Map
    {
        private string sourceDirectory;
        private EOrientation orientation;
        private ERenderOrder renderOrder;
        private EStaggerAxis staggerAxis;
        private EStaggerIndex staggerIndex;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }
        public EOrientation Orientation { get { return orientation; } private set { orientation = value; } }
        public ERenderOrder RenderOrder { get { return renderOrder; } private set { renderOrder = value; } }

        public int HexSideLength { get; private set; }
        public EStaggerAxis StaggerAxis { get { return staggerAxis; } private set { staggerAxis = value; } }
        public EStaggerIndex StaggerIndex { get { return staggerIndex; } private set { staggerIndex = value; } }

        public Color BackgroundColor { get; private set; }
        public List<Tileset> Tilesets { get; private set; }
        public List<Layer> Layers { get; private set; }
        public List<ObjectGroup> ObjectGroups { get; private set; }
        public List<Property> Properties { get; private set; }

        public Map(string _filePath)
        {
            XDocument src = XDocument.Load(_filePath);
            sourceDirectory = Path.GetDirectoryName(_filePath);
            Load(src);
        }

        public void Load(XDocument _source)
        {
            var map = _source.Element("map");

            Width = (int)map.Attribute("width");
            Height = (int)map.Attribute("height");
            TileWidth = (int)map.Attribute("tilewidth");
            TileHeight = (int)map.Attribute("tileheight");
            HexSideLength = (int?)map.Attribute("hexsidelength") ?? 0;

            OrientationMethods.Decode(ref orientation, (string)map.Attribute("orientation"));
            RenderOrderMethods.Decode(ref renderOrder, (string)map.Attribute("renderorder"));
            StaggerAxisMethods.Decode(ref staggerAxis, (string)map.Attribute("staggeraxis"));
            StaggerIndexMethods.Decode(ref staggerIndex, (string)map.Attribute("staggerindex"));

            BackgroundColor = new Color(map.Attribute("backgroundcolor"));

            Tilesets = new List<Tileset>();
            foreach (var tileset in map.Elements("tileset"))
                Tilesets.Add(new Tileset(tileset, sourceDirectory));

            Layers = new List<Layer>();
            foreach (var layer in map.Elements("layer"))
                Layers.Add(new Layer(layer, Width, Height));

            ObjectGroups = new List<ObjectGroup>();
            foreach (var objectGroup in map.Elements("objectgroup"))
                ObjectGroups.Add(new ObjectGroup(objectGroup));

            Properties = new List<Property>();
            var properties = map.Element("properties");
            if (properties != null)
            {
                foreach (var e in properties.Elements("property"))
                    Properties.Add(new Property(e));
            }
        }
    }
}
