namespace MisteryDungeon.MysteryDungeon {
    static class GameConfigMgr {

        public static string gameStatsFileName = "GameStats.json";
        public static string movementGridFileName = "MovementGrid.json";
        public static string roomObjectsFileName = "RoomObjects.json";
        public static string weaponFileName = "Weapon.json";

        public static bool debugBoxColliderWireframe = false;
        private static int roomsNumber = 5;
        public static int RoomsNumber {
            get { return roomsNumber; }
        }
        private static int hordesNumber = 2;
        public static int HordesNumber {
            get { return hordesNumber; }
        }
        private static int backgroundMusicNumber = 10;
        public static int BackgroundMusicNumber {
            get { return backgroundMusicNumber; }
        }
    }
}
