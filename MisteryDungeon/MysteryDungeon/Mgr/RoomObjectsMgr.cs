﻿using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    static class RoomObjectsMgr {

        private static Dictionary<int, bool>[] roomObjects;

        public static Dictionary<int, bool>[] RoomObjects {
            get { return roomObjects; }
        }

        static RoomObjectsMgr() {
            ResetRoomObjects();
        }

        public static bool AddRoomObjectActiveness(int objectId, bool isActive) {
            int roomId = TiledMapMgr.RoomId;
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
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Setto" + activeness + "per l'oggetto " + objectId));
            if (!changeGridType) return;
            float xPos = 0;
            float yPos = 0;
            Map map = TiledMapMgr.GetMap(roomId);
            foreach(ObjectGroup objectGroup in map.ObjectGroups) {
                foreach (Aiv.Tiled.Object obj in objectGroup.Objects) {
                    if (obj.Id != objectId) continue;
                    xPos = (float)obj.X;
                    yPos = (float)obj.Y;
                }
            }
            Vector2 cellPos = new Vector2(
                (int)Math.Ceiling(xPos / TiledMapMgr.TilePixelWidth),
                (int)Math.Ceiling(yPos / TiledMapMgr.TilePixelHeight) - 1
            );
            MovementGridMgr.ChangeGridTileType(cellPos, roomId, gridType);
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Setto " + gridType + " nella cella " + cellPos.ToString()));
        }

        public static void ResetRoomObjects() {
            roomObjects = new Dictionary<int, bool>[GameConfigMgr.RoomsNumber];
            for (int i = 0; i < GameConfigMgr.RoomsNumber; i++) {
                roomObjects[i] = new Dictionary<int, bool>();
            }
        }

        public static void SetRoomObjectsMap(Dictionary<int, bool> ro, int index) {
            roomObjects[index] = ro;
        }

        public static void LoadRoomObjects(Dictionary<int, bool>[] roomObjects) {
            for(int i = 0; i < roomObjects.Length; i++) {
                if (roomObjects[i].Count <= 0) continue;
                SetRoomObjectsMap(roomObjects[i], i);
            }
            Console.WriteLine();
        }
    }
}
