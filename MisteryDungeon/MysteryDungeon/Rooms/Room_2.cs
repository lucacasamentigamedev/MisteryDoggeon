using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace MisteryDungeon {
    public class Room_2 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("skull", "Assets/skull.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
            GfxMgr.AddTexture("redGlobe", "Assets/red_globe.png");
            GfxMgr.AddTexture("gate", "Assets/mushroom.png");
            GfxMgr.AddTexture("greenBlob", "Assets/Spritesheets/green_blob.png");
            GfxMgr.AddTexture("spines", "Assets/spines.png");
            GfxMgr.AddTexture("spawnPoint", "Assets/spawn_point.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/healthbar_foreground.png");
            GfxMgr.AddTexture("MapTileset.png", "Assets/Tiled/MapTileset.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateHordeMgr();
            CreateMap();

            /******************FIXME: cheat da togliere**************/
            //RoomObjectsMgr.SetRoomObjectActiveness(2, 39, false, true);
            //GameObject g = GameObject.Find("Object_2_39");
            //if (g != null) g.IsActive = false;*/
            /********************************/
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                true    //print enemy horde logs
            );
        }
        public void CreateHordeMgr() {
            GameObject go = new GameObject("HordeMgr", Vector2.Zero);
            go.AddComponent<HordeMgr>();
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }
    }
}
