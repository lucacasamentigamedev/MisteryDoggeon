using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public struct WeaponSerialized {
        public int WeaponType { get; set; }
        public int BulletType { get; set; }
        public float ReloadTime { get; set; }
        public float OffsetShootX { get; set; }
        public float OffsetShootY { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public WeaponSerialized(WeaponType WeaponType, BulletType BulletType,
            float ReloadTime, Vector2 OffsetShoot, int RoomId, int Id) {
            this.WeaponType = (int)WeaponType;
            this.BulletType = (int)BulletType;
            this.ReloadTime = ReloadTime;
            OffsetShootX = OffsetShoot.X;
            OffsetShootY = OffsetShoot.Y;
            this.RoomId = RoomId;
            this.Id = Id;
        }
    }

    public class Weapon : UserComponent {

        public WeaponType WeaponType { get; set; }
        public BulletType BulletType { get; set; }
        public float ReloadTime { get; set; }
        public Vector2 OffsetShoot { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public Weapon(GameObject owner, WeaponType weaponType, BulletType bulletType,
            float reloadTime, Vector2 offsetShoot, int roomId, int id) : base(owner) {
            WeaponType = weaponType;
            ReloadTime = reloadTime;
            BulletType = bulletType;
            OffsetShoot = offsetShoot;
            RoomId = roomId;
            Id = id;
        }

        public WeaponSerialized GetSerializedWeapon() {
            return new WeaponSerialized(WeaponType, BulletType, ReloadTime, OffsetShoot, RoomId, Id);
        }
    }
}
