using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using MisteryDungeon.MysteryDungeon.Rooms;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {

    static class GameMapMgr {

        private static Map[] maps;
        private static int roomId;

        static GameMapMgr() {
            maps = new Map[GameConfigMgr.RoomsNumber];
            for(int i = 0; i < maps.Length; i++) {
                maps[i] = null;
            }
        }

        public static void CreateMap(int id) {
            roomId = id;
            if (maps[roomId] == null) maps[id] = new Map("Assets/Tiled/Room" + roomId + ".tmx");
            GameConfigMgr.TileUnitWidth = (float)Game.Win.OrthoWidth / maps[id].Width;
            GameConfigMgr.TileUnitHeight = (float)Game.Win.OrthoHeight / maps[id].Height;
            GameConfigMgr.TilePixelWidth = maps[id].TileWidth;
            GameConfigMgr.TilePixelHeight = maps[id].TileHeight;
            GameConfigMgr.MapRows = maps[id].Width;
            GameConfigMgr.MapColumns = maps[id].Height;

            //Background & Collisions layers
            foreach (Layer layer in maps[id].Layers) {
                if (layer.Name == "Background") {
                    CreateBackground(layer);
                } else if (layer.Name == "Collisions") {
                    CreatePathfindingMap(layer);
                }
            }
            //Objects layers
            foreach (ObjectGroup objectGroup in maps[id].ObjectGroups) {
                for (int i = 0; i < objectGroup.Objects.Count; i++) {
                    CreateObjectTile(objectGroup.Objects[i]);
                }
            }
            //other
            CreateBulletMgr();
        }
        private static void CreateBackground(Layer layer) {
            for (int i = 0; i < layer.Tiles.GetLength(0); i++) {
                for (int j = 0; j < layer.Tiles.GetLength(1); j++) {
                    CreateTile(new TileSprite(maps[roomId].Tilesets, layer.Tiles[i, j]), i, j);
                }
            }
        }

        private static void CreateTile(TileSprite tileSprite, int xIndex, int yIndex) {
            Vector2 pos = new Vector2(
                GameConfigMgr.TileUnitWidth * xIndex + (GameConfigMgr.TileUnitWidth / 2),
                GameConfigMgr.TileUnitHeight * yIndex + (GameConfigMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Background_Tile_" + xIndex + "_" + yIndex, pos);
            go.AddComponent(SpriteRenderer.Factory(go, tileSprite.Texture, Vector2.One * 0.5f, DrawLayer.Background, GameConfigMgr.TilePixelWidth, GameConfigMgr.TilePixelWidth));
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.TextureOffset = new Vector2(tileSprite.OffsetX, tileSprite.OffsetY);
            go.transform.Scale = new Vector2(GameConfigMgr.TileUnitWidth / sr.Width, GameConfigMgr.TileUnitHeight / sr.Height);
            if(GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreatePathfindingMap(Layer layer) {
            if (GameGridMgr.GetRoomGrid(roomId) == null) {
                if (GameConfigMgr.debugPathfinding) Console.WriteLine("Mappa stanza " + roomId + " non presente, la creo ex novo");
                GameGridMgr.SetRoomGrid(roomId, new MovementGrid(GameConfigMgr.MapRows, GameConfigMgr.MapColumns, layer));
            } else {
                if (GameConfigMgr.debugPathfinding) Console.WriteLine("Mappa stanza " + roomId + " esistente, uso quella");
            };
            if (GameConfigMgr.debugPathfinding) GameGridMgr.PrintMovementGrid(roomId);
        }

        private static void CreateObjectTile(Aiv.Tiled.Object obj) {
            switch (obj.Name) {
                case "obstacle":
                    CreateObstacle(obj);
                    break;
                case "platformButton":
                    CreatePlatformButton(obj);
                    break;
                case "gate":
                    CreateGate(obj);
                    break;
                case "door":
                    CreateDoor(obj);
                    break;
                case "weapon":
                    CreateWeapon(obj);
                    break;
                case "player":
                    bool defaultPos = bool.Parse(getPropertyValueByName("startPosition", obj.Properties));
                    if (defaultPos && !GameConfigMgr.FirstDoorPassed) {
                        //disegno player in posizione di default (prima volta che apri il gioco)
                        CreatePlayer(obj);
                    } else if (!defaultPos && GameConfigMgr.FirstDoorPassed) {
                        //disegno player a fianco della porta in uscita (come da mappa tiled)
                        CreatePlayer(obj);
                    }
                    break;
            }
        }

        private static void CreateObstacle(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitHeight - (GameConfigMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Obstacle;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "crate", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfigMgr.TileUnitWidth / sr.Width, GameConfigMgr.TileUnitHeight / sr.Height);
            go.IsActive = GameRoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            go.AddComponent<Obstacle>(obj.Id);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Obstacle;
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreatePlatformButton(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitHeight - (GameConfigMgr.TileUnitHeight / 2)
            );
            int sequenceId = int.Parse(getPropertyValueByName("sequenceId", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.PlatformButton;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "red_button", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfigMgr.TileUnitWidth / sr.Width, GameConfigMgr.TileUnitHeight / sr.Height);
            go.AddComponent<PlatformButton>(obj.Id, sequenceId);
            go.IsActive = GameRoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            GameConfigMgr.PlatformButtons++;
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlatformButton;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if(GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreateGate(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitHeight - (GameConfigMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "gate", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfigMgr.TileUnitWidth / sr.Width, GameConfigMgr.TileUnitHeight / sr.Height);
            go.AddComponent<Gate>(obj.Id);
            go.IsActive = GameRoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreateDoor(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitHeight - (GameConfigMgr.TileUnitHeight / 2)
            );
            int roomToGo = int.Parse(getPropertyValueByName("roomToGo", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Door;
            go.AddComponent<Door>(obj.Id, roomToGo);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "door", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            sr.Sprite.SetMultiplyTint(0, 0, 0, 0.01f);
            go.transform.Scale = new Vector2((GameConfigMgr.TileUnitWidth / sr.Width), (GameConfigMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Door;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = GameRoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreateWeapon(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) * GameConfigMgr.TileUnitHeight - (GameConfigMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Weapon;
            string weaponType = getPropertyValueByName("weaponType", obj.Properties);
            SpriteRenderer sr = SpriteRenderer.Factory(go, weaponType, Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((GameConfigMgr.TileUnitWidth / sr.Width), (GameConfigMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Weapon;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            BulletType bulletType = (BulletType)int.Parse(getPropertyValueByName("bulletType", obj.Properties));
            float reloadTime = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float offsetShootX = float.Parse(getPropertyValueByName("offsetShootX", obj.Properties));
            float offsetShootY = float.Parse(getPropertyValueByName("offsetShootY", obj.Properties));
            go.AddComponent<Weapon>(bulletType, reloadTime, new Vector2(offsetShootX, offsetShootY));
            go.IsActive = GameRoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + pos.ToString());
        }

        private static void CreateBulletMgr() {
            GameObject bulletMgr = new GameObject("BulletMgr", Vector2.Zero);
            bulletMgr.AddComponent<BulletMgr>(5);
        }

        private static void CreatePlayer(Aiv.Tiled.Object obj) {
            Vector2 cellIndex = new Vector2(
                (float)obj.X / GameConfigMgr.TilePixelWidth,
                ((float)obj.Y / GameConfigMgr.TilePixelWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * GameConfigMgr.TileUnitWidth + (GameConfigMgr.TileUnitWidth / 2),
                cellIndex.Y * GameConfigMgr.TileUnitHeight + (GameConfigMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Player", pos);
            go.Tag = (int)GameObjectTag.Player;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("player"), 6, 4);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "player", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfigMgr.TileUnitWidth / sr.Width, GameConfigMgr.TileUnitHeight / sr.Height);
            go.AddComponent<PlayerController>(GameGridMgr.GetRoomGrid(roomId), 5f);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Player;
            rb.AddCollisionType((uint)RigidbodyType.Door);
            rb.AddCollisionType((uint)RigidbodyType.PlatformButton);
            rb.AddCollisionType((uint)RigidbodyType.Weapon);
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            CreatePlayerAnimations(go, sheet);
            go.AddComponent<ShootModule>().Enabled = GameStats.CanShoot;
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in cella " + cellIndex.ToString());
        }

        private static void CreatePlayerAnimations(GameObject go, Sheet sheet) {
            SheetClip idleDown = new SheetClip(
                sheet, "idleDown", new int[] { 0 }, false, 1
            );
            SheetClip idleRight = new SheetClip(
                sheet, "idleRight", new int[] { 1 }, false, 1
            );
            SheetClip idleLeft = new SheetClip(
                sheet, "idleLeft", new int[] { 2 }, false, 1
            );
            SheetClip idleUp = new SheetClip(
                sheet, "idleUp", new int[] { 3 }, false, 1
            );
            SheetClip death = new SheetClip(
                sheet, "death", new int[] { 4, 5 }, false, 1
            );
            SheetClip walkingDown = new SheetClip(
                sheet, "walkingDown", new int[] { 6, 7, 8, 9 }, true, 10
            );
            SheetClip walkingRight = new SheetClip(
                sheet, "walkingRight", new int[] { 10, 11, 12, 13 }, true, 10
            );
            SheetClip walkingLeft = new SheetClip(
                sheet, "walkingLeft", new int[] { 14, 15, 16, 17 }, true, 10
            );
            SheetClip walkingUp = new SheetClip(
                sheet, "walkingUp", new int[] { 18, 19, 20, 21 }, true, 10
            );
            SheetAnimator animator = go.AddComponent<SheetAnimator>(go.GetComponent<SpriteRenderer>());
            animator.AddClip(idleDown);
            animator.AddClip(idleUp);
            animator.AddClip(idleRight);
            animator.AddClip(idleLeft);
            animator.AddClip(death);
            animator.AddClip(walkingUp);
            animator.AddClip(walkingRight);
            animator.AddClip(walkingLeft);
            animator.AddClip(walkingDown);
        }

        private static string getPropertyValueByName(string name, List<Property> properties) {
            foreach (Property property in properties) {
                if (property.Name != name) continue;
                return property.Value;
            }
            return null;
        }

        public static Map GetMap(int roomId) {
            return maps[roomId];
        }
    }
}
