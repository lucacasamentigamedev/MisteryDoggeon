using Aiv.Fast2D.Component;
using System;

namespace MisteryDungeon.MysteryDungeon {
    public class LogMgr : UserComponent {

        private bool debugPathfinding;
        public bool DebugPathfinding {
            get { return debugPathfinding; }
            set {
                debugPathfinding = value;
                if (debugPathfinding) EventManager.AddListener(EventList.LOG_Pathfinding, OnConsoleLog);
            }
        }

        private bool debugPuzzle;
        public bool DebugPuzzle {
            get { return debugPuzzle; }
            set {
                debugPuzzle = value;
                if (debugPuzzle) EventManager.AddListener(EventList.LOG_Puzzle, OnConsoleLog);
            }
        }

        private bool debugGameObjectCreations;
        public bool DebugGameObjectCreations {
            get { return debugGameObjectCreations; }
            set {
                debugGameObjectCreations = value;
                if (debugGameObjectCreations) EventManager.AddListener(EventList.LOG_GameObjectCreation, OnConsoleLog);
            }
        }

        private bool debugEnemyHorde;
        public bool DebugEnemyHorde {
            get { return debugEnemyHorde; }
            set {
                debugEnemyHorde = value;
                if (debugEnemyHorde) EventManager.AddListener(EventList.LOG_EnemyHorde, OnConsoleLog);
            }
        }

        private bool debugMemoryCard;
        public bool DebugMemoryCard {
            get { return debugMemoryCard; }
            set {
                debugMemoryCard = value;
                if (debugMemoryCard) EventManager.AddListener(EventList.LOG_MemoryCard, OnConsoleLog);
            }
        }

        public LogMgr(GameObject owner, bool debugPathfinding = true, bool debugPuzzle = true,
            bool debugGameObjectCreations = true, bool debugEnemyHorde = true, bool debugMemoryCard = true) : base(owner) {
            DebugPathfinding = debugPathfinding;
            DebugPuzzle = debugPuzzle;
            DebugGameObjectCreations = debugGameObjectCreations;
            DebugEnemyHorde = debugEnemyHorde;
            DebugMemoryCard = debugMemoryCard;
        }

        public override void OnDestroy() {
            if(DebugGameObjectCreations) EventManager.RemoveListener(EventList.LOG_GameObjectCreation, OnConsoleLog);
            if(DebugPathfinding) EventManager.RemoveListener(EventList.LOG_Pathfinding, OnConsoleLog);
            if(DebugPuzzle) EventManager.RemoveListener(EventList.LOG_Puzzle, OnConsoleLog);
            if(DebugEnemyHorde) EventManager.RemoveListener(EventList.LOG_EnemyHorde, OnConsoleLog);
            if(DebugMemoryCard) EventManager.RemoveListener(EventList.LOG_MemoryCard, OnConsoleLog);
        }

        public void OnConsoleLog(EventArgs message) {
            EventArgsFactory.LOG_Parser(message, out string m);
            Console.WriteLine(m);
        }
    }
}
