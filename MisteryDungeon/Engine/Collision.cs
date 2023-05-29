using OpenTK;


namespace Aiv.Fast2D.Component {

    public enum CollisionType { None, RectsInteresction}

    public struct Collision {

        public CollisionType Type;
        public Vector2 Delta;
        public Collider Collider;

    }
}
