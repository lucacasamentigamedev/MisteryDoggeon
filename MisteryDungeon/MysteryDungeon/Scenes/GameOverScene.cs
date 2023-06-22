using Aiv.Fast2D.Component.UI;
using Aiv.Fast2D.Component;
using OpenTK;
using MisteryDungeon.MysteryDungeon;
using System;

namespace MisteryDungeon.Scenes {
    public class GameOverScene : Scene {

        protected override void LoadAssets() {
            FontMgr.AddFont("std_font", "Assets/Textures/text_sheet.png", 15, 32, 20, 20);
            GfxMgr.AddTexture("background", "Assets/Textures/corgi_background.jpg");
            AudioMgr.AddClip("background", "Assets/Sounds/Background/background5.ogg");
        }

        public override void InitializeScene() {
            base.InitializeScene();
            CreateBackground();
            CreateTitle();
            CreateStatistics();
            CreateMenuText();
            CreateMenuLogic();
            CreateBackgroundMusic();
        }

        public void CreateBackground() {
            GameObject background = new GameObject("background", Vector2.Zero);
            background.transform.Scale = new Vector2(1.1f, 1.1f);
            background.AddComponent(SpriteRenderer.Factory(background, "background", Vector2.Zero, DrawLayer.GUI));
            SpriteRenderer sr = background.GetComponent<SpriteRenderer>();
            sr.Sprite.SetMultiplyTint(0.3f, 0.3f, 0.3f, 1f);
        }

        public void CreateTitle() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject titleText = new GameObject("TitleText",
                new Vector2(Game.Win.OrthoWidth * 0.5f -
                Game.PixelsToUnit(stdFont.CharacterWidth) * 10, 1));
            titleText.AddComponent<TextBox>(stdFont, 15, Vector2.One * 2)
                .SetText("Game Over...");
        }

        public void CreateStatistics() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject temp = new GameObject("Statistics title", new Vector2
                    (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit(stdFont.CharacterWidth) * 10, Game.Win.OrthoHeight * 0.3f));
            temp.AddComponent<TextBox>(stdFont, 100, Vector2.One * 1.5f).
                SetText("Statistics:");
            temp = new GameObject("Statistics text", new Vector2
                    (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                    (stdFont.CharacterWidth) * 10 * 1.5f, Game.Win.OrthoHeight * 0.4f));
            temp.AddComponent<TextBox>(stdFont, 100, Vector2.One * 1.5f).
                SetText(
                    "Game Time: " + TimeSpan.FromSeconds(GameStats.ElapsedTime).ToString("hh':'mm':'ss") + "\n" +
                    "Enemies killed: " + GameStats.EnemiesKilled + "\n" +
                    "Arrows shot: " + GameStats.ArrowsShot + "\n" +
                    "Objects destroyed: " + GameStats.ObjectsDestroyed + "\n"
                );
        }

        public void CreateMenuText() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject feedbackText = new GameObject("FeedbackText", new Vector2
                    (Game.Win.OrthoWidth * 0.5f - Game.PixelsToUnit
                    (stdFont.CharacterWidth) * 9, Game.Win.OrthoHeight * 0.7f));
            feedbackText.AddComponent<TextBox>(stdFont, 60, Vector2.One * 1.5f).
                SetText("Press Enter\nto return to\nmain menu or\nEsc to exit");
        }

        public static void CreateMenuLogic() {
            GameObject menuController = new GameObject("MenuController", Vector2.Zero);
            menuController.AddComponent<MenuLogic>("UI_Confirm", "MenuScene", "UI_Cancel", null, false, "", "");
        }

        private static void CreateBackgroundMusic() {
            GameObject gameLogic = new GameObject("BackgroundMusic", Vector2.Zero);
            AudioSourceComponent audioSource = gameLogic.AddComponent<AudioSourceComponent>();
            gameLogic.AddComponent<BackgroundMusicLogic>();
            audioSource.SetClip(AudioMgr.GetClip("background"));
            audioSource.Loop = true;
            audioSource.Play();
        }
    }
}
