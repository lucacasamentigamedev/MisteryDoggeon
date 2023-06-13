using Aiv.Fast2D.Component;
using OpenTK;
using System;
using System.Collections.Generic;

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
        public int TotalButtons { get; set; }
        private Vector2[] objectsToActiveAfterPuzzleResolved;
        private Vector2[] objectsToDisactiveAfterPuzzleResolved;
        private List<PlatformButton> buttons;

        public PuzzleMgr(GameObject owner, float puzzleTimer, float waitingResetPuzzleTimer,
            Vector2[] objectsToActiveAfterPuzzleResolved, Vector2[] objectsToDisactiveAfterPuzzleResolved) : base(owner) {
            buttons = new List<PlatformButton>();
            this.puzzleTimer = puzzleTimer;
            this.waitingResetPuzzleTimer = waitingResetPuzzleTimer;
            ResetPuzzle();
            this.objectsToActiveAfterPuzzleResolved = objectsToActiveAfterPuzzleResolved;
            this.objectsToDisactiveAfterPuzzleResolved = objectsToDisactiveAfterPuzzleResolved;
            TotalButtons = 0;
        }

        public void AddPlatformButton(PlatformButton pb) {
            buttons.Add(pb);
        }

        public override void Update() {
            if (GameStats.PuzzleResolved) return;
            if (!puzzleReady) currentWaitingResetPuzzleTimer -= Game.DeltaTime;
            if (currentWaitingResetPuzzleTimer > 0) return;
            if (!puzzleReady) {
                EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Puzzle pronto"));
                EventManager.CastEvent(EventList.PuzzleReady, EventArgsFactory.PuzzleReadyFactory());
            };
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
            EventManager.CastEvent(EventList.SequenceWrong, EventArgsFactory.SequenceWrongFactory());
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Reset puzzle"));
            puzzleActive = false;
            currentPuzzleTimer = puzzleTimer+0.99f; //metto +0.99 così riesco effettivamente a fare un conteggio
            //che rispecchia il numero di secondi in ingresso
            lastButtonPressed = -1;
            buttonToPress = 0;
            puzzleReady = false;
            currentWaitingResetPuzzleTimer = waitingResetPuzzleTimer;
            lastRemainingSecs = 0;
            if (buttons.Count <= 0) return;
            foreach(PlatformButton button in buttons) {
                button.ChangeButtonState(false);
            }
        }

        public override void Start() {
            EventManager.AddListener(EventList.PlatformButtonPressed, OnButtonPressed);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.PlatformButtonPressed, OnButtonPressed);
        }

        public void OnButtonPressed(EventArgs message) {
            if (GameStats.PuzzleResolved || !puzzleReady) return;
            EventArgsFactory.PlatformButtonPressedParser(message, out PlatformButton platformButton);
            if (platformButton.Pressed) return;
            //test console.write
            if (!puzzleActive) EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Attivo il puzzle"));
            puzzleActive = true;
            if (platformButton.SequenceId == lastButtonPressed) return;
            if (platformButton.SequenceId != buttonToPress) {
                EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Sequenza puzzle sbagliata"));
                ResetPuzzle();
                return;
            };
            EventManager.CastEvent(EventList.SequenceRight, EventArgsFactory.SequenceRightFactory());
            platformButton.ChangeButtonState(true);
            lastButtonPressed = platformButton.SequenceId;
            buttonToPress++;
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Premuto pulsante giusto " + buttonToPress + "/" + TotalButtons));
            if(buttonToPress == TotalButtons) {
                EventManager.CastEvent(EventList.SequenceCompleted, EventArgsFactory.SequenceCompletedFactory());
                EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Puzzle risolto"));
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
            EventManager.CastEvent(EventList.ClockTick, EventArgsFactory.ClockTickFactory());
            EventManager.CastEvent(EventList.LOG_Puzzle, EventArgsFactory.LOG_Factory("Secondi rimanenti = " + lastRemainingSecs));
        }
    }
}
