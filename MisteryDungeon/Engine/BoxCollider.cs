using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public class BoxCollider : Collider, IDrawable{

        protected float halfWidth;
        protected float HalfWidth {
            get { return halfWidth * transform.Scale.X; }
        }
        protected float halfHeight;
        protected float HalfHeight {
            get { return halfHeight * transform.Scale.Y; }
        }

        public float Width {
            get { return 2 * HalfWidth; }
        }
        public float Height {
            get { return 2 * HalfHeight; }
        }
        public DrawLayer Layer {
            get { return DrawLayer.GUI; }
        }

        private bool debugMode;
        public bool DebugMode {
            get { return debugMode; }
            set {
                if (value == debugMode) return;
                debugMode = value;
                if (debugMode) {
                    DrawMgr.AddItem(this);
                } else {
                    DrawMgr.RemoveItem(this);
                }
            }
        }

        public BoxCollider(GameObject owner, float w, float h, 
            Vector2 offset) : base(owner, offset) {
            halfWidth = w / 2;
            halfHeight = h / 2;
        }

        public override bool Collides(Collider collider, ref Collision collisionInfo) {
            return collider.Collides(this, ref collisionInfo); //vistors
        }

        public override bool Collides(CircleCollider collider, ref Collision collisionInfo) {
            float xprc = Math.Max(Position.X - HalfWidth, Math.Min(collider.Position.X, 
                Position.X + HalfWidth)); //x del punto del rettangolo più vicino al cerchio
            float yprc = Math.Max(Position.Y - HalfHeight, Math.Min(collider.Position.Y, 
                Position.Y + HalfHeight)); //y del punto del rettangolo più vicino al cerchio
            float deltaX = collider.Position.X - xprc;
            float deltaY = collider.Position.Y - yprc;
            return (deltaX * deltaX + deltaY * deltaY) <= (collider.Radius * collider.Radius);
        }

        public override bool Collides(BoxCollider collider, ref Collision collisionInfo) {
            float deltaX = Math.Abs(collider.Position.X - Position.X) - 
                (HalfWidth + collider.HalfWidth);
            if (deltaX > 0) return false;
            float deltaY = Math.Abs(collider.Position.Y - Position.Y) - 
                (HalfHeight + collider.HalfHeight);
            if (deltaY > 0) return false;

            collisionInfo.Delta = new Vector2(-deltaX, -deltaY);
            collisionInfo.Type = CollisionType.RectsInteresction;
            return true;
        }

        public override bool Contains(Vector2 point) {
            return
                point.X >= Position.X - HalfWidth &&
                point.X <= Position.X + HalfWidth &&
                point.Y >= Position.Y - HalfHeight &&
                point.Y <= Position.Y + HalfHeight;
        }

        public override Component Clone(GameObject owner) {
            return new BoxCollider(owner, halfWidth * 2, halfHeight * 2, Offset); ;
        }

        public void Draw () {
            Sprite sprite = new Sprite(Width, Height);
            sprite.pivot = new Vector2(sprite.Width * 0.5f, sprite.Height * 0.5f);
            sprite.position = Position;
            sprite.EulerRotation = transform.Rotation;
            sprite.DrawWireframe(new Vector4(1, 0, 0, 1));
        }

    }
}
