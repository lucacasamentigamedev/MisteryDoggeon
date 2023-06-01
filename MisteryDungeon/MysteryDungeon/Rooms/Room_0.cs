using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
using OpenTK;
using System;

namespace MisteryDungeon {
    internal class Room_0 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("red_button", "Assets/red_button.png");
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("loading", "Assets/loading.png");
            GfxMgr.AddTexture("gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            GameConfigMgr.PlatformButtons = 0;
            GameMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
            CreatePuzzleMgr();
        }

        public void CreatePuzzleMgr() {
            GameObject go = new GameObject("PuzzleMgr", Vector2.Zero);
            go.AddComponent<PuzzleMgr>(
                10, //secondi totali del timer del puzzle
                2,  //secondi da aspettare prima che il puzzle sia pronto
                0,  //room che contiene il gate da sbloccare col puzzle
                27  //ID del gate da sbloccare nella stanza del parametro sopra
            );
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + Vector2.Zero);
        }
    }
}
