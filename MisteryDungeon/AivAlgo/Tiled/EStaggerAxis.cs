using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public enum EStaggerAxis
    {
        X,
        Y
    }

    public static class StaggerAxisMethods
    {
        private static Dictionary<string, EStaggerAxis> staggerDic = new Dictionary<string, EStaggerAxis> {
                {"x", EStaggerAxis.X},
                {"y", EStaggerAxis.Y}
            };

        public static bool Decode(ref EStaggerAxis _eStaggerAxis, string _sValue)
        {
            if (_sValue != null)
            {
                return staggerDic.TryGetValue(_sValue, out _eStaggerAxis);
            }
            return false;
        }
    }
}
