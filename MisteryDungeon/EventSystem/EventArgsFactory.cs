using MisteryDungeon.MysteryDungeon;
using System;

namespace Aiv.Fast2D.Component {
    public static class EventArgsFactory {

        public static EventArgs ButtonPressedFactory(int sequenceId) {
            return new SingleIntEventArg(sequenceId);
        }

        public static void ButtonPressedParser(EventArgs message, out int sequenceId) {
            SingleIntEventArg parsedMessage = (SingleIntEventArg)message;
            sequenceId = parsedMessage.IntParameter;
        }

        public static EventArgs LOG_Factory(string message) {
            return new SingleStringEventArg(message);
        }

        public static void LOG_Parser(EventArgs message, out string m) {
            SingleStringEventArg parsedMessage = (SingleStringEventArg)message;
            m = parsedMessage.StringParameter;
        }
    }
}
