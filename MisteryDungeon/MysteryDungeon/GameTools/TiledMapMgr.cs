using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using MisteryDungeon.MysteryDungeon.Rooms;
using OpenTK;
using System;
using System.Collections.Generic;
using static MisteryDungeon.AivAlgo.Pathfinding.MovementGrid;

namespace MisteryDungeon.MysteryDungeon {

    static class TiledMapMgr {

        private static Map[] maps;
        private static int roomId;

        static TiledMapMgr() {
            maps = new Map[GameConfig.RoomsNumber];
            for(int i = 0; i < maps.Length; i++) {
                maps[i] = null;
            }
        }

        public static void CreateMap(int id) {
            roomId = id;
            if (maps[roomId] == null) maps[id] = new Map("Assets/Tiled/Room" + roomId + ".tmx");
            GameConfig.TileUnitWidth = (float)Game.Win.OrthoWidth / maps[id].Width;
            GameConfig.TileUnitHeight = (float)Game.Win.OrthoHeight / maps[id].Height;
            GameConfig.TilePixelWidth = maps[id].TileWidth;
            GameConfig.TilePixelHeight = maps[id].TileHeight;
            GameConfig.MapRows = maps[id].Width;
            GameConfig.MapColumns = maps[id].Height;
            //pathfinding layer
            foreach (Layer layer in maps[id].Layers) {
                if (layer.Name != "Collisions") continue;
                CreatePathfindingMap(layer);
            }
            //Background layer
            foreach (Layer layer in maps[id].Layers) {
                if (layer.Name != "Background") continue;
                CreateBackground(layer);
            }
            //Objects layers
            foreach (ObjectGroup objectGroup in maps[id].ObjectGroups) {
                for (int i = 0; i < objectGroup.Objects.Count; i++) {
                    CreateObjectTile(objectGroup.Objects[i]);
                }
            }
            //other
            CreateBulletMgr();
            GameStats.PreviousRoom = GameStats.ActualRoom;
            GameStats.ActualRoom = roomId;
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
                GameConfig.TileUnitWidth * xIndex + (GameConfig.TileUnitWidth / 2),
                GameConfig.TileUnitHeight * yIndex + (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Background_Tile_" + xIndex + "_" + yIndex, pos);
            Vector2 cell = new Vector2(
                (int)Math.Ceiling(pos.X / GameConfig.TileUnitWidth) - 1,
                (int)Math.Ceiling(pos.Y / GameConfig.TileUnitHeight) - 1
            );
            go.AddComponent(SpriteRenderer.Factory(go, tileSprite.Texture, Vector2.One * 0.5f, DrawLayer.Background, GameConfig.TilePixelWidth, GameConfig.TilePixelWidth));
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.TextureOffset = new Vector2(tileSprite.OffsetX, tileSprite.OffsetY);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            if ( (cell.X == 0 || cell.X == GameConfig.MapRows - 1 ||
                //se è la cella muro perimetrale ci metto il rigidbody
                cell.Y == 0 || cell.Y == GameConfig.MapColumns - 1)
                && MovementGridMgr.GetGridTile(roomId, cell) == EGridTile.Wall) {
                go.Tag = (int)GameObjectTag.Wall;
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.Type = RigidbodyType.Wall;
                go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            }
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreatePathfindingMap(Layer layer) {
            if (MovementGridMgr.GetRoomGrid(roomId) == null) {
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa stanza " + roomId + " non presente, la creo ex novo"));
                MovementGridMgr.SetRoomGrid(roomId, new MovementGrid(GameConfig.MapRows, GameConfig.MapColumns, layer));
            } else {
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa stanza " + roomId + " esistente, uso quella"));
            };
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(MovementGridMgr.PrintMovementGrid(roomId)));
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
                case "key":
                    CreateKey(obj);
                    break;
                case "spawnPoint":
                    CreateSpawnPoint(obj);
                    break;
                case "spines":
                    CreateSpines(obj);
                    break;
                case "player":
                    int fromRoom = int.Parse(getPropertyValueByName("fromRoom", obj.Properties));
                    if (GameStats.ActualRoom == fromRoom) CreatePlayer(obj);
                    break;
                case "boss":
                    CreateBoss(obj);
                    break;
            }
        }

