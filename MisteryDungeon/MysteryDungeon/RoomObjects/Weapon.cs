using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class Weapon : UserComponent {

        public WeaponType WeaponType { get; set; }
        public BulletType BulletType { get; set; }
        public float ReloadTime { get; set; }

        public Vector2 OffsetShoot { get; set; }

        public Weapon(GameObject owner, WeaponType weaponType, BulletType bulletType, float reloadTime, Vector2 offsetShoot) : base(owner) {
            WeaponType = weaponType;
            ReloadTime = reloadTime;
            BulletType = bulletType;
            OffsetShoot = offsetShoot;
        }
    }
}
