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

        public static void SetRoomObjectActiveness(int roomId, int objectId, bool isActive) {
            if (!roomObjects[roomId].ContainsKey(objectId)) return;
            roomObjects[roomId][objectId] = isActive;
            if (isActive) return;
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
            Console.WriteLine("Setto false per l'oggetto " + objectId + " e Floor nella cella " + cellPos.ToString());
            GameGridMgr.ChangeGridTileType(cellPos, roomId, MovementGrid.EGridTile.Floor);
        }
    }
}
