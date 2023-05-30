using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public enum EStaggerIndex
    {
        Odd = 0,
        Even = 1,
    }

    public static class StaggerIndexMethods
    {
        private static Dictionary<string, EStaggerIndex> staggerDic = new Dictionary<string, EStaggerIndex> {
                {"odd", EStaggerIndex.Odd},
                {"even", EStaggerIndex.Even}
            };

        public static bool Decode(ref EStaggerIndex _eStaggerIndex, string _sValue)
        {
            if (_sValue != null)
            {
                return staggerDic.TryGetValue(_sValue, out _eStaggerIndex);
            }
            return false;
        }
    }
}
