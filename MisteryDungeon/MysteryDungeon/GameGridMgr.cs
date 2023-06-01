using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;

namespace MisteryDungeon.MysteryDungeon {
    static class GameGridMgr {

        private static MovementGrid[] grids;

        static GameGridMgr() {
            grids = new MovementGrid[GameConfigMgr.RoomsNumber];
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
            Console.WriteLine("Setto " + type + " in stanza " + roomId + " in pos " + pos.ToString());
            GetRoomGrid(roomId).Map[(int)pos.X, (int)pos.Y] = type;
            PrintMovementGrid(roomId);
        }

        public static void PrintMovementGrid(int roomId) {
            Console.WriteLine();
            Console.WriteLine("Mappa pathfinding");
            Console.WriteLine();
            MovementGrid grid = GameGridMgr.GetRoomGrid(roomId);
            for (int x = 0; x < grid.Map.GetLength(0); ++x) {
                for (int y = 0; y < grid.Map.GetLength(1); ++y) {
                    Console.Write((int)grid.Map[y, x] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
