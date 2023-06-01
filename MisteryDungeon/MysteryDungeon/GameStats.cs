namespace MisteryDungeon.MysteryDungeon {
    static class GameStats {
        private static bool puzzleResolved = false;
        public static bool PuzzleResolved {
            get { return puzzleResolved; }
            set { puzzleResolved = value; }
        }
    }
}
