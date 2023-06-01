using System;

namespace Aiv.Fast2D.Component {

    public enum EventList {
        ButtonPressed
    }

    public static class EventManager {

        private static Action<EventArgs>[] gameEvents; 

        static EventManager () {
            gameEvents = new Action<EventArgs>[Enum.GetValues(typeof(EventList)).Length];
        }

        public static void CastEvent (EventList eventToCast, EventArgs message) {
            gameEvents[(int)eventToCast]?.Invoke(message);
        }

        public static void AddListener (EventList eventToListen, Action<EventArgs> methodToAdd) {
            gameEvents[(int)eventToListen] += methodToAdd;
        }

        public static void RemoveListener (EventList eventToStopListen, Action<EventArgs> methodToRemove) {
            gameEvents[(int)eventToStopListen] -= methodToRemove;
        }

    }

    public class SingleIntEventArgs : EventArgs {

        private int intParameter;
        public int IntParameter {
            get { return intParameter; }
        }

        public SingleIntEventArgs(int intParameter) {
            this.intParameter = intParameter;
        }
    }
}
