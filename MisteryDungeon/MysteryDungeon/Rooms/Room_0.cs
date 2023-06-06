using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;

namespace MisteryDungeon {
    public class Room_0 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("red_button", "Assets/red_button.png");
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
            GfxMgr.AddTexture("bow", "Assets/bow.png");
            GfxMgr.AddTexture("redGlobe", "Assets/red_globe.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
            GfxMgr.AddTexture("healthBarBackground", "Assets/healthbar_background.png");
            GfxMgr.AddTexture("healthBarForeground", "Assets/healthbar_foreground.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreatePuzzleMgr();
            CreateMap();

            /******************FIXME: cheat da togliere**************/
            RoomObjectsMgr.SetRoomObjectActiveness(0, 27, false);
            GameObject g = GameObject.Find("Object_0_27");
            if (g != null) g.IsActive = false;
            g = GameObject.Find("Object_0_22");
            if (g != null) g.IsActive = true;
            GameStats.HordeDefeated = true;
            GameStats.collectedKeys.Add(25);
            /*****************************************************/
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,   //print puzzle logs
                false,  //print object creations logs
                false   //print enemy horde logs
            );
        }

        public void CreateMap() {
            TiledMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
        }

        public void CreatePuzzleMgr() {
            GameObject go = new GameObject("PuzzleMgr", Vector2.Zero);
            go.AddComponent<PuzzleMgr>(
                10, //secondi totali del timer del puzzle
                1,  //secondi da aspettare prima che il puzzle sia pronto
                new Vector2[] { new Vector2(0, 22)}, //arma, da attivare
                new Vector2[] { new Vector2(0, 27) } //gate, da disattivare
            );
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
        }
    }
}
