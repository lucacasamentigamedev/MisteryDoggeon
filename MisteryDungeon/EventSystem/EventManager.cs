using System;

namespace Aiv.Fast2D.Component {

    public enum EventList {
        LOG_Pathfinding,
        LOG_GameObjectCreation,
        LOG_Puzzle,
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

    public class SingleIntEventArg : EventArgs {

        private int intParameter;
        public int IntParameter {
            get { return intParameter; }
        }

        public SingleIntEventArg(int intParameter) {
            this.intParameter = intParameter;
        }
    }
    
    public class SingleStringEventArg : EventArgs {

        private string stringParameter;
        public string StringParameter {
            get { return stringParameter; }
        }

        public SingleStringEventArg(string stringParameter) {
            this.stringParameter = stringParameter;
        }
    }
}
