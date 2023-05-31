using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using MisteryDungeon.MysteryDungeon;
namespace MisteryDungeon {
    internal class Room_0 : Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("red_button", "Assets/red_button.png");
            GfxMgr.AddTexture("crate", "Assets/crate.png");
            GfxMgr.AddTexture("door", "Assets/crate.png");
            GfxMgr.AddTexture("lamp_gate", "Assets/lamp_gate.png");
            GfxMgr.AddTexture("default_bullet", "Assets/default_bullet.png");
            GfxMgr.AddTexture("player", "Assets/Spritesheets/player.png");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            TiledMapCreator tmc = new TiledMapCreator(0);
            tmc.CreateMap();
        }
    }
}
