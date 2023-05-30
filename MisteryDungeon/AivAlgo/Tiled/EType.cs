using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiv.Tiled
{
    public enum EType
    {
        EString,
        EInt,
        EFloat,
        EBool,
        EColor,
        EFile
    }

    public static class TypeMethods
    {
        private static Dictionary<string, EType> typeDict = new Dictionary<string, EType> {
                {"string", EType.EString},
                {"int", EType.EInt},
                {"float", EType.EFloat},
                {"bool", EType.EBool},
                {"color", EType.EColor},
                {"file", EType.EFile}
            };

        public static bool Decode(ref EType _eType, string _sValue)
        {
            if (_sValue != null)
            {
                return typeDict.TryGetValue(_sValue, out _eType);
            }
            return false;
        }
    }
}
