using OpenTK;
using System;

namespace Aiv.Fast2D.Component {
    public class Transform {

        private Vector2 position;
        public Vector2 Position {
            get { return position; }
            set { position = value; }
        }
        private float rotation;
        public float Rotation {
            get { return rotation; }
            set { rotation = value; }
        }
        private Vector2 scale;
        public Vector2 Scale {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Forward {
            get {
                return new Vector2((float)Math.Cos(DegreesToRadaints(rotation)),
                    (float)Math.Sin(DegreesToRadaints(rotation)));
            }
            set {
                rotation = RadiantsToDegrees((float)Math.Atan2(value.Y , value.X));
            }
        }

        public Transform (Vector2 position, Vector2 scale, float rotation = 0) {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public static float RadiantsToDegrees (float radiant) {
            //return MathHelper.RadiansToDegrees(radiant);
            return 180 / MathHelper.Pi * radiant;
        }

        public static float DegreesToRadaints (float degrees) {
            //return MathHelper.DegreesToRadians(degrees);
            return MathHelper.Pi * degrees / 180;
        }

    }
}
