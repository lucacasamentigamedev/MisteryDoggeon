using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public struct GameStatsSerialize {
        public bool PuzzleResolved { get; set; }
        public bool PlayerCanShoot { get; set; }
        public List<int> CollectedKeys { get; set; }
        public int PreviousRoom { get; set; }
        public int ActualRoom { get; set; }
        public bool HordeDefeated { get; set; }
        public bool BossDefeated { get; set; }
        public float PlayerHealth { get; set; }
        public bool FirstDoorPassed { get; set; }
        public GameStatsSerialize(bool PuzzleResolved, bool PlayerCanShoot,
            List<int> CollectedKeys, int PreviousRoom, int ActualRoom, bool HordeDefeated,
            bool BossDefeated, float PlayerHealth, bool FirstDoorPassed) {
            this.PuzzleResolved = PuzzleResolved;
            this.PlayerCanShoot = PlayerCanShoot;
            this.CollectedKeys = CollectedKeys;
            this.PreviousRoom = PreviousRoom;
            this.ActualRoom = ActualRoom;
            this.HordeDefeated = HordeDefeated;
            this.BossDefeated = BossDefeated;
            this.PlayerHealth = PlayerHealth;
            this.FirstDoorPassed = FirstDoorPassed;
        }
    }

    static class GameStatsMgr {

        public static bool PuzzleResolved { get; set; }
        public static bool PlayerCanShoot { get; set; }
        public static Weapon ActiveWeapon { get; set; }
        private static List<int> collectedKeys = new List<int>();
        public static List<int> CollectedKeys {
            get { return collectedKeys; }
            set { collectedKeys = value; }
        }
        public static int PreviousRoom { get; set; }
        private static int actualRoom = -1;
        public static int ActualRoom {
            get { return actualRoom;  }
            set { actualRoom = value; }
        }
        public static bool HordeDefeated { get; set; }
        public static bool BossDefeated { get; set; }
        public static float maxPlayerHealth = 20;
        private static float playerHealth = 20;
        public static float PlayerHealth {
            get { return playerHealth; }
            set { playerHealth = value; }
        }
        public static bool FirstDoorPassed { get; set; }

        public static void ResetGameStats() {
            PuzzleResolved = false;
            PlayerCanShoot = false;
            ActiveWeapon = null;
            CollectedKeys.Clear();
            PreviousRoom = 0;
            ActualRoom = -1;
            HordeDefeated = false;
            BossDefeated = false;
            playerHealth = maxPlayerHealth;
            FirstDoorPassed = false;
        }

        public static GameStatsSerialize GetGameStats() {
            return new GameStatsSerialize(PuzzleResolved, PlayerCanShoot,
                CollectedKeys, PreviousRoom, ActualRoom, HordeDefeated,
                BossDefeated, PlayerHealth, FirstDoorPassed);
        }

        public static void LoadGameStats(GameStatsSerialize gameStats) {

        }
    }
}
