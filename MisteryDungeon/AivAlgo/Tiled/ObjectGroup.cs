using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class ObjectGroup
    {
        private EDrawOrder drawOrder;
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public EDrawOrder DrawOrder { get { return drawOrder; } private set { drawOrder = value; } }

        public double Opacity { get; private set; }
        public bool Visible { get; private set; }
        public double OffsetX { get; private set; }
        public double OffsetY { get; private set; }

        public List<Object> Objects;

        public List<Property> Properties;

        public ObjectGroup(XElement _element)
        {
            Name = (string)_element.Attribute("name") ?? String.Empty;
            Color = new Color(_element.Attribute("color"));
            Opacity = (double?)_element.Attribute("opacity") ?? 1.0;
            Visible = (bool?)_element.Attribute("visible") ?? true;
            OffsetX = (double?)_element.Attribute("offsetx") ?? 0.0;
            OffsetY = (double?)_element.Attribute("offsety") ?? 0.0;

            DrawOrderMethods.Decode(ref drawOrder, (string)_element.Attribute("draworder"));

            Objects = new List<Object>();
            foreach (var e in _element.Elements("object"))
                Objects.Add(new Object(e));

            Properties = new List<Property>();
            var properties = _element.Element("properties");
            if (properties != null)
            {
                foreach (var e in properties.Elements("property"))
                    Properties.Add(new Property(e));
            }
        }
    }
}
