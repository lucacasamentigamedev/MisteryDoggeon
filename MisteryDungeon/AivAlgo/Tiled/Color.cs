using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Aiv.Tiled
{
    public class Color
    {
        public int R { get; private set; }
        public int G { get; private set; }
        public int B { get; private set; }
        public int A { get; private set; }

        public Color(XAttribute xColor)
        {
            R = 0;
            G = 0;
            B = 0;
            A = 255;
            if (xColor != null)
            {
                DecodeColor((string)xColor);
            }
        }

        public Color(string sColor)
        {
            R = 0;
            G = 0;
            B = 0;
            A = 255;
            DecodeColor(sColor);
        }

        private void DecodeColor(string sColor)
        {
            if (sColor == null) return; // #AARRGGBB

            var colorStr = sColor.TrimStart("#".ToCharArray()); // AARRGGBB

            int i = 0;
            if (colorStr.Length > 6)
            {
                A = int.Parse(colorStr.Substring(i, 2), NumberStyles.HexNumber);
                i += 2;
            }
            else
            {
                A = 255;
            }
            R = int.Parse(colorStr.Substring(i + 0, 2), NumberStyles.HexNumber); // __RR____
            G = int.Parse(colorStr.Substring(i + 2, 2), NumberStyles.HexNumber); // ____GG__
            B = int.Parse(colorStr.Substring(i + 4, 2), NumberStyles.HexNumber); // ______BB
        }
    }
}
