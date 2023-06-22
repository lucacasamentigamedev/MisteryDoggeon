using Aiv.Fast2D.Component;
using OpenTK;
using System;

namespace MisteryDungeon.MysteryDungeon.Logic {
    public class GameStatsMgr : UserComponent {
        public GameStatsMgr(GameObject owner) : base(owner) {}

        public override void Update() {
            GameStats.ElapsedTime += Game.DeltaTime;
        }

        public override void Start() {
            EventManager.AddListener(EventList.EnemyDead, OnEnemyDead);
            EventManager.AddListener(EventList.ArrowShot, OnArrowShot);
            EventManager.AddListener(EventList.ObjectDestroyed, OnObjectDestroyed);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.EnemyDead, OnEnemyDead);
            EventManager.RemoveListener(EventList.ArrowShot, OnArrowShot);
            EventManager.RemoveListener(EventList.ObjectDestroyed, OnObjectDestroyed);
        }

        public void OnEnemyDead(EventArgs message) {
            GameStats.EnemiesKilled++;
            Console.WriteLine("Ricevuto evento, nemici sconfitti: " + GameStats.EnemiesKilled);
        }

        public void OnArrowShot(EventArgs message) {
            GameStats.ArrowsShot++;
        }

        public void OnObjectDestroyed(EventArgs message) {
            GameStats.ObjectsDestroyed++;
        }

        public void ResetGameStats() {
            GameStats.PuzzleResolved = false;
            GameStats.PlayerCanShoot = false;
            GameStats.ActiveWeapon = null;
            GameStats.CollectedKeys.Clear();
            GameStats.CollectedWeapons.Clear();
            GameStats.PreviousRoom = 0;
            GameStats.ActualRoom = 0;
            GameStats.BossDefeated = false;
            GameStats.PlayerHealth = GameStats.MaxPlayerHealth;
            GameStats.FirstDoorPassed = false;
            GameStats.HordesDefeated = 0;
            GameStats.ElapsedTime = 0;
            GameStats.EnemiesKilled = 0;
            GameStats.ArrowsShot = 0;
            GameStats.ObjectsDestroyed = 0;
        }

        public GameStatsSerialized GetGameStats() {
            return new GameStatsSerialized(GameStats.PuzzleResolved, GameStats.PlayerCanShoot,
            GameStats.CollectedKeys, GameStats.CollectedWeapons, GameStats.PreviousRoom, GameStats.ActualRoom, GameStats.HordesDefeated,
            GameStats.BossDefeated, GameStats.PlayerHealth, GameStats.FirstDoorPassed, GameStats.ElapsedTime,
            GameStats.EnemiesKilled, GameStats.ArrowsShot, GameStats.ObjectsDestroyed);
        }

        public void LoadGameStats(GameStatsSerialized gameStats) {
            GameStats.PuzzleResolved = gameStats.PuzzleResolved;
            GameStats.PlayerCanShoot = gameStats.PlayerCanShoot;
            GameStats.CollectedKeys = gameStats.CollectedKeys;
            GameStats.CollectedWeapons = gameStats.CollectedWeapons;
            GameStats.PreviousRoom = gameStats.PreviousRoom;
            GameStats.ActualRoom = gameStats.ActualRoom;
            GameStats.BossDefeated = gameStats.BossDefeated;
            GameStats.PlayerHealth = gameStats.PlayerHealth;
            GameStats.FirstDoorPassed = gameStats.FirstDoorPassed;
            GameStats.HordesDefeated = gameStats.HordesDefeated;
            GameStats.ElapsedTime = gameStats.ElapsedTime;
            GameStats.EnemiesKilled = gameStats.EnemiesKilled;
            GameStats.ArrowsShot = gameStats.ArrowsShot;
            GameStats.ObjectsDestroyed = gameStats.ObjectsDestroyed;
        }

        public void LoadActiveWeapon(WeaponSerialized weapon) {
            GameObject owner = GameObject.Find("Object_" + weapon.RoomId + "_" + weapon.Id);
            GameStats.ActiveWeapon = new Weapon(owner, (WeaponType)weapon.WeaponType, (BulletType)weapon.BulletType, weapon.ReloadTime,
                new Vector2(weapon.OffsetShootX, weapon.OffsetShootY), weapon.RoomId, weapon.Id);
        }
    }
}
