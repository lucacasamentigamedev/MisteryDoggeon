using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System.Collections.Generic;
using System.IO;
using static MisteryDungeon.AivAlgo.Pathfinding.MovementGrid;

namespace MisteryDungeon.MysteryDungeon {
    static class MovementGridMgr {

        private static MovementGrid[] grids;

        static MovementGridMgr() {
            grids = new MovementGrid[GameConfig.RoomsNumber];
            for (int i = 0; i < grids.Length; i++) {
                grids[i] = null;
            }
        }

        public static MovementGrid GetRoomGrid(int roomId) {
            return grids[roomId];
        }

        public static void SetRoomGrid(int roomId, MovementGrid grid) {
            grids[roomId] = grid;
        }

        public static void ChangeGridTileType(Vector2 pos, int roomId, MovementGrid.EGridTile type) {
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Setto " + type + " in stanza " + roomId + " in pos " + pos.ToString()));
            GetRoomGrid(roomId).Map[(int)pos.X, (int)pos.Y] = type;
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(PrintMovementGrid(roomId)));
        }

        public static string PrintMovementGrid(int roomId) {
            string final = "";
            final += "\n";
            final += "Mappa pathfinding\n";
            final += "\n";
            MovementGrid grid = GetRoomGrid(roomId);
            for (int x = 0; x < grid.Map.GetLength(0); ++x) {
                for (int y = 0; y < grid.Map.GetLength(1); ++y) {
                    final += (int)grid.Map[y, x] + " ";
                }
                final += "\n";
            }
            final += "\n";
            return final;
        }

        public static string PrintPathfindingPath(List<Vector2> path) {
            string final = "";
            if (path.Count > 0) {
                final += "Percorso: ";
                foreach (var point in path) {
                    final += "(" + point.X + "," + point.Y + ") ";
                }
                final += "\n";
            } else {
                final += "Nessun percorso disponibile\n";
            }
            return final;
        }

        public static EGridTile GetGridTile(int roomId, Vector2 cell) {
            return GetRoomGrid(roomId).GetGridType(cell);
        }
    }
}
