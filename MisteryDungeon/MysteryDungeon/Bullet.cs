using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace Aiv.Fast2D.Component {
    public class Bullet : UserComponent {

        private BulletType bulletType;
        public BulletType BulletType {
            get { return BulletType; }
        }

        private float damage;
        public float Damage {
            get { return damage; }
        }
        protected Rigidbody rb;

        public Bullet(GameObject owner, float damage, BulletType bulletType) : base(owner) {
            this.damage = damage;
            this.bulletType = bulletType;
        }

        public override void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public override void Start() {
            gameObject.IsActive = false;
        }

        public override void LateUpdate() {
            if (!CameraMgr.InsideCameraLimits(transform.Position) && transform.Position.Y > 0) {
                DestroyBullet();
            }
        }

        public virtual void Shoot(Vector2 startPosition, Vector2 velocity) {
            transform.Position = startPosition;
            rb.Velocity = velocity;
            gameObject.IsActive = true;
        }

        public void DestroyBullet() {
            gameObject.IsActive = false;
        }

        public override void OnCollide(Collision collisionInfo) {}
    }
}
