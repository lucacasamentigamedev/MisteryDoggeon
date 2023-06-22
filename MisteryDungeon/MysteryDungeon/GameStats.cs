using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public struct GameStatsSerialized {
        public bool PuzzleResolved { get; set; }
        public bool PlayerCanShoot { get; set; }
        public List<int> CollectedKeys { get; set; }
        public List<int> CollectedWeapons { get; set; }
        public int PreviousRoom { get; set; }
        public int ActualRoom { get; set; }
        public int HordesDefeated { get; set; }
        public bool BossDefeated { get; set; }
        public float PlayerHealth { get; set; }
        public bool FirstDoorPassed { get; set; }
        public float ElapsedTime { get; set; }
        public int EnemiesKilled { get; set; }
        public int ArrowsShot { get; set; }
        public int ObjectsDestroyed { get; set; }

        public GameStatsSerialized(bool PuzzleResolved, bool PlayerCanShoot,
            List<int> CollectedKeys, List<int> CollectedWeapons, int PreviousRoom,
            int ActualRoom, int HordesDefeated, bool BossDefeated,
            float PlayerHealth, bool FirstDoorPassed, float ElapsedTime,
            int EnemiesKilled, int ArrowsShot, int ObjectsDestroyed) {
            this.PuzzleResolved = PuzzleResolved;
            this.PlayerCanShoot = PlayerCanShoot;
            this.CollectedKeys = CollectedKeys;
            this.CollectedWeapons = CollectedWeapons;
            this.PreviousRoom = PreviousRoom;
            this.ActualRoom = ActualRoom;
            this.BossDefeated = BossDefeated;
            this.PlayerHealth = PlayerHealth;
            this.FirstDoorPassed = FirstDoorPassed;
            this.HordesDefeated = HordesDefeated;
            this.ElapsedTime = ElapsedTime;
            this.EnemiesKilled = EnemiesKilled;
            this.ArrowsShot = ArrowsShot;
            this.ObjectsDestroyed = ObjectsDestroyed;
        }
    }

    static class GameStats {

        public static bool PuzzleResolved { get; set; }
        public static bool PlayerCanShoot { get; set; }
        public static Weapon ActiveWeapon { get; set; }
        public static List<int> collectedWeapons { get; set; }
        public static List<int> CollectedWeapons {
            get { return collectedWeapons; }
            set { collectedWeapons = value; }
        }
        private static List<int> collectedKeys;
        public static List<int> CollectedKeys {
            get { return collectedKeys; }
            set { collectedKeys = value; }
        }
        public static int PreviousRoom { get; set; }
        public static int ActualRoom { get; set; }
        public static int HordesDefeated { get; set; }
        public static bool BossDefeated { get; set; }
        public static float maxPlayerHealth = 20;
        public static float MaxPlayerHealth {
            get { return maxPlayerHealth; }
            set { maxPlayerHealth = value; }
        }
        private static float playerHealth = 20;
        public static float PlayerHealth {
            get { return playerHealth; }
            set { playerHealth = value; }
        }
        public static bool FirstDoorPassed { get; set; }
        public static float ElapsedTime { get; set; }
        public static int EnemiesKilled { get; set; }
        public static int ArrowsShot { get; set; }
        public static int ObjectsDestroyed { get; set; }

        static GameStats() {
            CollectedKeys = new List<int>();
            CollectedWeapons = new List<int>();
        }
    }
}
