using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class FaceVelocity : UserComponent {

        private Rigidbody rb;

        public FaceVelocity(GameObject owner) : base(owner) {}

        public override void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public override void Update() {
            if (rb.Velocity == Vector2.Zero) return;
            transform.Forward = rb.Velocity.Normalized();
        }
    }
}
