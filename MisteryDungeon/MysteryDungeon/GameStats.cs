namespace MisteryDungeon.MysteryDungeon {
    static class GameStats {
        private static bool puzzleResolved = false;
        public static bool PuzzleResolved {
            get { return puzzleResolved; }
            set { puzzleResolved = value; }
        }

        private static bool canShoot = false;
        public static bool CanShoot {
            get { return canShoot; }
            set { canShoot = value; }
        }

        private static bool bowPicked = false;
        public static bool BowPicked {
            get { return bowPicked; }
            set { bowPicked = value; }
        }
    }
}
