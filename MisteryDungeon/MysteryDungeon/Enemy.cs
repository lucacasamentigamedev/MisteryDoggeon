using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class Enemy : UserComponent {

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
            GetComponent<HealthModule>().ResetHealth();
        }

        public void TakeDamage(float damage) {
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) DestroyEnemy();
            else EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Enemy colpito, vita rimanente " + hm.Health));
        }

        public void DestroyEnemy() {
            //TODO: suono distruzione nemico
            EventManager.CastEvent(EventList.EnemyDestroyed, EventArgsFactory.EnemyDestroyedFactory());
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Enemy distrutto"));
            gameObject.IsActive = false;
        }
    }
}
