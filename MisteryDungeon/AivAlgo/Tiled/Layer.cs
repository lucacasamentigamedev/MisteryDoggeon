using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Layer
    {
        public Tile[,] Tiles;

        public string Name { get; private set; }
        public double Opacity { get; private set; }
        public bool Visible { get; private set; }
        public double OffsetX { get; private set; }
        public double OffsetY { get; private set; }
        public List<Property> Properties;

        public Layer(XElement _element, int _width, int _height)
        {
            Name = (string)_element.Attribute("name");
            Opacity = (double?)_element.Attribute("opacity") ?? 1.0;
            Visible = (bool?)_element.Attribute("visible") ?? true;
            OffsetX = (double?)_element.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)_element.Attribute("offsety") ?? 0.0;

            var xData = _element.Element("data");
            var encoding = (string)xData.Attribute("encoding");

            Tiles = new Tile[_width, _height];
            if (encoding == null)
            {
                int k = 0;
                foreach (var tile in xData.Elements("tile"))
                {
                    var gid = (uint?)tile.Attribute("gid") ?? 0;

                    var x = k % _width;
                    var y = k / _width;

                    Tiles[x, y] = new Tile(gid);
                    k++;
                }
            }
            else if (encoding == "base64")
            {
                var decodedStream = new Base64Loader(xData);
                var stream = decodedStream.Data;

                using (var br = new BinaryReader(stream))
                    for (int y = 0; y < _height; y++)
                        for (int x = 0; x < _width; x++)
                            Tiles[x, y] = new Tile(br.ReadUInt32());
            }
            else if (encoding == "csv") // Comma Separated Values
            {
                var csvData = (string)xData.Value;
                int k = 0;
                foreach (var s in csvData.Split(','))
                {
                    var gid = uint.Parse(s.Trim());
                    var x = k % _width;
                    var y = k / _width;
                    Tiles[x, y] = new Tile(gid);
                    k++;
                }
            }

            var properties = _element.Element("properties");
            if (properties != null)
            {
                Properties = new List<Property>();
                foreach (var e in properties.Elements("property"))
                    Properties.Add(new Property(e));
            }
        }
    }
}
