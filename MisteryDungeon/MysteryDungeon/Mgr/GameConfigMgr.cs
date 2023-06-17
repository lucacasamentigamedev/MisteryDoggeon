namespace MisteryDungeon.MysteryDungeon {
    static class GameConfigMgr {

        public static string gameStatsFileName = "GameStats.json";
        public static string movementGridFileName = "MovementGrid.json";
        public static string roomObjectsFileName = "RoomObjects.json";
        public static string weaponFileName = "Weapon.json";

        public static bool debugBoxColliderWireframe = true;
        private static int roomsNumber = 4;
        public static int RoomsNumber {
            get { return roomsNumber; }
        }
        private static int backgroundMusicNumber = 10;
        public static int BackgroundMusicNumber {
            get { return backgroundMusicNumber; }
        }
    }
}
