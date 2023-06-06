using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    static class GameStats {
        public static bool PuzzleResolved { get; set; }
        public static bool CanShoot { get; set; }
        public static Weapon ActiveWeapon { get; set; }
        public static bool BowPicked { get; set; }
        public static List<int> collectedKeys = new List<int>();
        public static int PreviousRoom { get; set; }
        private static int actualRoom = -1;
        public static int ActualRoom {
            get { return actualRoom;  }
            set { actualRoom = value; }
        }
        public static bool HordeDefeated { get; set; }
        public static bool BossDefeated { get; set; }
    }
}
