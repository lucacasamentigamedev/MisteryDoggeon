using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public class CircleCollider : Collider {

        public float Radius;

        public CircleCollider(GameObject owner, float radius, Vector2 offset) : base(owner, offset) {
            Radius = radius;
        }

        public override bool Collides(Collider collider, ref Collision collisionInfo) {
            return collider.Collides(this, ref collisionInfo); //visitors 
        }

        public override bool Collides(CircleCollider collider, ref Collision collisionInfo) {
            //la parte commentata è molto, MOLTO MOLTO LENTA, perché per la distanza abbiamo il calcolo di una radice quadrata
            //float dist = (Position - collider.Position).Length;
            //return dist <= Radius + collider.Radius;


            float dist = (Position - collider.Position).LengthSquared;
            return dist <= Math.Pow(Radius + collider.Radius, 2);
        }

        public override bool Collides(BoxCollider collider, ref Collision collisionInfo) {
            return collider.Collides(this, ref collisionInfo);
        }

        public override bool Contains(Vector2 point) {
            float dist = (Position - point).LengthSquared;
            return dist <= Radius * Radius;
        }

    }
}
