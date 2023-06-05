namespace MisteryDungeon.MysteryDungeon {
    static class GameConfigMgr {

        /* DEBUG var, to active/disactive Console.Writeline*/
        public static bool debugBoxColliderWireframe = true;
        /************************************************/

        private static float tileUnitWidth;
        public static float TileUnitWidth {
            get { return tileUnitWidth; }
            set { tileUnitWidth = value; }
        }

        private static float tileUnitHeight;
        public static float TileUnitHeight {
            get { return tileUnitHeight; }
            set { tileUnitHeight = value; }
        }

        private static int roomsNumber = 4;
        public static int RoomsNumber {
            get { return roomsNumber; }
        }

        private static bool firstDoorPassed = false;
        public static bool FirstDoorPassed {
            get { return firstDoorPassed; }
            set { firstDoorPassed = value; }
        }

        private static float tilePixelWidth;
        public static float TilePixelWidth {
            get { return tilePixelWidth; }
            set { tilePixelWidth = value; }
        }

        private static float tilePixelHeight;
        public static float TilePixelHeight {
            get { return tilePixelHeight; }
            set { tilePixelHeight = value; }
        }

        private static int mapRows;
        public static int MapRows {
            get { return mapRows; }
            set { mapRows = value; }
        }

        private static int mapColumns;
        public static int MapColumns {
            get { return mapColumns; }
            set { mapColumns = value; }
        }

        private static int platformButtons = 5;
        public static int PlatformButtons {
            get { return platformButtons; }
            set { platformButtons = value; }
        }
    }
}
