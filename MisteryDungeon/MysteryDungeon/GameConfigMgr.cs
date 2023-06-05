namespace MisteryDungeon.MysteryDungeon {
    static class GameConfigMgr {

        public static bool debugBoxColliderWireframe = false;
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
        private static int platformButtons = 5;
        public static int PlatformButtons {
            get { return platformButtons; }
            set { platformButtons = value; }
        }
    }
}
