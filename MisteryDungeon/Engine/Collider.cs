using OpenTK;

namespace Aiv.Fast2D.Component {
    public abstract class Collider : Component {

        public Vector2 Offset;
        public Vector2 Position {
            get { return transform.Position + Offset; }
        }

        public Collider (GameObject owner, Vector2 offset) : base (owner) {
            Offset = offset;
            Rigidbody rb = GetComponent(typeof(Rigidbody)) as Rigidbody;
            if (rb == null) return;
            rb.Collider = this;
        }

        public abstract bool Collides(Collider collider, ref Collision collisionInfo);
        public abstract bool Collides(BoxCollider collider, ref Collision collisionInfo);
        public abstract bool Collides(CircleCollider collider, ref Collision collisionInfo);

        public abstract bool Contains(Vector2 point);

    }
}
