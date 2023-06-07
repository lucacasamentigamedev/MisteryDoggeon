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

        private SheetAnimator animator;
        private Transform targetTransform;
        private Rigidbody rigidBody;
        private ShootModule shootModule;

        public BossController(GameObject owner, float readyTimer, float speed, float deathTimer) : base(owner) {
            currentReadyTimer = readyTimer;
            this.speed = speed;
            active = false;
            dead = false;
            currentDeathTimer = deathTimer;
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
            if (GetComponent<HealthModule>().TakeDamage(damage)) {
                //TODO: suono boss sconfitto
                rigidBody.Velocity = Vector2.Zero;
                animator.ChangeClip("death");
                dead = true;
                shootModule.Enabled = false;
            } else {
                //TODO: suono danno al boss
            }
        }

        public override void Update() {
            if (dead) {
                currentDeathTimer -= Game.DeltaTime;
                if (currentDeathTimer > 0) return;
                RoomObjectsMgr.SetRoomObjectActiveness(3, 39, false, true);
                GameObject.Find("Object_3_39").IsActive = false;
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
