using Aiv.Fast2D.Component;
using System;

namespace MisteryDungeon.MysteryDungeon {
    public class LogMgr : UserComponent {

        private static bool debugPathfinding;
        public bool DebugPathfinding {
            get { return debugPathfinding; }
            set {
                debugPathfinding = value;
                if (debugPathfinding) EventManager.AddListener(EventList.LOG_Pathfinding, OnConsoleLog);
            }
        }

        private static bool debugPuzzle;
        public bool DebugPuzzle {
            get { return debugPuzzle; }
            set {
                debugPuzzle = value;
                if (debugPuzzle) EventManager.AddListener(EventList.LOG_Puzzle, OnConsoleLog);
            }
        }

        private static bool debugGameObjectCreations;
        public bool DebugGameObjectCreations {
            get { return debugGameObjectCreations; }
            set {
                debugGameObjectCreations = value;
                if (debugGameObjectCreations) EventManager.AddListener(EventList.LOG_GameObjectCreation, OnConsoleLog);
            }
        }

        private static bool debugEnemyHorde;
        public bool DebugEnemyHorde {
            get { return debugEnemyHorde; }
            set {
                debugEnemyHorde = value;
                if (debugEnemyHorde) EventManager.AddListener(EventList.LOG_EnemyHorde, OnConsoleLog);
            }
        }

        public LogMgr(GameObject owner, bool debugPathfinding = true, bool debugPuzzle = true, bool debugGameObjectCreations = true) : base(owner) {
            DebugPathfinding = debugPathfinding;
            DebugPuzzle = debugPuzzle;
            DebugGameObjectCreations = debugGameObjectCreations;
        }

        public override void OnDestroy() {
            if(DebugGameObjectCreations) EventManager.RemoveListener(EventList.LOG_GameObjectCreation, OnConsoleLog);
            if(DebugPathfinding) EventManager.RemoveListener(EventList.LOG_Pathfinding, OnConsoleLog);
            if(DebugPuzzle) EventManager.RemoveListener(EventList.LOG_Puzzle, OnConsoleLog);
            if(DebugEnemyHorde) EventManager.RemoveListener(EventList.LOG_EnemyHorde, OnConsoleLog);
        }

        public void OnConsoleLog(EventArgs message) {
            EventArgsFactory.LOG_Parser(message, out string m);
            Console.WriteLine(m);
        }
    }
}
