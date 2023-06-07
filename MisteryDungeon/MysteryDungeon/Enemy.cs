using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class Enemy : UserComponent {

        private float damage;
        public float Damage {
            get { return damage; }
        }
        public float speed;
        public float currentDeathTimer;
        private bool dead;
        public bool Dead {
            get { return dead; }
        }
        private float deathTimer;

        protected Rigidbody rigidbody;
        protected Transform targetTransform;
        private SheetAnimator animator;

        public Enemy(GameObject owner, float speed, float damage, float deathTimer) : base(owner) {
            this.speed = speed;
            this.damage = damage;
            dead = false;
            this.deathTimer = deathTimer;
            currentDeathTimer = deathTimer;
        }

        public override void Awake() {
            animator = GetComponent<SheetAnimator>();
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Start() {
            targetTransform = GameObject.Find("Player").transform;
        }

        public override void Update() {
            if (dead) {
                currentDeathTimer -= Game.DeltaTime;
                if (currentDeathTimer > 0) return;
                DestroyEnemy();
                return;
            }
            Vector2 direction = targetTransform.Position - transform.Position;
            rigidbody.Velocity = direction.Normalized() * speed;
        }

        public void Spawn(Vector2 startPosition) {
            transform.Position = startPosition;
            dead = false;
            currentDeathTimer = deathTimer;
            gameObject.IsActive = true;
            animator.ChangeClip("walking");
            GetComponent<HealthModule>().ResetHealth();
        }

        public void TakeDamage(float damage) {
            if (dead) return;
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) {
                rigidbody.Velocity = Vector2.Zero;
                animator.ChangeClip("death");
                dead = true;
            }
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
