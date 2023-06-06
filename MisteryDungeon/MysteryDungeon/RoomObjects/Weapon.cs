using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class Weapon : UserComponent {

        private float reloadTime;
        public float ReloadTime {
            get { return reloadTime; }
            set { reloadTime = value; }
        }
        private BulletType bulletType;
        public BulletType BulletType {
            get { return bulletType; }
            set { bulletType = value; }
        }

        private Vector2 offsetShoot;
        public Vector2 OffsetShoot {
            get { return offsetShoot; }
            set { offsetShoot = value; }
        }

        public Weapon(GameObject owner, BulletType bulletType, float reloadTime, Vector2 offsetShoot) : base(owner) {
            this.reloadTime = reloadTime;
            this.bulletType = bulletType;
            this.offsetShoot = offsetShoot;
        }
    }
}
