using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public enum ERenderOrder
    {
        RightDown,
        RightUp,
        LeftDown,
        LeftUp
    }

    public static class RenderOrderMethods
    {
        private static Dictionary<string, ERenderOrder> renderOrderDict = new Dictionary<string, ERenderOrder> {
                {"right-down", ERenderOrder.RightDown},
                {"right-up", ERenderOrder.RightUp},
                {"left-down", ERenderOrder.LeftDown},
                {"left-up", ERenderOrder.LeftUp}
            };

        public static bool Decode(ref ERenderOrder _eRenderOrder, string _sValue)
        {
            if (_sValue != null)
            {
                return renderOrderDict.TryGetValue(_sValue, out _eRenderOrder);
            }
            return false;
        }
    }
}
