using OpenTK;
using System;

namespace Aiv.Fast2D.Component.UI {
    public class TextBox : Component, IDrawable {

        public DrawLayer Layer { get { return DrawLayer.GUI; } }

        private Font myFont;
        private Sprite sprite;
        private Vector2[] availableCharacters_Position;
        private Vector2[] availableCharacters_Offset;
        private string currentText;

        public int MaxCharacters {
            get { return availableCharacters_Position.Length; }
        }

        public Camera Camera {
            get { return sprite.Camera; }
            set { sprite.Camera = value; }
        }

        public TextBox (GameObject owner, Font font, int maxCharacters, 
            Vector2 fontScale) : base (owner) {
            currentText = string.Empty;
            myFont = font;
            availableCharacters_Position = new Vector2[maxCharacters];
            availableCharacters_Offset = new Vector2[maxCharacters];
            sprite = new Sprite((Game.PixelsToUnit(font.CharacterWidth * fontScale.X)),
                (Game.PixelsToUnit(font.CharacterHeight * fontScale.Y)));
            DrawMgr.AddItem(this);
        }

        public void SetText (string text) {
            if (text == currentText) return;
            currentText = text;
            int xIndex = 0;
            float yPos = transform.Position.Y;
            int maxIndex = GetMax();
            for (int i = 0; i < maxIndex; i++) {
                if (currentText[i].Equals(Environment.NewLine)) {
                    yPos += sprite.Height;
                    xIndex = 0;
                    continue;
                }
                availableCharacters_Position[i].X = transform.Position.X + xIndex * sprite.Width;
                availableCharacters_Position[i].Y = yPos;
                availableCharacters_Offset[i] = myFont.GetOffset(currentText[i]);
                xIndex++;
            }
        }

        public void Draw () {
            int maxIndex = GetMax();
            for (int i = 0; i < maxIndex; i++) {
                sprite.position = availableCharacters_Position[i];
                sprite.DrawTexture(myFont.Texture, (int)availableCharacters_Offset[i].X,
                    (int)availableCharacters_Offset[i].Y, myFont.CharacterWidth,
                    myFont.CharacterHeight);
            }
        }

        private int GetMax () {
            return availableCharacters_Position.Length > 
                currentText.Length ? currentText.Length :
                availableCharacters_Position.Length;
        }

    }
}
