using Aiv.Fast2D.Component;
using Aiv.Fast2D;

namespace MisteryDungeon.MysteryDungeon.Shader {
    public class WobbleEffectShader : PostProcessingEffect {

        private static string fragmentShader = @"
            #version 330 core
        
            precision highp float;

            in vec2 uv;
            uniform sampler2D tex;
            uniform float wave;

            out vec4 color;

            void main () {
                vec2 uv2 = uv;
                uv2.x += sin(uv2.y * 9 * 3.14159 + wave) / 100;
                color = texture (tex, uv2);
            }"
        ;

        private float wave;
        private float speed;

        public WobbleEffectShader(float speed) : base(fragmentShader) {
            this.speed = speed;
        }

        public override void Update(Window window) {
            wave += Game.DeltaTime * speed;
            screenMesh.shader.SetUniform("wave", wave);
        }
    }
}