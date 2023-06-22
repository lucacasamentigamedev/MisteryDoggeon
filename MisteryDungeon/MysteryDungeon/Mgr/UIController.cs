using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;

namespace MisteryDungeon.MysteryDungeon.Mgr {
    public class UIController : UserComponent {
        public UIController(GameObject owner) : base(owner) {}

        TextBox puzzleTimer;

        public override void Awake() {
            puzzleTimer = GameObject.Find("PuzzleTimer").GetComponent<TextBox>();
        }

        public void ActivatePuzzleTimer() {
            puzzleTimer.Enabled = true;
        }

        public void DisactivePuzzleTimer() {
            puzzleTimer.Enabled = false;
        }

        public void SetPuzzleTimerCountdownText(string text) {
            puzzleTimer.SetText(text);
        }
    }
}
