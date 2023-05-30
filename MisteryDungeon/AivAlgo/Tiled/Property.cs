using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Property
    {
        private EType type;
        public string Name { get; private set; }
        public EType Type { get { return type; } private set { type = value; } }
        public string Value { get; private set; }

        public Property(XElement _element)
        {
            Name = (string)_element.Attribute("name") ?? String.Empty;

            TypeMethods.Decode(ref type, (string)_element.Attribute("type"));

            Value = (string)_element.Attribute("value");
        }

        public string AsString()
        {
            if (Type != EType.EString)
            {
                throw new FormatException($"Property [{Name}] is accessed as String but value type is [{Type.ToString()}]");
            }
            return Value;
        }

        public int AsInt()
        {
            if (Type != EType.EInt)
            {
                throw new FormatException($"Property [{Name}] is accessed as Int but value type is [{Type.ToString()}]");
            }
            return int.Parse(Value);
        }

        public float AsFloat()
        {
            if (Type != EType.EFloat)
            {
                throw new FormatException($"Property [{Name}] is accessed as Float but value type is [{Type.ToString()}]");
            }
            return float.Parse(Value);
        }

        public bool AsBool()
        {
            if (Type != EType.EBool)
            {
                throw new FormatException($"Property [{Name}] is accessed as Bool but value type is [{Type.ToString()}]");
            }
            return bool.Parse(Value);
        }

        public Color AsColor()
        {
            if (Type != EType.EColor)
            {
                throw new FormatException($"Property [{Name}] is accessed as Color but value type is [{Type.ToString()}]");
            }
            return new Color(Value);
        }

        public string AsFile()
        {
            if (Type != EType.EFile)
            {
                throw new FormatException($"Property [{Name}] is accessed as File but value type is [{Type.ToString()}]");
            }
            return Value;
        }
    }
}
