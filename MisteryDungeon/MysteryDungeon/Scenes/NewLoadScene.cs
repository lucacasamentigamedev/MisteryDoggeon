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
            CreateBackground();
            CreateTitle();
            CreateMenu();
            CreateMenuLogic();
        }

        public void CreateLogMgr() {
            GameObject go = new GameObject("LogMgr", Vector2.Zero);
            go.AddComponent<LogMgr>(
                false,  //print pathfinding logs
                false,  //print puzzle logs
                false,  //print object creations logs
                false,  //print enemy horde logs
                false    //print memory card logs
            );
        }

        private static void CreateMemoryCardMgr() {
            GameObject memoryCardMgr = new GameObject("MemoryCardMgr", Vector2.Zero);
            memoryCardMgr.AddComponent<MemoryCardMgr>();
        }

        public static void CreateBackground() {
            GameObject background = new GameObject("background", Vector2.Zero);
            background.transform.Scale = new Vector2(1.1f, 1.1f);
            background.AddComponent(SpriteRenderer.Factory(background, "background", Vector2.Zero, DrawLayer.GUI));
            SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
            sr.Sprite.SetMultiplyTint(0.5f, 0.5f, 0.5f, 1f);
        }

        public static void CreateTitle() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject titleText = new GameObject("TitleText",
                new Vector2(Game.Win.OrthoWidth * 0.5f -
                Game.PixelsToUnit(stdFont.CharacterWidth) * 7 * 2, Game.Win.OrthoHeight * 0.25f));
            titleText.AddComponent<TextBox>(stdFont, 15, Vector2.One * 2)
                .SetText("Mystery Dungeon");
        }

        private static void CreateMenu() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject feedbackText = new GameObject("FeedbackText", new Vector2
                (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                (stdFont.CharacterWidth) * 10 * 1.3f, Game.Win.OrthoHeight * 0.5f - 1));
            feedbackText.AddComponent<TextBox>(stdFont, 100, Vector2.One * 1.5f).
                SetText("Mode:\n\nPress N to\n new game\nPress L to\n load existing game\nPress Esc to exit");
        }

        public static void CreateMenuLogic() {
            GameObject menuController = new GameObject("MenuController", Vector2.Zero);
            menuController.AddComponent<MenuLogic>("UI_N", "Room_0", "UI_L", "", true, "UI_Cancel", null);
        }
    }
}
