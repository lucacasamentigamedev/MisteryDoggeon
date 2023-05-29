using System;

namespace Aiv.Fast2D.Component {

    public enum EventList {
        //BulletDestroyed,
        //CannonShooted,
        //BoxDestroyed,
        //PlayerHPChanged,
        //BulletSpawned
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

    public class SingleIntDoubleFloatEventArgs : EventArgs {

        private int intParameter;
        public int IntParameter {
            get { return intParameter; }
        }
        private float firstFloatParameter;
        public float FirstFloatParameter {
            get { return firstFloatParameter; }
        }
        private float secondFloatParameter;
        public float SecondFloatParameter {
            get { return secondFloatParameter; }
        }

        public SingleIntDoubleFloatEventArgs(int intParameter, float firstFloatParameter, 
            float secondFloatParameter) {
            this.intParameter = intParameter;
            this.firstFloatParameter = firstFloatParameter;
            this.secondFloatParameter = secondFloatParameter;
        }

    }

    //public class BulletSpawnedEventArgs : EventArgs {
    //    private Bullet bullet;
    //    public Bullet Bullet {
    //        get { return bullet; }
    //    }

    //    public BulletSpawnedEventArgs (Bullet bullet) {
    //        this.bullet = bullet;
    //    }
    //}

}
