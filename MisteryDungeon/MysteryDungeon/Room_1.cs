using Aiv.Fast2D.Component;
using Aiv.Fast2D.Component.UI;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    internal class Room_1: Scene {
        protected override void LoadAssets() {
            FontMgr.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
        }

        public override void InitializeScene() {
            base.InitializeScene();
            TestTitle();
        }

        public void TestTitle() {
            Font stdFont = FontMgr.GetFont("stdFont");
            GameObject testTitle = new GameObject("TestTitle",
                new Vector2(Game.Win.OrthoWidth * 0.3f, Game.Win.OrthoHeight * 0.5f));
            testTitle.AddComponent<TextBox>(stdFont, 14, Vector2.One * 3)
                .SetText("Test title");
        }
    }
}
