using MisteryDungeon.MysteryDungeon;
using System;

namespace Aiv.Fast2D.Component {
    public static class EventArgsFactory {

        public static EventArgs EnemySpawnedFactory() {
            return new EventArgs();
        }

        public static void EnemySpawnedParser() { }

        public static EventArgs EnemyDespawnFactory() {
            return new EventArgs();
        }

        public static void EnemyDespawnParser() { }

        public static EventArgs SpawnPointDestroyedFactory() {
            return new EventArgs();
        }

        public static void SpawnPointDestroyedParser() {}

        public static EventArgs PlatformButtonPressedFactory(PlatformButton platformButton) {
            return new PlatformButtonEventArgs(platformButton);
        }

        public static void PlatformButtonPressedParser(EventArgs message, out PlatformButton platformButton) {
            PlatformButtonEventArgs parsedMessage = (PlatformButtonEventArgs)message;
            platformButton = parsedMessage.PlatformButton;
        }

        public static EventArgs LOG_Factory(string message) {
            return new SingleStringEventArg(message);
        }

        public static void LOG_Parser(EventArgs message, out string m) {
            SingleStringEventArg parsedMessage = (SingleStringEventArg)message;
            m = parsedMessage.StringParameter;
        }

        public static void SequenceWrongParser() { }

        public static EventArgs SequenceWrongFactory() {
            return new EventArgs();
        }

        public static void SequenceRightParser() { }

        public static EventArgs SequenceRightFactory() {
            return new EventArgs();
        }

        public static void SequenceCompletedParser() { }

        public static EventArgs SequenceCompletedFactory() {
            return new EventArgs();
        }

        public static void ActionNotAllowedParser() { }

        public static EventArgs ActionNotAllowedFactory() {
            return new EventArgs();
        }

        public static void ObjectPickedParser() { }

        public static EventArgs ObjectPickedFactory() {
            return new EventArgs();
        }

        public static void PuzzleReadyParser() { }

        public static EventArgs PuzzleReadyFactory() {
            return new EventArgs();
        }

        public static void ClockTickParser() { }

        public static EventArgs ClockTickFactory() {
            return new EventArgs();
        }
        
        public static void ArrowShotParser() { }

        public static EventArgs ArrowShotFactory() {
            return new EventArgs();
        }

        public static void ObjectDestroyedParser() { }

        public static EventArgs ObjectDestroyedFactory() {
            return new EventArgs();
        }
        
        public static void RoomLeftParser() { }

        public static EventArgs RoomLeftFactory() {
            return new EventArgs();
        }

        public static void PlayerTakesDamageParser() { }

        public static EventArgs PlayerTakesDamageFactory() {
            return new EventArgs();
        }

        public static void PlayerDeadParser() { }

        public static EventArgs PlayerDeadFactory() {
            return new EventArgs();
        }

        public static void EnemyTakesDamageParser() { }

        public static EventArgs EnemyTakesDamageFactory() {
            return new EventArgs();
        }
        
        public static void EnemyDeadParser() { }

        public static EventArgs EnemyDeadFactory() {
            return new EventArgs();
        }
        
        public static void BossDefeatedParser() { }

        public static EventArgs BossDefeatedFactory() {
            return new EventArgs();
        }
        
        public static void HordeDefeatedParser() { }

        public static EventArgs HordeDefeatedFactory() {
            return new EventArgs();
        }

        public static void GamePlayParser() { }

        public static EventArgs GamePlayFactory() {
            return new EventArgs();
        }

        public static void GamePauseParser() { }

        public static EventArgs GamePauseFactory() {
            return new EventArgs();
        }

        public static void NewGameParser() { }

        public static EventArgs NewGameFactory() {
            return new EventArgs();
        }

        public static void LoadGameParser() { }

        public static EventArgs LoadGameFactory() {
            return new EventArgs();
        }

        public static void SaveGameParser() { }

        public static EventArgs SaveGameFactory() {
            return new EventArgs();
        }

        public static void StartLoadingParser() { }

        public static EventArgs StartLoadingFactory() {
            return new EventArgs();
        }

        public static void EndLoadingParser() { }

        public static EventArgs EndLoadingFactory() {
            return new EventArgs();
        }
        
        public static void SpawnPointHittedParser() { }

        public static EventArgs SpawnPointHittedFactory() {
            return new EventArgs();
        }
    }
}
