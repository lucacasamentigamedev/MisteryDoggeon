using MisteryDungeon.MysteryDungeon;
using System;

namespace Aiv.Fast2D.Component {
    public static class EventArgsFactory {

        public static EventArgs EnemySpawnedFactory() {
            return new EventArgs();
        }

        public static void EnemySpawnedParser() { }

        public static EventArgs EnemyDestroyedFactory() {
            return new EventArgs();
        }

        public static void EnemyDestroyedParser() { }

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

        public static void PathUnreachableParser() { }

        public static EventArgs PathUnreachableFactory() {
            return new EventArgs();
        }

        public static void ObjectPickedParser() { }

        public static EventArgs ObjectPickedFactory() {
            return new EventArgs();
        }
    }
}
