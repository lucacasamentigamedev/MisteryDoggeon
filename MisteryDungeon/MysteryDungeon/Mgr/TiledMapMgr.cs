using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using Aiv.Tiled;
using MisteryDungeon.AivAlgo.Pathfinding;
using MisteryDungeon.MysteryDungeon.Mgr;
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
        public static int BackgroundClipId { get; set; }

        static TiledMapMgr() {
            LoadTiledObjectFactories();
            ResetMaps();
        }

        private static void LoadTiledObjectFactories() {
            tiledObjectsFactories = new Dictionary<string, Action<Aiv.Tiled.Object>>();
            tiledObjectsFactories["Obstacle"] = TiledObjectFactoryMgr.CreateObstacle;
            tiledObjectsFactories["Spines"] = TiledObjectFactoryMgr.CreateSpines;
            tiledObjectsFactories["PlatformButton"] = TiledObjectFactoryMgr.CreatePlatformButton;
            tiledObjectsFactories["Gate"] = TiledObjectFactoryMgr.CreateGate;
            tiledObjectsFactories["Door"] = TiledObjectFactoryMgr.CreateDoor;
            tiledObjectsFactories["Key"] = TiledObjectFactoryMgr.CreateKey;
            tiledObjectsFactories["SpawnPoint"] = TiledObjectFactoryMgr.CreateSpawnPoint;
            tiledObjectsFactories["Weapon"] = TiledObjectFactoryMgr.CreateWeapon;
            tiledObjectsFactories["BossController"] = TiledObjectFactoryMgr.CreateBoss;
            tiledObjectsFactories["PlayerController"] = TiledObjectFactoryMgr.CreatePlayer;
            tiledObjectsFactories["MemoryCard"] = TiledObjectFactoryMgr.CreateMemoryCard;
        }

        public static void CreateMap(int id) {
            RoomId = id;
            LoadTiledMap();
            CreatePathfindingMap(maps[id].Layers, "Collisions");
            CreateBackground(maps[id].Layers);
            CreateObjectTiles(maps[id].ObjectGroups);
            CreateBulletMgr();
            CreateSFXMgr();
            CreateBackgroundMusic();
            CreatePauseUI();
            CreateLoadingUI();
        }

        private static void LoadTiledMap() {
            if (maps[RoomId] == null) {
                maps[RoomId] = new Map("Assets/Tiled/Room" + RoomId + ".tmx");
            }
            TileUnitWidth = (float)Game.Win.OrthoWidth / maps[RoomId].Width;
            TileUnitHeight = (float)Game.Win.OrthoHeight / maps[RoomId].Height;
            TilePixelWidth = maps[RoomId].TileWidth;
            TilePixelHeight = maps[RoomId].TileHeight;
            MapRows = maps[RoomId].Width;
            MapColumns = maps[RoomId].Height;
        }

        private static void CreatePathfindingMap(List<Layer> layers, string collisionLayerName) {
            if (MovementGridMgr.GetRoomGrid(RoomId) != null) {
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa pathfinding stanza " + RoomId + " esistente, uso quella"));
            } else {
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Mappa stanza " + RoomId + " non presente, la creo ex novo"));
                foreach (Layer layer in layers) {
                    if (layer.Name != collisionLayerName) continue;
                    MovementGridMgr.SetRoomGrid(RoomId, new MovementGrid(MapRows, MapColumns, layer));
                    EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(MovementGridMgr.PrintMovementGrid(RoomId)));
                }
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
            bulletMgr.AddComponent<BulletMgr>(
                10, // bullet pool size
                5,  // arrow damage
                5,  // arrow speed
                5,  // globe damage
                5   // globe speed
            );
        }

        private static void CreateSFXMgr() {
            GameObject gameObject = new GameObject("SFXManager", Vector2.Zero);
            gameObject.AddComponent<SFXMgr>();
            AudioSourceComponent asc = gameObject.AddComponent<AudioSourceComponent>();
            asc.MyType = (int)AudioLayer.sfx;
        }

        private static void CreateBackgroundMusic() {
            GameObject gameLogic = new GameObject("BackgroundMusic", Vector2.Zero);
            AudioSourceComponent audioSource = gameLogic.AddComponent<AudioSourceComponent>();
            gameLogic.AddComponent<BackgroundMusicLogic>();
            int backgroundClipId = BackgroundClipId;
            while(backgroundClipId == BackgroundClipId) {
                backgroundClipId = RandomGenerator.GetRandomInt(1, GameConfigMgr.BackgroundMusicNumber + 1);
            }
            BackgroundClipId = backgroundClipId;
            audioSource.SetClip(AudioMgr.GetClip("background"+ backgroundClipId));
            audioSource.Loop = true;
            audioSource.Play();
        }

        private static void CreatePauseUI() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject pauseBackground = new GameObject("PauseBackground", Vector2.Zero);
            pauseBackground.transform.Scale = new Vector2(3, 3);
            pauseBackground.AddComponent(SpriteRenderer.Factory(pauseBackground, "blackScreen", Vector2.Zero, DrawLayer.GUI));
            SpriteRenderer sr = pauseBackground.GetComponent<SpriteRenderer>();
            sr.Sprite.SetMultiplyTint(0, 0, 0, 0.7f);
            sr.Camera = CameraMgr.GetCamera("GUI");
            GameObject pauseWrite = new GameObject("PauseWrite", new Vector2(Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit(stdFont.CharacterWidth) * 2.5f * 2, Game.Win.OrthoHeight * 0.5f));
            TextBox tb = pauseWrite.AddComponent<TextBox>(stdFont, 5, Vector2.One * 2);
            tb.SetText("Pause");
            tb.Camera = CameraMgr.GetCamera("GUI");
            GameObject pauseLogic = new GameObject("PauseLogic", Vector2.Zero);
            pauseLogic.AddComponent<PauseLogic>(new string[] { "PauseBackground", "PauseWrite" }, "Pause");
        }

        public static void CreateLoadingUI() {
            GameObject load = new GameObject("Loading", new Vector2(Game.Win.OrthoWidth / 2, Game.Win.OrthoHeight / 2));
            SpriteRenderer sr = SpriteRenderer.Factory(load, "loading", Vector2.One * 0.5f, DrawLayer.GUI);
            sr.Enabled = false;
            load.transform.Scale = new Vector2(Game.Win.OrthoWidth / sr.Width / 1.5f, Game.Win.OrthoHeight / sr.Width / 1.5f);
            load.AddComponent(sr);
            load.AddComponent<LoadingLogic>();
        }

        public static void SetMap(Map map, int index) {
            maps[index] = map;
        }

        public static Map GetMap(int RoomId) {
            return maps[RoomId];
        }

        public static void ResetMaps() {
            maps = new Map[GameConfigMgr.RoomsNumber];
            for (int i = 0; i < maps.Length; i++) {
                maps[i] = null;
            }
        }
    }
}
