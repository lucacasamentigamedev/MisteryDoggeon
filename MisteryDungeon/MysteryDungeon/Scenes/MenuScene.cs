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
            CreateMenuScene();
        }

        public static void CreateMenuScene() {
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
                (stdFont.CharacterWidth) * 10 * 1.1f, Game.Win.OrthoHeight * 0.5f - 1));
            feedbackText.AddComponent<TextBox>(stdFont, 60, Vector2.One * 1.5f).
                SetText("Press Enter to\nstart the game\nor Esc to exit\nfrom the game");
            GameObject menuController = new GameObject("MenuController", Vector2.Zero);
            menuController.AddComponent<MenuLogic>("UI_Confirm", "NewLoadScene", "UI_Cancel", null, false);
        }
    }
}
