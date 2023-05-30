using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public enum EOrientation
    {
        Unknown,
        Orthogonal,
        Isometric,
        Staggered,
        Hexagonal
    }

    public static class OrientationMethods
    {
        private static Dictionary<string, EOrientation> orientDict = new Dictionary<string, EOrientation> {
                {"unknown", EOrientation.Unknown},
                {"orthogonal", EOrientation.Orthogonal},
                {"isometric", EOrientation.Isometric},
                {"staggered", EOrientation.Staggered},
                {"hexagonal", EOrientation.Hexagonal},
            };

        public static bool Decode(ref EOrientation _eOrientation, string _sValue)
        {
            if (_sValue != null)
            {
                return orientDict.TryGetValue(_sValue, out _eOrientation);
            }
            return false;
        }
    }
}
