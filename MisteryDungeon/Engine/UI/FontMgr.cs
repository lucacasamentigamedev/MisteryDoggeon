using System.Collections.Generic;

namespace Aiv.Fast2D.Component.UI {
    public static class FontMgr {

        private static Dictionary<string, Font> fonts;
        private static Font defaultFont;

        static FontMgr () {
            fonts = new Dictionary<string, Font>();
            defaultFont = null;
        }

        public static Font AddFont (string fontName, string texturePath, int numCol,
            int firstChar, int charWidth, int charHeight) {
            Font font = new Font(fontName, texturePath, numCol, firstChar,
                charWidth, charHeight);
            fonts.Add(fontName, font);
            if (defaultFont == null) defaultFont = font;
            return font;
        }

        public static Font GetFont (string fontName) {
            if (fonts.ContainsKey(fontName)) {
                return fonts[fontName];
            }
            return defaultFont;
        }

        public static void ClearAll () {
            fonts.Clear();
            defaultFont = null;
        }


    }
}
