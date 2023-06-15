namespace MisteryDungeon.MysteryDungeon {
    static class GameConfig {

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
