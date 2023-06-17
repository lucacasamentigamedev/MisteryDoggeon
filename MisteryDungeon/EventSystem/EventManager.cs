using MisteryDungeon.MysteryDungeon;
using System;

namespace Aiv.Fast2D.Component {

    public enum EventList {
        // LogMgr events
        LOG_Pathfinding,
        LOG_GameObjectCreation,
        LOG_Puzzle,
        LOG_EnemyHorde,
        LOG_Boss,
        LOG_MemoryCard,
        // Game events
        EnemySpawned,
        EnemyDestroyed,
        SpawnPointDestroyed,
        ObjectBroke,
        ObjectPicked,
        PlatformButtonPressed,
        SequenceRight,
        SequenceCompleted,
        SequenceWrong,
        ArrowShot,
        PathUnreachable,
        PuzzleReady,
        ClockTick,
        RoomLeft,
        PlayerTakesDamage,
        PlayerDead,
        EnemyTakesDamage,
        EnemyDead,
        BossDefeated,
        HordeDefeated,
        GamePause,
        GamePlay,
        NewGame,
        SaveGame,
        LoadGame,
        StartLoading,
        EndLoading
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

    public class SpawnPointCreatedEventArgs : EventArgs {
        private SpawnPoint spawnPoint;
        public SpawnPoint SpawnPoint {
            get { return spawnPoint; }
        }

        public SpawnPointCreatedEventArgs(SpawnPoint spawnPoint) {
            this.spawnPoint = spawnPoint;
        }
    }

    public class PlatformButtonEventArgs : EventArgs {
        private PlatformButton platformButton;
        public PlatformButton PlatformButton {
            get { return platformButton; }
        }

        public PlatformButtonEventArgs(PlatformButton platformButton) {
            this.platformButton = platformButton;
        }
    }
}
