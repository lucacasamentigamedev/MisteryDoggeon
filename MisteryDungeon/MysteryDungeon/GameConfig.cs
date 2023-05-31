namespace MisteryDungeon.MysteryDungeon {
    static class GameConfig {
        private static bool firstDoorPassed = false;
        public static bool FirstDoorPassed {
            get { return firstDoorPassed; }
            set { firstDoorPassed = value; }
        }
    }
}
