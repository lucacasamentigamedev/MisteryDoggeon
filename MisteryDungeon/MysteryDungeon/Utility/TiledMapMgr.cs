using Aiv.Fast2D.Component;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using MisteryDungeon.MysteryDungeon.Utility.Tiled;
using OpenTK;
using System;
using System.Collections.Generic;
using static MisteryDungeon.AivAlgo.Pathfinding.MovementGrid;

namespace MisteryDungeon.MysteryDungeon {

    static class TiledMapMgr {

        private static Map[] maps;
        private static Dictionary<string, Action<Aiv.Tiled.Object>> tiledObjectsFactories;
        public static int RoomId { get; set; }
        public static float TileUnitWidth { get; set; }
        public static float TileUnitHeight { get; set; }
        public static float TilePixelWidth { get; set; }
        public static float TilePixelHeight { get; set; }
        public static int MapRows { get; set; }
        public static int MapColumns { get; set; }

        static TiledMapMgr() {
            CreateMapArray();
            LoadTiledObjectFactories();
        }

        private static void CreateMapArray() {
            maps = new Map[GameConfig.RoomsNumber];
            for (int i = 0; i < maps.Length; i++) {
                maps[i] = null;
            }
        }

        private static void LoadTiledObjectFactories() {
            tiledObjectsFactories = new Dictionary<string, Action<Aiv.Tiled.Object>>();
            tiledObjectsFactories["Obstacle"] = TiledObjectFactory.CreateObstacle;
            tiledObjectsFactories["Spines"] = TiledObjectFactory.CreateSpines;
            tiledObjectsFactories["PlatformButton"] = TiledObjectFactory.CreatePlatformButton;
            tiledObjectsFactories["Gate"] = TiledObjectFactory.CreateGate;
            tiledObjectsFactories["Door"] = TiledObjectFactory.CreateDoor;
            tiledObjectsFactories["Key"] = TiledObjectFactory.CreateKey;
            tiledObjectsFactories["SpawnPoint"] = TiledObjectFactory.CreateSpawnPoint;
            tiledObjectsFactories["Weapon"] = TiledObjectFactory.CreateWeapon;
            tiledObjectsFactories["BossController"] = TiledObjectFactory.CreateBoss;
            tiledObjectsFactories["PlayerController"] = TiledObjectFactory.CreatePlayer;
        }

        public static void CreateMap(int id) {
            RoomId = id;
            if (maps[RoomId] == null) maps[id] = new Map("Assets/Tiled/Room" + RoomId + ".tmx");
            TileUnitWidth = (float)Game.Win.OrthoWidth / maps[id].Width;
            TileUnitHeight = (float)Game.Win.OrthoHeight / maps[id].Height;
            TilePixelWidth = maps[id].TileWidth;
            TilePixelHeight = maps[id].TileHeight;
            MapRows = maps[id].Width;
            MapColumns = maps[id].Height;
            CreatePathfindingMap(maps[id].Layers);
            CreateBackground(maps[id].Layers);
            CreateObjectTiles(maps[id].ObjectGroups);
            CreateBulletMgr();
            CreateSFXMgr();
            CreateBackgroundMusic();
            GameStats.PreviousRoom = GameStats.ActualRoom;
            GameStats.ActualRoom = RoomId;
        }

        private static void CreatePathfindingMap(List<Layer> layers) {
            foreach (Layer layer in layers) {
                if (layer.Name != "Collisions") continue;
                if (MovementGridMgr.GetRoomGrid(RoomId) != null) {
                    EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa stanza " + RoomId + " esistente, uso quella"));
                    return;
                }
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa stanza " + RoomId + " non presente, la creo ex novo"));
                MovementGridMgr.SetRoomGrid(RoomId, new MovementGrid(MapRows, MapColumns, layer));
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(MovementGridMgr.PrintMovementGrid(RoomId)));
            }
        }

        private static void CreateBackground(List<Layer> layers) {
            foreach (Layer layer in layers) {
                if (layer.Name != "Background") continue;
                for (int i = 0; i < layer.Tiles.GetLength(0); i++) {
                    for (int j = 0; j < layer.Tiles.GetLength(1); j++) {
                        CreateBackgroundTile(new TileSprite(maps[RoomId].Tilesets, layer.Tiles[i, j]), i, j, layer.Tiles[i, j].HorizontalFlip, layer.Tiles[i, j].VerticalFlip);
                    }
                }
            }
        }

        private static void CreateBackgroundTile(TileSprite tileSprite, int xIndex, int yIndex, bool horizontalFlip, bool verticalFlip) {
            Vector2 pos = new Vector2(
                TileUnitWidth * xIndex + (TileUnitWidth / 2),
                TileUnitHeight * yIndex + (TileUnitHeight / 2)
            );
            GameObject go = new GameObject("Background_Tile_" + xIndex + "_" + yIndex, pos);
            Vector2 cell = new Vector2(
                (int)Math.Ceiling(pos.X / TileUnitWidth) - 1,
                (int)Math.Ceiling(pos.Y / TileUnitHeight) - 1
            );
            go.AddComponent(SpriteRenderer.Factory(go, GfxMgr.GetTexture("MapTileset.png"), Vector2.One * 0.5f, DrawLayer.Background, TilePixelWidth, TilePixelWidth));
            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            if (horizontalFlip) sr.Sprite.FlipX = true;
            if (verticalFlip) sr.Sprite.FlipY = true;
            sr.TextureOffset = new Vector2(tileSprite.OffsetX, tileSprite.OffsetY);
            go.transform.Scale = new Vector2(TileUnitWidth / sr.Width, TileUnitHeight / sr.Height);
            if ( (cell.X == 0 || cell.X == MapRows - 1 ||
                //se è la cella muro perimetrale ci metto il rigidbody
                cell.Y == 0 || cell.Y == MapColumns - 1)
                && MovementGridMgr.GetGridTile(RoomId, cell) == EGridTile.Wall) {
                go.Tag = (int)GameObjectTag.Wall;
                Rigidbody rb = go.AddComponent<Rigidbody>();
                rb.Type = RigidbodyType.Wall;
                go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            }
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + pos.ToString()));
        }

        private static void CreateObjectTiles(List<ObjectGroup> groups) {
            foreach (ObjectGroup objectGroup in groups) {
                for (int i = 0; i < objectGroup.Objects.Count; i++) {
                    tiledObjectsFactories[objectGroup.Objects[i].Class].Invoke(objectGroup.Objects[i]);
                }
            }
        }

        private static void CreateBulletMgr() {
            GameObject bulletMgr = new GameObject("BulletMgr", Vector2.Zero);
            bulletMgr.AddComponent<BulletMgr>(5);
        }

        private static void CreateSFXMgr() {
            GameObject gameObject = new GameObject("SFXManager", Vector2.Zero);
            gameObject.AddComponent<SFXMgr>();
            gameObject.AddComponent<AudioSourceComponent>().MyType = (int)AudioLayer.sfx;
        }

        private static void CreateBackgroundMusic() {
            GameObject gameLogic = new GameObject("BackgroundMusic", Vector2.Zero);
            AudioSourceComponent audioSource = gameLogic.AddComponent<AudioSourceComponent>();
            audioSource.SetClip(AudioMgr.GetClip("background"));
            audioSource.Loop = true;
            audioSource.Play();
        }

        public static Map GetMap(int RoomId) {
            return maps[RoomId];
        }
    }
}
