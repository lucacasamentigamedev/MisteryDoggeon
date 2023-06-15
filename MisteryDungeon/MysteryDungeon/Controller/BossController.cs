using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    internal class BossController : UserComponent {

        private float currentReadyTimer;
        private float currentDeathTimer;
        private float speed;
        private bool active;
        private bool dead;
        public bool Dead {
            get { return dead; }
        }
        private Vector2[] objectsToDisactiveAfterBossDefeated;
        private SheetAnimator animator;
        private Transform targetTransform;
        private Rigidbody rigidBody;
        private ShootModule shootModule;

        public BossController(GameObject owner, float readyTimer, float speed,
            float deathTimer, Vector2[] objectsToDisactiveAfterBossDefeated) : base(owner) {
            currentReadyTimer = readyTimer;
            this.speed = speed;
            active = false;
            dead = false;
            currentDeathTimer = deathTimer;
            this.objectsToDisactiveAfterBossDefeated = objectsToDisactiveAfterBossDefeated;
        }
        public override void Awake() {
            animator = GetComponent<SheetAnimator>();
            shootModule = GetComponent<ShootModule>();
            rigidBody = GetComponent<Rigidbody>();
        }

        public override void Start() {
            targetTransform = GameObject.Find("Player").transform;
        }

        public void TakeDamage(float damage) {
            if (dead) return;
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) {
                EventManager.CastEvent(EventList.EnemyDead, EventArgsFactory.EnemyDeadFactory());
                rigidBody.Velocity = Vector2.Zero;
                animator.ChangeClip("death");
                dead = true;
                shootModule.Enabled = false;
            } else {
                EventManager.CastEvent(EventList.LOG_Boss, EventArgsFactory.LOG_Factory("Boss colpito, vita rimanente " + hm.Health));
                EventManager.CastEvent(EventList.EnemyTakesDamage, EventArgsFactory.EnemyTakesDamageFactory());
            }
        }

        public override void Update() {
            if (dead) {
                currentDeathTimer -= Game.DeltaTime;
                if (currentDeathTimer > 0) return;
                EventManager.CastEvent(EventList.BossDefeated, EventArgsFactory.BossDefeatedFactory());
                EventManager.CastEvent(EventList.LOG_Boss, EventArgsFactory.LOG_Factory("Boss sconfitto"));
                foreach (Vector2 v in objectsToDisactiveAfterBossDefeated) {
                    GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = false;
                    RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, false);
                }
                GameStats.BossDefeated = true;
                gameObject.IsActive = false;
                return;
            }
            currentReadyTimer -= Game.DeltaTime;
            if (currentReadyTimer > 0) {
                shootModule.Enabled = false;
                return;
            };
            if (!active) {
                shootModule.Enabled = true;
                animator.ChangeClip("walking");
            };
            active = true;
            Vector2 direction = targetTransform.Position - transform.Position;
            rigidBody.Velocity = direction.Normalized() * speed;
        }
    }
}