        private static void CreateObstacle(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Obstacle;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "crate", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            go.AddComponent<Obstacle>(obj.Id, roomId);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Obstacle;
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }
        
        private static void CreateSpines(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "spines", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            go.AddComponent<Spines>(obj.Id);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreatePlatformButton(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            int sequenceId = int.Parse(getPropertyValueByName("sequenceId", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            GameObject.Find("PuzzleMgr").GetComponent<PuzzleMgr>().TotalButtons++;
            go.Tag = (int)GameObjectTag.PlatformButton;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "red_button", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            go.AddComponent<PlatformButton>(obj.Id, sequenceId);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlatformButton;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if(GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateGate(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "gate", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            go.AddComponent<Gate>(obj.Id);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateDoor(Aiv.Tiled.Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            int roomToGo = int.Parse(getPropertyValueByName("roomToGo", obj.Properties));
            int lockedBy = int.Parse(getPropertyValueByName("lockedBy", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Door;
            go.AddComponent<Door>(obj.Id, roomToGo, lockedBy);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "door", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            sr.Sprite.SetMultiplyTint(0, 0, 0, 0.01f);
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width), (GameConfig.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Door;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateKey(Aiv.Tiled.Object obj) {
            if(GameStats.collectedKeys.Contains(obj.Id)) return;
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            int gateId = int.Parse(getPropertyValueByName("gateId", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Key;
            go.AddComponent<Key>(obj.Id, gateId);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "key", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width), (GameConfig.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Key;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }
        
        private static void CreateSpawnPoint(Aiv.Tiled.Object obj) {
            if(GameStats.HordeDefeated) return;
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            EnemyType enemyType = (EnemyType)int.Parse(getPropertyValueByName("enemyType", obj.Properties));
            float spawnTimer = float.Parse(getPropertyValueByName("spawnTimer", obj.Properties));
            float readyTimer = float.Parse(getPropertyValueByName("readyTimer", obj.Properties));
            int enemiesNumber = int.Parse(getPropertyValueByName("enemiesNumber", obj.Properties));
            float spawnPointHealth = float.Parse(getPropertyValueByName("spawnPointHealth", obj.Properties));
            float enemyHealth = float.Parse(getPropertyValueByName("enemyHealth", obj.Properties));
            float enemySpeed = float.Parse(getPropertyValueByName("enemySpeed", obj.Properties));
            float enemyDamage = float.Parse(getPropertyValueByName("enemyDamage", obj.Properties));
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.SpawnPoint;
            SpawnPoint sp = go.AddComponent<SpawnPoint>(enemiesNumber, enemyType,
                spawnTimer,readyTimer, enemyHealth, enemySpeed, enemyDamage);
            GameObject.Find("HordeMgr").GetComponent<HordeMgr>().AddSpawnPoint(sp);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "spawnPoint", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width), (GameConfig.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.SpawnPoint;
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            go.AddComponent<HealthModule>(spawnPointHealth, new Vector2(-0.5f, -0.5f));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateWeapon(Aiv.Tiled.Object obj) {
            string weaponType = getPropertyValueByName("weaponType", obj.Properties);
            if (weaponType == "bow" && GameStats.BowPicked) return;
            Vector2 pos = new Vector2(
                ((float)obj.X / GameConfig.TilePixelWidth) * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                ((float)obj.Y / GameConfig.TilePixelWidth) * GameConfig.TileUnitHeight - (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + roomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Weapon;
            SpriteRenderer sr = SpriteRenderer.Factory(go, weaponType, Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width), (GameConfig.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Weapon;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            BulletType bulletType = (BulletType)int.Parse(getPropertyValueByName("bulletType", obj.Properties));
            float reloadTime = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float offsetShootX = float.Parse(getPropertyValueByName("offsetShootX", obj.Properties));
            float offsetShootY = float.Parse(getPropertyValueByName("offsetShootY", obj.Properties));
            go.AddComponent<Weapon>(bulletType, reloadTime, new Vector2(offsetShootX, offsetShootY));
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(roomId, obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateBulletMgr() {
            GameObject bulletMgr = new GameObject("BulletMgr", Vector2.Zero);
            bulletMgr.AddComponent<BulletMgr>(5);
        }

        private static void CreatePlayer(Aiv.Tiled.Object obj) {
            Vector2 cellIndex = new Vector2(
                (float)obj.X / GameConfig.TilePixelWidth,
                ((float)obj.Y / GameConfig.TilePixelWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                cellIndex.Y * GameConfig.TileUnitHeight + (GameConfig.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Player", pos);
            go.Tag = (int)GameObjectTag.Player;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("player"), 6, 4);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "player", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(GameConfig.TileUnitWidth / sr.Width, GameConfig.TileUnitHeight / sr.Height);
            go.AddComponent<PlayerController>(MovementGridMgr.GetRoomGrid(roomId), 5f);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Player;
            rb.AddCollisionType((uint)RigidbodyType.Door);
            rb.AddCollisionType((uint)RigidbodyType.PlatformButton);
            rb.AddCollisionType((uint)RigidbodyType.Weapon);
            rb.AddCollisionType((uint)RigidbodyType.Key);
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            rb.AddCollisionType((uint)RigidbodyType.EnemyBullet);
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            CreatePlayerAnimations(go, sheet);
            ShootModule sm = go.AddComponent<ShootModule>("Shoot", false);
            if(GameStats.PlayerCanShoot) {
                sm.Enabled = GameStats.PlayerCanShoot;
                Weapon weapon = GameStats.ActiveWeapon;
                sm.SetWeapon(weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot);
            }
            go.AddComponent<HealthModule>(15, new Vector2(-0.45f, -0.5f));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in cella " + cellIndex.ToString()));
        }
        
        private static void CreateBoss(Aiv.Tiled.Object obj) {
            if (GameStats.BossDefeated) return;
            Vector2 cellIndex = new Vector2(
                (float)obj.X / GameConfig.TilePixelWidth,
                ((float)obj.Y / GameConfig.TilePixelWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * GameConfig.TileUnitWidth + (GameConfig.TileUnitWidth / 2),
                cellIndex.Y * GameConfig.TileUnitHeight + (GameConfig.TileUnitHeight / 2)
            );
            int bulletType = int.Parse(getPropertyValueByName("bulletType", obj.Properties));
            float health = float.Parse(getPropertyValueByName("health", obj.Properties));
            float readyTimer = float.Parse(getPropertyValueByName("readyTimer", obj.Properties));
            float reloadTime = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float speed = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float offsetShootX = float.Parse(getPropertyValueByName("offsetShootX", obj.Properties));
            float offsetShootY = float.Parse(getPropertyValueByName("offsetShootY", obj.Properties));
            GameObject go = new GameObject("Boss", pos);
            go.Tag = (int)GameObjectTag.Boss;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("boss"), 4, 2);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "boss", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width) * 2, (GameConfig.TileUnitHeight / sr.Height) * 2);
            go.AddComponent<BossController>(readyTimer, speed);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Boss;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            CreateBossAnimations(go, sheet);
            ShootModule sm = go.AddComponent<ShootModule>("", true);
            Weapon weapon = new Weapon(go, (BulletType)bulletType, reloadTime, new Vector2(offsetShootX, offsetShootY));
            sm.SetWeapon(weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot);
            go.AddComponent<HealthModule>(health, new Vector2(-0.45f, -0.5f));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in cella " + cellIndex.ToString()));
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

        private static void CreateBossAnimations(GameObject go, Sheet sheet) {
            SheetClip idle = new SheetClip(
                sheet, "idle", new int[] { 2 }, true, 10
            );
            SheetClip walking = new SheetClip(
                sheet, "walking", new int[] { 0, 1, 2, 3, 4 }, true, 7
            );
            SheetClip death = new SheetClip(
                sheet, "death", new int[] { 5, 6, 7 }, false, 7
            );
            SheetAnimator animator = go.AddComponent<SheetAnimator>(go.GetComponent<SpriteRenderer>());
            animator.AddClip(idle);
            animator.AddClip(walking);
            animator.AddClip(death);
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
