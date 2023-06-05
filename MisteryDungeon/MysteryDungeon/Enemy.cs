using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    internal class Enemy : UserComponent {

        private float damage;
        public float Damage {
            get { return damage; }
        }
        public float speed;
        protected Rigidbody rigidbody;
        protected Transform targetTransform;

        public Enemy(GameObject owner, float speed, float damage) : base(owner) {
            this.speed = speed;
            this.damage = damage;
        }

        public override void Start() {
            rigidbody = GetComponent<Rigidbody>();
            targetTransform = GameObject.Find("Player").transform;
        }

        public override void Update() {
            Vector2 direction = targetTransform.Position - transform.Position;
            rigidbody.Velocity = direction.Normalized() * speed;
        }

        public void Spawn(Vector2 startPosition) {
            transform.Position = startPosition;
            gameObject.IsActive = true;
        }

        public void DestroyEnemy() {
            gameObject.IsActive = false;
        }
    }
}
