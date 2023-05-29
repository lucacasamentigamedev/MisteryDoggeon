using OpenTK;

namespace Aiv.Fast2D.Component.UI {
    public class Font {

        protected int numCol;
        protected int firstVal;
        public int CharacterWidth {
            get;
            protected set;
        }
        public int CharacterHeight {
            get;
            protected set;
        }
        public string TextureName {
            get;
            protected set;
        }
        public Texture Texture {
            get;
            protected set;
        }

        public Font (string textureName, string texturePath, int numCol, int firstChar,
            int charWidth, int charHeight) {
            TextureName = textureName;
            Texture = GfxMgr.AddTexture(textureName, texturePath);
            firstVal = firstChar;
            CharacterWidth = charWidth;
            CharacterHeight = charHeight;
            this.numCol = numCol;
        }

        public Vector2 GetOffset (char c) {
            int cVal = (int)c;
            int delta = cVal - firstVal;
            int x = delta % numCol;
            int y = delta / numCol;
            return new Vector2(x * CharacterWidth, y * CharacterHeight);
        }


    }
}
