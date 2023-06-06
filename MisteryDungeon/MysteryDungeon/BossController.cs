using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    internal class BossController : UserComponent {

        float readyTimer;
        float currentReadyTimer;
        float speed;
        bool active;

        private SheetAnimator animator;
        private Transform targetTransform;
        private Rigidbody rigidBody;
        private ShootModule shootModule;

        public BossController(GameObject owner, float readyTimer, float speed) : base(owner) {
            this.readyTimer = readyTimer;
            currentReadyTimer = readyTimer;
            this.speed = speed;
            this.active = false;
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
            if (GetComponent<HealthModule>().TakeDamage(damage)) {
                //TODO: suono boss sconfitto
                animator.ChangeClip("death");
                gameObject.IsActive = false;
                GameStats.BossDefeated = true;
                RoomObjectsMgr.SetRoomObjectActiveness(3, 39, false, true);
                GameObject.Find("Object_3_39").IsActive = false;
            } else {
                //TODO: suono danno al boss
            }
        }

        public override void Update() {
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
