using Aiv.Fast2D.Component.UI;
using Aiv.Fast2D.Component;
using OpenTK;
using MisteryDungeon.MysteryDungeon;
using MisteryDungeon.MysteryDungeon.Mgr;

namespace MisteryDungeon {
    public class NewLoadScene : Scene {

        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("background", "Assets/Textures/corgi_background.jpg");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateLogMgr();
            CreateMemoryCardMgr();
            CreateNewLoadMenu();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                false,  //print enemy horde logs
                true    //print memory card logs
            );
        }

        private static void CreateMemoryCardMgr() {
            GameObject memoryCardMgr = new GameObject("MemoryCardMgr", Vector2.Zero);
            memoryCardMgr.AddComponent<MemoryCardMgr>();
        }

        private static void CreateNewLoadMenu() {
            GameObject background = new GameObject("background", Vector2.Zero);
            background.transform.Scale = new Vector2(1.1f, 1.1f);
            background.AddComponent(SpriteRenderer.Factory(background, "background", Vector2.Zero, DrawLayer.GUI));
            SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
            sr.Sprite.SetMultiplyTint(0.5f, 0.5f, 0.5f, 1f);
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject titleText = new GameObject("TitleText",
                new Vector2(Game.Win.OrthoWidth * 0.5f -
                Game.PixelsToUnit(stdFont.CharacterWidth) * 7 * 2, Game.Win.OrthoHeight * 0.25f));
            titleText.AddComponent<TextBox>(stdFont, 15, Vector2.One * 2)
                .SetText("Mystery Dungeon");
            GameObject feedbackText = new GameObject("FeedbackText", new Vector2
                (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                (stdFont.CharacterWidth) * 10 * 1.3f, Game.Win.OrthoHeight * 0.5f - 1));
            feedbackText.AddComponent<TextBox>(stdFont, 60, Vector2.One * 1.5f).
                SetText("Press 1 to new game\nor 2 for loading\nexisting game");
            GameObject menuController = new GameObject("MenuController", Vector2.Zero);
            menuController.AddComponent<MenuLogic>("UI_1", "Room_0", "UI_2", "", true);
        }
    }
}
