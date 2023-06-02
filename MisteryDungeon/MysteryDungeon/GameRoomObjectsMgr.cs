using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    static class GameRoomObjectsMgr {

        private static Dictionary<int, bool>[] roomObjects;

        static GameRoomObjectsMgr() {
            roomObjects = new Dictionary<int, bool>[GameConfigMgr.RoomsNumber];
            for (int i = 0; i < GameConfigMgr.RoomsNumber; i++) {
                roomObjects[i] = new Dictionary<int, bool>();
            }
        }

        public static bool AddRoomObjectActiveness(int roomId, int objectId, bool isActive) {
            if (!roomObjects[roomId].ContainsKey(objectId)) {
                roomObjects[roomId].Add(objectId, isActive);
                return isActive;
            } else {
                return roomObjects[roomId][objectId];
            }
        }

        public static void SetRoomObjectActiveness(int roomId, int objectId, bool activeness,
            bool changeGridType = true, MovementGrid.EGridTile gridType = MovementGrid.EGridTile.Floor) {
            if (!roomObjects[roomId].ContainsKey(objectId)) return;
            roomObjects[roomId][objectId] = activeness;
            if (activeness) return;
            float xPos = 0;
            float yPos = 0;
            Map map = GameMapMgr.GetMap(roomId);
            foreach(ObjectGroup objectGroup in map.ObjectGroups) {
                foreach (Aiv.Tiled.Object obj in objectGroup.Objects) {
                    if (obj.Id != objectId) continue;
                    xPos = (float)obj.X;
                    yPos = (float)obj.Y;
                }
            }
            Vector2 cellPos = new Vector2(
                (int)Math.Ceiling(xPos / GameConfigMgr.TilePixelWidth),
                (int)Math.Ceiling(yPos / GameConfigMgr.TilePixelHeight) - 1
            );
            if (GameConfigMgr.debugPathfinding) Console.WriteLine("Setto false per l'oggetto " + objectId + " e Floor nella cella " + cellPos.ToString());
            if(changeGridType) GameGridMgr.ChangeGridTileType(cellPos, roomId, gridType);
        }
    }
}
