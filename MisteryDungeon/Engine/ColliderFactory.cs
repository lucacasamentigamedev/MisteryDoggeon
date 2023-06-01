using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public static class ColliderFactory {

        public static Collider CreateCircleFor(GameObject obj) {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            float halfDiagonal = (float)(Math.Sqrt(sr.Width * sr.Width + sr.Height * sr.Height)) * 0.5f;
            return new CircleCollider(obj, halfDiagonal, Vector2.Zero);
        }

        public static Collider CreateBoxFor(GameObject obj) {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            return new BoxCollider(obj, sr.Width, sr.Height, 
                new Vector2 ((0.5f - sr.Pivot.X) * sr.Width, (0.5f - sr.Pivot.Y) * sr.Height));
        }

        public static Collider CreateUnscaledBoxFor(GameObject obj) {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            return new BoxCollider(obj, sr.WidthUnscaled, sr.HeightUnscaled,
                new Vector2((0.5f - sr.Pivot.X) * sr.WidthUnscaled, (0.5f - sr.Pivot.Y) * sr.HeightUnscaled));
        }
        public static Collider CreateHalfUnscaledBoxFor(GameObject obj) {
            SpriteRenderer sr = obj.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            return new BoxCollider(obj, sr.WidthUnscaled / 2, sr.HeightUnscaled / 2,
                new Vector2((0.5f - sr.Pivot.X) * sr.WidthUnscaled, (0.5f - sr.Pivot.Y) * sr.HeightUnscaled));
        }
    }
}
