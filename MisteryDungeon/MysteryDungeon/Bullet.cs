using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class Bullet : UserComponent {

        private BulletType bulletType;
        public BulletType BulletType {
            get { return bulletType; }
        }

        private float damage;
        public float Damage {
            get { return damage; }
        }

        public float speed;

        protected Rigidbody rb;
        protected SpriteRenderer sr;

        public Bullet(GameObject owner, float damage, BulletType bulletType, float speed) : base(owner) {
            this.damage = damage;
            this.bulletType = bulletType;
            this.speed = speed;
        }

        public override void Awake() {
            rb = GetComponent<Rigidbody>();
            sr = GetComponent<SpriteRenderer>();
        }

        public override void LateUpdate() {
            if((transform.Position.X - sr.Pivot.X) > Game.Win.OrthoWidth) DestroyBullet();
            else if((transform.Position.X + sr.Pivot.X) < 0) DestroyBullet();
            if ((transform.Position.Y - sr.Pivot.Y) > Game.Win.OrthoHeight) DestroyBullet();
            else if ((transform.Position.Y + sr.Pivot.Y) < 0) DestroyBullet();
        }

        public virtual void Shoot(Vector2 startPosition, Vector2 direction) {
            transform.Position = startPosition;
            rb.Velocity = direction.Normalized() * speed;
            gameObject.IsActive = true;
        }

        public void DestroyBullet() {
            gameObject.IsActive = false;
        }

        public override void OnCollide(Collision collisionInfo) {}
    }
}
