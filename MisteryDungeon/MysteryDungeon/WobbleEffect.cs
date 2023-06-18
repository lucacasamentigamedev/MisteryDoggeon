using Aiv.Fast2D;
using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Shader;

namespace MisteryDungeon.MysteryDungeon {
    internal class WobbleEffect : UserComponent {

        PostProcessingEffect ppe;

        public WobbleEffect(GameObject owner, int intensity) : base(owner) {
            ppe = Game.Win.AddPostProcessingEffect(new WobbleEffectShader(intensity));
        }

        public override void OnEnable() {
            ppe.enabled = true;
        }

        public override void OnDisable() {
            ppe.enabled = false;
        }
    }
}
