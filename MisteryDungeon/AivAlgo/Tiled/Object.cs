using System;
using System.Collections.Generic;
using System.Globalization;
using OpenTK;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Object
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
        public double Rotation { get; private set; }
        public Tile? Tile { get; private set; }
        public bool Visible { get; private set; }

        public List<Vector2> Points;

        public List<Property> Properties;

        public Object(XElement _element)
        {
            Id = (int?)_element.Attribute("id") ?? 0;
            Name = (string)_element.Attribute("name") ?? String.Empty;
            X = (double)_element.Attribute("x");
            Y = (double)_element.Attribute("y");
            Width = (double?)_element.Attribute("width") ?? 0.0;
            Height = (double?)_element.Attribute("height") ?? 0.0;
            Type = (string)_element.Attribute("type") ?? String.Empty;
            Visible = (bool?)_element.Attribute("visible") ?? true;
            Rotation = (double?)_element.Attribute("rotation") ?? 0.0;
            Tile = null;

            // Assess object type and assign appropriate content
            var xGid = _element.Attribute("gid");
            var xPolygon = _element.Element("polygon");
            var xPolyline = _element.Element("polyline");

            if (xGid != null)
            {
                Tile = new Tile((uint)xGid);
            }
            else if (xPolygon != null)
            {
                Points = ParsePoints(xPolygon);
            }
            else if (xPolyline != null)
            {
                Points = ParsePoints(xPolyline);
            }

            Properties = new List<Property>();
            var properties = _element.Element("properties");
            if (properties != null)
            {
                foreach (var e in properties.Elements("property"))
                    Properties.Add(new Property(e));
            }
        }

        private List<Vector2> ParsePoints(XElement _element)
        {
            var points = new List<Vector2>();

            var pointString = (string)_element.Attribute("points"); // points="1,2 4,5 17,21 57.3,21.8"
            var pointStringPair = pointString.Split(' ');
            foreach (var s in pointStringPair)
            {
                var pt = s.Split(',');
                points.Add(new Vector2(float.Parse(pt[0], NumberStyles.Float,
                                 CultureInfo.InvariantCulture), float.Parse(pt[1], NumberStyles.Float,
                                 CultureInfo.InvariantCulture)));
            }
            return points;
        }
    }
}
