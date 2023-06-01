using System;

namespace Aiv.Fast2D.Component {
    public static class EventArgsFactory {

        public static EventArgs ButtonPressedFactory(int sequenceId) {
            return new SingleIntEventArgs(sequenceId);
        }

        public static void ButtonPressedParser(EventArgs message, out int sequenceId) {
            SingleIntEventArgs parsedMessage = (SingleIntEventArgs)message;
            sequenceId = parsedMessage.IntParameter;
        }
    }
}
