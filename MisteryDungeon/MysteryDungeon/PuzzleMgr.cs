using Aiv.Fast2D.Component;
using OpenTK;
using System;

namespace MisteryDungeon.MysteryDungeon {

    public class PuzzleMgr : UserComponent {

        private float puzzleTimer;
        private float currentPuzzleTimer;
        private int lastButtonPressed;
        private int buttonToPress;
        private bool puzzleActive;
        private float waitingResetPuzzleTimer;
        private float currentWaitingResetPuzzleTimer;
        private bool puzzleReady;
        private int lastRemainingSecs;
        private Vector2[] objectsToActiveAfterPuzzleResolved;
        private Vector2[] objectsToDisactiveAfterPuzzleResolved;

        public PuzzleMgr(GameObject owner, float puzzleTimer, float waitingResetPuzzleTimer,
            Vector2[] objectsToActiveAfterPuzzleResolved, Vector2[] objectsToDisactiveAfterPuzzleResolved) : base(owner) {
            this.puzzleTimer = puzzleTimer;
            this.waitingResetPuzzleTimer = waitingResetPuzzleTimer;
            ResetPuzzle();
            this.objectsToActiveAfterPuzzleResolved = objectsToActiveAfterPuzzleResolved;
            this.objectsToDisactiveAfterPuzzleResolved = objectsToDisactiveAfterPuzzleResolved;
        }

        public override void Update() {
            if (GameStats.PuzzleResolved) return;
            if (!puzzleReady) currentWaitingResetPuzzleTimer -= Game.DeltaTime;
            if (currentWaitingResetPuzzleTimer > 0) return;
            if(!puzzleReady) EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Puzzle pronto"));
            puzzleReady = true;
            if (!puzzleActive) return;
            int actualRemainingSecs = (int)currentPuzzleTimer;
            if (actualRemainingSecs != lastRemainingSecs) TickCountdown(actualRemainingSecs);
            currentPuzzleTimer -= Game.DeltaTime;
            if (currentPuzzleTimer > 0) return;
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Tempo scaduto"));
            ResetPuzzle();
        }

        public void ResetPuzzle() {
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Reset puzzle"));
            puzzleActive = false;
            currentPuzzleTimer = puzzleTimer+0.99f; //metto +0.99 così riesco effettivamente a fare un conteggio
            //che rispecchia il numero di secondi in ingresso
            lastButtonPressed = -1;
            buttonToPress = 0;
            puzzleReady = false;
            currentWaitingResetPuzzleTimer = waitingResetPuzzleTimer;
            lastRemainingSecs = 0;
        }

        public override void Start() {
            EventManager.AddListener(EventList.ButtonPressed, OnButtonPressed);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.ButtonPressed, OnButtonPressed);
        }

        public void OnButtonPressed(EventArgs message) {
            if (GameStats.PuzzleResolved || !puzzleReady) return;
            //test console.write
            if (!puzzleActive) EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Attivo il puzzle"));
            puzzleActive = true;
            EventArgsFactory.ButtonPressedParser(message, out int sequenceId);
            if (sequenceId == lastButtonPressed) return;
            if (sequenceId != buttonToPress) {
                EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Sequenza puzzle sbagliata"));
                //TODO: rumore di sequenza sbagliata
                ResetPuzzle();
                return;
            };
            lastButtonPressed = sequenceId;
            buttonToPress++;
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Premuto pulsante giusto " + buttonToPress + "/" + GameConfigMgr.PlatformButtons));
            if(buttonToPress == GameConfigMgr.PlatformButtons) {
                EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Puzzle risolto"));
                //TODO: rumore puzzle completato
                //TODO: sbloccare gate
                //TODO: spawn pistola
                GameStats.PuzzleResolved = true;
                foreach (Vector2 v in objectsToActiveAfterPuzzleResolved) {
                    GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = true;
                    RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, true);
                }
                foreach (Vector2 v in objectsToDisactiveAfterPuzzleResolved) {
                    GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = false;
                    RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, false);
                }
            }
        }

        private void TickCountdown(int actualRemainingSecs) {
            lastRemainingSecs = actualRemainingSecs;
            //TODO: suono conteggio
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Secondi rimanenti = " + lastRemainingSecs));
        }
    }
}
