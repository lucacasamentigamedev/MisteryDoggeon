using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public class SpriteRenderer : Component, IDrawable {

        public Camera Camera {
            get { return mySprite.Camera; }
            set { mySprite.Camera = value; }
        }
        private Texture myTexture;
        public Texture Texture {
            get { return myTexture; }
            set { myTexture = value; }
        }
        private Sprite mySprite;
        public Sprite Sprite {
            get { return mySprite; }
        }
        private DrawLayer layer;
        public DrawLayer Layer {
            get { return layer; }
            set { layer = value; }
        }
        private Vector2 pivot; //in coordinate normalizzate
        public Vector2 Pivot {
            get { return pivot; }
            set {
                pivot = value;
                mySprite.pivot = new Vector2(mySprite.Width * pivot.X, 
                    mySprite.Height * pivot.Y);
            }
        }
        private float height;
        public float Height {
            set {
                height = value;
            }
            get {
                return height * transform.Scale.Y;
            }
        }
        public float HeightUnscaled {
            get {
                return height;
            }
        }
        private float width;
        public float Width {
            get {
                return width * transform.Scale.X;
            }
            set {
                width = value;
            }
        }

        public float WidthUnscaled {
            get {
                return width;
            }
        }

        private Vector2 textureOffset;
        public Vector2 TextureOffset {
            get { return textureOffset; }
            set {
                textureOffset = value;
            }
        }


        public SpriteRenderer(GameObject owner, string textureName, Vector2 pivot, 
            DrawLayer layer) : base(owner) {
            myTexture = GfxMgr.GetTexture(textureName);
            width = Game.PixelsToUnit(myTexture.Width);
            height = Game.PixelsToUnit(myTexture.Height);
            mySprite = new Sprite(width * transform.Scale.X, 
                height * transform.Scale.Y);
            textureOffset = Vector2.Zero;
            Pivot = pivot;
            this.layer = layer;
            DrawMgr.AddItem(this);
        }

        public SpriteRenderer(GameObject owner, Texture texture, Vector2 pivot,
            DrawLayer layer) : base(owner) {
            myTexture = texture;
            width = Game.PixelsToUnit(myTexture.Width);
            height = Game.PixelsToUnit(myTexture.Height);
            mySprite = new Sprite(width * transform.Scale.X,
                height * transform.Scale.Y);
            textureOffset = Vector2.Zero;
            Pivot = pivot;
            this.layer = layer;
            DrawMgr.AddItem(this);
        }

        public SpriteRenderer (GameObject owner, string textureName, Vector2 pivot,
            DrawLayer layer, float width, float height) : base (owner) {
            myTexture = GfxMgr.GetTexture(textureName);
            this.width = Game.PixelsToUnit(width);
            this.height = Game.PixelsToUnit(height);
            mySprite = new Sprite(this.width * transform.Scale.X,
                this.height * transform.Scale.Y);
            textureOffset = Vector2.Zero;
            Pivot = pivot;
            this.layer = layer;
            DrawMgr.AddItem(this);
        }

        public SpriteRenderer(GameObject owner, Texture texture, Vector2 pivot,
            DrawLayer layer, float width, float height) : base(owner) {
            myTexture = texture;
            this.width = Game.PixelsToUnit(width);
            this.height = Game.PixelsToUnit(height);
            mySprite = new Sprite(this.width * transform.Scale.X,
                this.height * transform.Scale.Y);
            textureOffset = Vector2.Zero;
            Pivot = pivot;
            this.layer = layer;
            DrawMgr.AddItem(this);
        }

        public SpriteRenderer (GameObject owner) : base (owner) {
            DrawMgr.AddItem(this);
        }

        public void Draw () {
            mySprite.position = transform.Position;
            mySprite.scale = transform.Scale;
            mySprite.EulerRotation = transform.Rotation;
            mySprite.DrawTexture(myTexture, (int)textureOffset.X,
                (int)textureOffset.Y,(int)Game.UnitToPixels(width), 
                (int)Game.UnitToPixels(height));
        }

        public override Component Clone(GameObject owner) {
            SpriteRenderer clone = new SpriteRenderer(owner);
            clone.layer = layer;
            clone.myTexture = myTexture;
            clone.mySprite = new Sprite(mySprite.Width, mySprite.Height);
            clone.width = width;
            clone.height = height;
            clone.Pivot = Pivot;
            clone.textureOffset = textureOffset;
            clone.Camera = Camera;
            return clone;
        }

        public static SpriteRenderer Factory (GameObject owner, string textureName, Vector2 pivot, 
            DrawLayer drawLayer) {
            return new SpriteRenderer(owner, textureName, pivot, drawLayer);
        }

        public static SpriteRenderer Factory(GameObject owner, Texture texture, Vector2 pivot,
            DrawLayer drawLayer) {
            return new SpriteRenderer(owner, texture, pivot, drawLayer);
        }

        public static SpriteRenderer Factory (GameObject owner, string textureName, Vector2 pivot,
            DrawLayer drawLayer, float width, float height) {
            return new SpriteRenderer(owner, textureName, pivot, drawLayer, width, height);
        }

        public static SpriteRenderer Factory(GameObject owner, Texture texture, Vector2 pivot,
           DrawLayer drawLayer, float width, float height) {
            return new SpriteRenderer(owner, texture, pivot, drawLayer, width, height);
        }

    }
}
