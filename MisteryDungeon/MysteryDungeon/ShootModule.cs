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

        private BulletMgr bulletMgr;

        public ShootModule(GameObject owner) : base(owner) {}

        public override void Awake() {
            bulletMgr = GameObject.Find("BulletMgr").GetComponent<BulletMgr>();
            currentReloadTime = reloadTime;
        }

        public override void Update() {
            currentReloadTime -= Game.DeltaTime;
            if (currentReloadTime <= 0 && Input.GetUserButton("Shoot") && GameStats.CanShoot) {
                if (Shoot(transform.Position + (Game.Win.MousePosition - transform.Position).Normalized() * 0.5f, Game.Win.MousePosition - transform.Position)) {
                    currentReloadTime = reloadTime;
                }
            }
        }

        public bool Shoot(Vector2 startPosition, Vector2 velocity) {
            Bullet bullet = bulletMgr.GetBullet(bulletType);
            if (bullet == null) return false;
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
