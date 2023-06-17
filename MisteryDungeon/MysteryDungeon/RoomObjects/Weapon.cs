using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public class WeaponSerialized {
        public int WeaponType { get; set; }
        public int BulletType { get; set; }
        public float ReloadTime { get; set; }
        public string OffsetShoot { get; set; }
        public WeaponSerialized(WeaponType WeaponType, BulletType BulletType,
            float ReloadTime, Vector2 OffsetShoot) {
            this.WeaponType = (int)WeaponType;
            this.BulletType = (int)BulletType;
            this.ReloadTime = ReloadTime;
            this.OffsetShoot = OffsetShoot.ToString();
        }
    }

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

        public WeaponSerialized GetSerializedWeapon() {
            return new WeaponSerialized(WeaponType, BulletType, ReloadTime, OffsetShoot);
        }
    }
}
