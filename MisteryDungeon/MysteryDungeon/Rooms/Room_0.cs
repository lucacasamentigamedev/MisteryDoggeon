using Aiv.Fast2D;
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
            GfxMgr.AddTexture("bow", "Assets/bow.png");
            GfxMgr.AddTexture("arrow", "Assets/arrow.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            GameConfigMgr.PlatformButtons = 0;
            GameMapMgr.CreateMap(int.Parse(GetType().Name.Substring(GetType().Name.LastIndexOf('_') + 1)));
            CreatePuzzleMgr();
            /*********TODO: TEST per provare subito l'arma*****************/
            //GameObject.Find("Object_0_22").IsActive = true;
            /*********TODO: TEST pre provare subito l'arma*****************/
        }

        public void CreatePuzzleMgr() {
            GameObject go = new GameObject("PuzzleMgr", Vector2.Zero);
            go.AddComponent<PuzzleMgr>(
                10, //secondi totali del timer del puzzle
                1,  //secondi da aspettare prima che il puzzle sia pronto
                new Vector2[] { new Vector2(0, 22)}, //arma, da attivare
                new Vector2[] { new Vector2(0, 27) } //gate, da disattivare
            );
            if (GameConfigMgr.debugGameObjectCreations) Console.WriteLine("Creato " + go.Name + " in posizione " + Vector2.Zero);
        }
    }
}
