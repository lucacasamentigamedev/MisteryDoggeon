using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.MysteryDungeon.RoomObjects;
using MisteryDungeon.MysteryDungeon.Rooms;
using OpenTK;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon.Utility.Tiled {
    static class TiledObjectFactoryMgr {
        public static void CreateObstacle(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Obstacle;
            string spriteName = "";
            int r = RandomGenerator.GetRandomInt(0, 4);
            if (r == 0) spriteName = "skull";
            else if (r == 1) spriteName = "pot";
            else if (r == 2) spriteName = "shell";
            else if (r == 3) spriteName = "bones";
            if (r != 1) go.transform.Rotation = RandomGenerator.GetRandomInt(0, 361);
            SpriteRenderer sr = SpriteRenderer.Factory(go, spriteName, Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width, TiledMapMgr.TileUnitHeight / sr.Height);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            go.AddComponent<Obstacle>(obj.Id, TiledMapMgr.RoomId);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Obstacle;
            Collider c = ColliderFactory.CreateHalfUnscaledBoxFor(go);
            go.AddComponent(c);
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateSpines(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "spines", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width, TiledMapMgr.TileUnitHeight / sr.Height);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            go.AddComponent<Spines>(obj.Id);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreatePlatformButton(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            int sequenceId = int.Parse(getPropertyValueByName("sequenceId", obj.Properties));
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            GameObject.Find("PuzzleMgr").GetComponent<PuzzleMgr>().TotalButtons++;
            go.Tag = (int)GameObjectTag.PlatformButton;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "platformButton", Vector2.One * 0.5f,DrawLayer.Middleground,TiledMapMgr.TilePixelWidth, TiledMapMgr.TilePixelHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width, TiledMapMgr.TileUnitHeight / sr.Height);
            GameObject.Find("PuzzleMgr").GetComponent<PuzzleMgr>().AddPlatformButton(go.AddComponent<PlatformButton>(obj.Id, sequenceId));
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlatformButton;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateGate(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "gate", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width, TiledMapMgr.TileUnitHeight / sr.Height);
            go.AddComponent<Gate>(obj.Id);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateDoor(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            int roomToGo = int.Parse(getPropertyValueByName("roomToGo", obj.Properties));
            int lockedBy = int.Parse(getPropertyValueByName("lockedBy", obj.Properties));
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Door;
            go.AddComponent<Door>(obj.Id, roomToGo, lockedBy);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "door", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            sr.Sprite.SetMultiplyTint(0, 0, 0, 0.01f);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Door;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            RoomObjectsMgr.SetRoomObjectActiveness(TiledMapMgr.RoomId, obj.Id, true, true, AivAlgo.Pathfinding.MovementGrid.EGridTile.Floor);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateKey(Object obj) {
            if (GameStatsMgr.CollectedKeys.Contains(obj.Id)) return;
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Key;
            go.AddComponent<Key>(obj.Id);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "key", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Key;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateSpawnPoint(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            EnemyType enemyType = (EnemyType)int.Parse(getPropertyValueByName("enemyType", obj.Properties));
            float spawnTimer = float.Parse(getPropertyValueByName("spawnTimer", obj.Properties));
            float readyTimer = float.Parse(getPropertyValueByName("readyTimer", obj.Properties));
            int enemiesNumber = int.Parse(getPropertyValueByName("enemiesNumber", obj.Properties));
            float spawnPointHealth = float.Parse(getPropertyValueByName("spawnPointHealth", obj.Properties));
            float enemyHealth = float.Parse(getPropertyValueByName("enemyHealth", obj.Properties));
            float enemySpeed = float.Parse(getPropertyValueByName("enemySpeed", obj.Properties));
            float enemyDamage = float.Parse(getPropertyValueByName("enemyDamage", obj.Properties));
            float deathTimer = float.Parse(getPropertyValueByName("deathTimer", obj.Properties));
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.SpawnPoint;
            SpawnPoint sp = go.AddComponent<SpawnPoint>(enemiesNumber, enemyType,
                spawnTimer, readyTimer, enemyHealth, enemySpeed, enemyDamage, deathTimer);
            GameObject.Find("HordeMgr").GetComponent<HordeMgr>().AddSpawnPoint(sp);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "spawnPoint", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.SpawnPoint;
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = false;
            go.AddComponent<HealthModule>(spawnPointHealth, spawnPointHealth, new Vector2(-0.5f, -0.5f));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateWeapon(Object obj) {
            if (GameStatsMgr.collectedWeapons.Contains(obj.Id)) return;
            int weaponType = int.Parse(getPropertyValueByName("weaponType", obj.Properties));
            string weaponImage = getPropertyValueByName("weaponImage", obj.Properties);
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Weapon;
            SpriteRenderer sr = SpriteRenderer.Factory(go, weaponImage, Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Weapon;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            BulletType bulletType = (BulletType)int.Parse(getPropertyValueByName("bulletType", obj.Properties));
            float reloadTime = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float offsetShootX = float.Parse(getPropertyValueByName("offsetShootX", obj.Properties));
            float offsetShootY = float.Parse(getPropertyValueByName("offsetShootY", obj.Properties));
            go.AddComponent<Weapon>((WeaponType)weaponType, bulletType, reloadTime, new Vector2(offsetShootX, offsetShootY), TiledMapMgr.RoomId, obj.Id);
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreatePlayer(Object obj) {
            int fromRoom = int.Parse(getPropertyValueByName("fromRoom", obj.Properties));
            
            if (GameStatsMgr.PreviousRoom != fromRoom) return;

            Vector2 cellIndex = new Vector2(
                (float)obj.X / TiledMapMgr.TilePixelWidth,
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                cellIndex.Y * TiledMapMgr.TileUnitHeight + (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Player", pos);
            go.Tag = (int)GameObjectTag.Player;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("player"), 6, 4);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "player", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width, TiledMapMgr.TileUnitHeight / sr.Height);
            go.AddComponent<PlayerController>(MovementGridMgr.GetRoomGrid(TiledMapMgr.RoomId), 5f, "Move", 3f);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Player;
            rb.AddCollisionType((uint)RigidbodyType.Door);
            rb.AddCollisionType((uint)RigidbodyType.PlatformButton);
            rb.AddCollisionType((uint)RigidbodyType.Weapon);
            rb.AddCollisionType((uint)RigidbodyType.Key);
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            rb.AddCollisionType((uint)RigidbodyType.EnemyBullet);
            rb.AddCollisionType((uint)RigidbodyType.MemoryCard);
            rb.AddCollisionType((uint)RigidbodyType.Hearth);
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            CreatePlayerAnimations(go, sheet);
            ShootModule sm = go.AddComponent<ShootModule>("Shoot", false);
            if (GameStatsMgr.PlayerCanShoot) {
                sm.Enabled = GameStatsMgr.PlayerCanShoot;
                Weapon weapon = GameStatsMgr.ActiveWeapon;
                sm.SetWeapon(weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot);
            }
            go.AddComponent<HealthModule>(GameStatsMgr.PlayerHealth, GameStatsMgr.maxPlayerHealth, new Vector2(-0.5f, -0.5f));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in cella " + cellIndex.ToString()));
        }

        public static void CreateBoss(Object obj) {
            if (GameStatsMgr.BossDefeated) return;
            Vector2 cellIndex = new Vector2(
                (float)obj.X / TiledMapMgr.TilePixelWidth,
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) - 1
            );
            Vector2 pos = new Vector2(
                cellIndex.X * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                cellIndex.Y * TiledMapMgr.TileUnitHeight + (TiledMapMgr.TileUnitHeight / 2)
            );
            int bulletType = int.Parse(getPropertyValueByName("bulletType", obj.Properties));
            float health = float.Parse(getPropertyValueByName("health", obj.Properties));
            float readyTimer = float.Parse(getPropertyValueByName("readyTimer", obj.Properties));
            float reloadTime = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float speed = float.Parse(getPropertyValueByName("reloadTime", obj.Properties));
            float offsetShootX = float.Parse(getPropertyValueByName("offsetShootX", obj.Properties));
            float offsetShootY = float.Parse(getPropertyValueByName("offsetShootY", obj.Properties));
            float deathTimer = float.Parse(getPropertyValueByName("deathTimer", obj.Properties));
            GameObject go = new GameObject("Boss", pos);
            go.Tag = (int)GameObjectTag.Boss;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("redBlob"), 4, 2);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "redBlob", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width) * 2, (TiledMapMgr.TileUnitHeight / sr.Height) * 2);
            go.AddComponent<BossController>(
                readyTimer, speed, deathTimer,
                new Vector2[] { new Vector2(3, 39) },
                new Vector2[] { new Vector2(3, 86), new Vector2(3, 87), new Vector2(3, 84), new Vector2(3, 88) }
            );
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Boss;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            CreateBossAnimations(go, sheet);
            ShootModule sm = go.AddComponent<ShootModule>("", true);
            Weapon weapon = new Weapon(go, WeaponType.Blaster, (BulletType)bulletType, reloadTime, new Vector2(offsetShootX, offsetShootY), TiledMapMgr.RoomId, obj.Id);
            sm.SetWeapon(weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot);
            go.AddComponent<HealthModule>(health, health, new Vector2(-0.5f, -0.75f));
            go.AddComponent<WobbleEffect>(7).Enabled = false;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in cella " + cellIndex.ToString()));
        }

        public static void CreatePlayerAnimations(GameObject go, Sheet sheet) {
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

        public static void CreateBossAnimations(GameObject go, Sheet sheet) {
            SheetClip walking = new SheetClip(
                sheet, "walking", new int[] { 0, 1, 2, 3, 4 }, true, 7
            );
            SheetClip death = new SheetClip(
                sheet, "death", new int[] { 5, 6, 7 }, false, 7
            );
            SheetAnimator animator = go.AddComponent<SheetAnimator>(go.GetComponent<SpriteRenderer>());
            animator.AddClip(walking);
            animator.AddClip(death);
        }

        public static void CreateMemoryCard(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            float respawnTimer = float.Parse(getPropertyValueByName("respawnTimer", obj.Properties));
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.MemoryCard;
            go.AddComponent<MemoryCard>(obj.Id, TiledMapMgr.RoomId, respawnTimer);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "memoryCard", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.MemoryCard;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static void CreateHearth(Object obj) {
            Vector2 pos = new Vector2(
                ((float)obj.X / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitWidth + (TiledMapMgr.TileUnitWidth / 2),
                ((float)obj.Y / TiledMapMgr.TilePixelWidth) * TiledMapMgr.TileUnitHeight - (TiledMapMgr.TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Object_" + TiledMapMgr.RoomId + "_" + obj.Id, pos);
            go.Tag = (int)GameObjectTag.Hearth;
            go.AddComponent<Hearth>(obj.Id, TiledMapMgr.RoomId);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "hearth", Vector2.One * 0.5f, DrawLayer.Middleground);
            go.AddComponent(sr);
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Hearth;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.IsActive = RoomObjectsMgr.AddRoomObjectActiveness(obj.Id, obj.Visible);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        public static string getPropertyValueByName(string name, List<Property> properties) {
            foreach (Property property in properties) {
                if (property.Name != name) continue;
                return property.Value;
            }
            return null;
        }
    }
}
