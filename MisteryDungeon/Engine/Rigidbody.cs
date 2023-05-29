using OpenTK;

namespace Aiv.Fast2D.Component {
    public class Rigidbody : Component {

        protected uint collisionMask;
        public RigidbodyType Type;
        public bool IsCollisionAffected;

        public Collider Collider { get; set; }

        private float friction;
        public float Friction {
            get { return friction; }
            set {
                if (value < 0) return;
                friction = value;
            }
        }

        public Vector2 Velocity;
        public bool IsGravityAffected;

        public bool IsActive {
            get { return gameObject.IsActive; }
        }

        public Rigidbody(GameObject gameObject) : base (gameObject) {
            IsCollisionAffected = true;
            PhysicsMgr.AddItem(this);
            Collider = GetComponent(typeof(Collider)) as Collider;
        }

        public virtual void FixedUpdate() {
            if (IsGravityAffected) {
                Velocity.Y += Game.Gravity * Game.DeltaTime;
            }
            if (Velocity.LengthSquared > 0) {
                float fAmount = Friction * Game.DeltaTime;
                float newVelocityLength = Velocity.Length - fAmount;
                if (newVelocityLength < 0) newVelocityLength = 0;
                Velocity = Velocity.Normalized() * newVelocityLength;
            }
            transform.Position += Velocity * Game.DeltaTime;
        }

        public void AddCollisionType(uint add) {
            collisionMask |= add;
        }

        public void RemoveCollisionType(uint add) {
            collisionMask &= ~add;
        }

        public bool CanInteract(RigidbodyType type) {
            return ((uint)type & collisionMask) != 0;
        }

        public bool Collides(Rigidbody other, ref Collision collisionInfo) {
            return Collider.Collides(other.Collider, ref collisionInfo);
        }

    }
}
