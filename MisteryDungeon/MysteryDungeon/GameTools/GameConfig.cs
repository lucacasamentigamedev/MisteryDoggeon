namespace MisteryDungeon.MysteryDungeon {
    static class GameConfig {

        public static bool debugBoxColliderWireframe = true;
        public static float TileUnitWidth { get; set; }
        public static float TileUnitHeight { get; set; }

        private static int roomsNumber = 4;
        public static int RoomsNumber {
            get { return roomsNumber; }
        }
        public static bool FirstDoorPassed { get; set; }
        public static float TilePixelWidth { get; set; }
        public static float TilePixelHeight { get; set; }
        public static int MapRows { get; set; }
        public static int MapColumns { get; set; }
    }
}
