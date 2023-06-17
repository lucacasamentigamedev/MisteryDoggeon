using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    public class PlatformButton : UserComponent {

        private int id;
        public int ID { get { return id; } }
        private int sequenceId;
        public int SequenceId { get { return sequenceId; } }
        private float buttonWidth;
        public bool Pressed { get; set; }

        SpriteRenderer spriteRenderer;

        public PlatformButton(GameObject go, int id, int sequenceId) : base(go) {
            this.id = id;
            this.sequenceId = sequenceId;
        }

        public override void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.Width = Game.PixelsToUnit(spriteRenderer.Texture.Width / 2);
            spriteRenderer.Height = Game.PixelsToUnit(spriteRenderer.Texture.Height);
            buttonWidth = Game.UnitToPixels(spriteRenderer.WidthUnscaled);
            Pressed = GameStatsMgr.PuzzleResolved;
            ChangeButtonState(GameStatsMgr.PuzzleResolved);
        }

        public void ChangeButtonState(bool pressedState) {
            if (pressedState == Pressed) return;
            Pressed = pressedState;
            spriteRenderer.TextureOffset = pressedState ? new Vector2(buttonWidth, 0) : Vector2.Zero;
        }
    }
}
