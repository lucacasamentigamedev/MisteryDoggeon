using System;
using Aiv.Fast2D.Component;
using Aiv.Tiled;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class LittleBlobController : UserComponent {

        private float damage;
        public float Damage {
            get { return damage; }
        }
        public float currentDeathTimer;
        private bool dead;
        public bool Dead {
            get { return dead; }
        }
        private float deathTimer;
        private float speed;
        private Rigidbody rigidbody;
        private Transform targetTransform;
        private SheetAnimator animator;

        public LittleBlobController(GameObject owner, float damage, float deathTimer) : base(owner) {
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

        public void Spawn(Vector2 startPosition, float speed) {
            transform.Position = startPosition;
            dead = false;
            this.speed = speed;
            currentDeathTimer = deathTimer;
            gameObject.IsActive = true;
            animator.ChangeClip("walking");
            GetComponent<HealthModule>().ResetHealth();
        }

        public void TakeDamage(float damage) {
            if (dead) return;
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) {
                EventManager.CastEvent(EventList.EnemyDead, EventArgsFactory.EnemyDeadFactory());
                rigidbody.Velocity = Vector2.Zero;
                animator.ChangeClip("death");
                dead = true;
            } else {
                EventManager.CastEvent(EventList.EnemyTakesDamage, EventArgsFactory.EnemyTakesDamageFactory());
                EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Enemy colpito, vita rimanente " + hm.Health));
            }
        }

        public void DestroyEnemy() {
            EventManager.CastEvent(EventList.EnemyDestroyed, EventArgsFactory.EnemyDestroyedFactory());
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Enemy distrutto"));
            gameObject.IsActive = false;
        }
    }
}
