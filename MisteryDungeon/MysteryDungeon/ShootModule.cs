using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class ShootModule : UserComponent {

        private float reloadTime;
        public float ReloadTime {
            get { return reloadTime; }
            set { reloadTime = value; }
        }
        private float currentReloadTime;
        private Vector2 offsetShoot;
        public Vector2 OffsetShoot {
            get { return offsetShoot; }
            set { offsetShoot = value; }
        }

        private BulletType bulletType;
        public BulletType BulletType {
            get { return bulletType; }
            set { bulletType = value; }
        }
        private string shootAction;
        private bool isEnemy;

        private BulletMgr bulletMgr;
        private Transform targetTransform;

        public ShootModule(GameObject owner, string shootAction, bool isEnemy) : base(owner) {
            this.shootAction = shootAction;
            this.isEnemy = isEnemy;
        }

        public override void Awake() {
            bulletMgr = GameObject.Find("BulletMgr").GetComponent<BulletMgr>();
            currentReloadTime = 0;
        }

        public override void Start() {
            if(isEnemy) targetTransform = GameObject.Find("Player").transform;
        }

        public override void Update() {
            currentReloadTime -= Game.DeltaTime;
            if (currentReloadTime <= 0) {
                if(isEnemy || (!isEnemy && Input.GetUserButton(shootAction) && GameStats.PlayerCanShoot) ) {
                    Vector2 direction = !isEnemy ?
                        Game.Win.MousePosition - transform.Position :
                        targetTransform.Position - transform.Position;
                    Vector2 startPosition = transform.Position + direction.Normalized() * 0.5f;
                    if (Shoot(startPosition, direction)) {
                        currentReloadTime = reloadTime;
                    }
                }
            }
        }

        public bool Shoot(Vector2 startPosition, Vector2 velocity) {
            Bullet bullet = bulletMgr.GetBullet(bulletType);
            if (bullet == null) return false;
            if(bulletType == BulletType.Arrow || bulletType == BulletType.GoldArrow)
                EventManager.CastEvent(EventList.ArrowShot, EventArgsFactory.ArrowShotFactory());
            bullet.Shoot(startPosition, velocity);
            return true;
        }

        public void SetWeapon(BulletType bulletType, float reloadTime, Vector2 offsetShoot) {
            BulletType = bulletType;
            ReloadTime = reloadTime;
            OffsetShoot = offsetShoot;
        }
    }
}
