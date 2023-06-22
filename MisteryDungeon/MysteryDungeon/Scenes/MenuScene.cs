using Aiv.Fast2D.Component.UI;
using Aiv.Fast2D.Component;
using OpenTK;
using MisteryDungeon.MysteryDungeon;

namespace MisteryDungeon {
    public class MenuScene : Scene {

        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("background", "Assets/Textures/corgi_background.jpg");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateBackground();
            CreateTitle();
            CreateMenuText();
            CreateCommands();
            CreateMenuLogic();
        }

        public static void CreateBackground() {
            GameObject background = new GameObject("background", Vector2.Zero);
            background.transform.Scale = new Vector2(1.1f, 1.1f);
            background.AddComponent(SpriteRenderer.Factory(background, "background", Vector2.Zero, DrawLayer.GUI));
            SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
            sr.Sprite.SetMultiplyTint(0.3f, 0.3f, 0.3f, 1f);
        }

        public static void CreateTitle() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject titleText = new GameObject("TitleText",
                new Vector2(Game.Win.OrthoWidth * 0.5f -
                Game.PixelsToUnit(stdFont.CharacterWidth) * 7 * 2, Game.Win.OrthoHeight * 0.25f));
            titleText.AddComponent<TextBox>(stdFont, 15, Vector2.One * 2)
                .SetText("Mystery Dungeon");
        }

        public static void CreateMenuText() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject feedbackText = new GameObject("FeedbackText", new Vector2
                    (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                    (stdFont.CharacterWidth) * 10 * 1.1f, Game.Win.OrthoHeight * 0.5f - 1));
            feedbackText.AddComponent<TextBox>(stdFont, 60, Vector2.One * 1.5f).
                SetText("Welcome!\nPress Enter to\nstart the game\nor Esc to quit");
        }

        public static void CreateCommands() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject feedbackText = new GameObject("FeedbackTextCommands", new Vector2
                        (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                        (stdFont.CharacterWidth) * 10 * 1.1f, Game.Win.OrthoHeight * 0.75f - 1));
            feedbackText.AddComponent<TextBox>(stdFont, 60, Vector2.One * 1.5f).
                SetText("Commands:\n\nMove: Left mouse\nShoot: Right mouse");
        }

        public static void CreateMenuLogic() {
            GameObject menuController = new GameObject("MenuController", Vector2.Zero);
            menuController.AddComponent<MenuLogic>("UI_Confirm", "NewLoadScene", "UI_Cancel", null, false, "", "");
        }
    }
}
