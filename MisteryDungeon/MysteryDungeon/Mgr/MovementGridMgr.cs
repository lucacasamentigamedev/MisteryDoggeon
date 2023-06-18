using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;
using static MisteryDungeon.AivAlgo.Pathfinding.MovementGrid;
using static MisteryDungeon.MysteryDungeon.Utility.JsonFileUtils;

namespace MisteryDungeon.MysteryDungeon {

    public struct MovementGridToSerialize {
        public MovementGrid[] Grids { get; set; }  
        public MovementGridToSerialize(MovementGrid[] Grids) {
            this.Grids = Grids;
        }
    }

    static class MovementGridMgr {

        private static MovementGrid[] grids;
        public static MovementGrid[] Grids {
            get { return grids;}
            set { grids = value;}
        }
        static MovementGridMgr() {
            ResetMovementsGrids();
        }

        public static MovementGrid GetRoomGrid(int roomId) {
            return grids[roomId];
        }

        public static void SetRoomGrid(int roomId, MovementGrid grid) {
            grids[roomId] = grid;
        }

        public static void ChangeGridTileType(Vector2 pos, int roomId, MovementGrid.EGridTile type) {
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Setto " + type + " in stanza " + roomId + " in pos " + pos.ToString()));
            GetRoomGrid(roomId).map[(int)pos.X, (int)pos.Y] = type;
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(PrintMovementGrid(roomId)));
        }

        public static string PrintMovementGrid(int roomId) {
            string final = "";
            final += "\n";
            final += "Mappa pathfinding\n";
            final += "\n";
            MovementGrid grid = GetRoomGrid(roomId);
            for (int x = 0; x < grid.map.GetLength(0); ++x) {
                for (int y = 0; y < grid.map.GetLength(1); ++y) {
                    final += (int)grid.map[y, x] + " ";
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

        public static void ResetMovementsGrids() {
            grids = new MovementGrid[GameConfigMgr.RoomsNumber];
            for (int i = 0; i < grids.Length; i++) {
                grids[i] = null;
            }
        }

        public static MovementGridToSerialize GetMovementsGrids() {
            return new MovementGridToSerialize(Grids);
        }
        public static void LoadMovementsGrids(List<MovementGridArrayElemSerialized> movementsGrids) {
            for(int i = 0; i < movementsGrids.Count; i++) {
                if (movementsGrids[i] == null) continue;
                var loadedGrid = movementsGrids[i].Map;
                int MapRows = loadedGrid.Count;
                int MapColumns = loadedGrid[0].Count;
                int[,] loadedGridConverted = new int[MapRows, MapColumns];
                for(int j= 0; j < MapRows; j++) {
                    for(int k = 0; k < MapColumns; k++) {
                        loadedGridConverted[k, j] = loadedGrid[j][k];
                    }
                }
                SetRoomGrid(i, new MovementGrid(MapRows, MapColumns, loadedGridConverted));
            }
            Console.WriteLine();
        }
    }
}
