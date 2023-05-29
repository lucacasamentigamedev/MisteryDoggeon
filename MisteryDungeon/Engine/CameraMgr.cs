using OpenTK;
using System;
using System.Collections.Generic;

namespace Aiv.Fast2D.Component {

    public struct CameraLimits {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;

        public CameraLimits (float maxX, float minX, float maxY, float minY) {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }

    }

    public static class CameraMgr {


        private static Dictionary<string, Tuple<Camera, float>> cameras;

        private static CameraLimits limits;

        private static float speed;
        public static float Speed {
            get { return speed; }
            set { speed = value == 0 ? speed : value; }
        }
        public static GameObject target;
        public static Camera MainCamera { get; private set; }

        static CameraMgr () {
            MainCamera = new Camera();
            speed = 10f;
            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void Init (Vector2 position, Vector2 pivot) {
            MainCamera.position = position;
            MainCamera.pivot = pivot;
        }

        public static void SetCameraLimits (float minX, float maxX, float minY,
            float maxY) {
            limits.MinX = minX;
            limits.MaxX = maxX;
            limits.MinY = minY;
            limits.MaxY = maxY;
        }

        public static void AddCamera (string cameraName, Camera camera = null, float cameraSpeed = 0.0f) {
            if (camera == null) {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }
            cameras.Add(cameraName, new Tuple<Camera, float>(camera, cameraSpeed));
        }

        public static Camera GetCamera (string cameraName) {
            if (!cameras.ContainsKey(cameraName)) return null;
            return cameras[cameraName].Item1;
        }

        public static void MoveCameras () {
            if (target == null) return;
            Vector2 oldCameraPos = MainCamera.position;
            MainCamera.position = Vector2.Lerp(MainCamera.position, target.transform.Position, 
                speed * Game.Win.DeltaTime);
            FixPosition();
            Vector2 cameraDelta = MainCamera.position - oldCameraPos;
            foreach (var cam in cameras) {
                cam.Value.Item1.position += cameraDelta * cam.Value.Item2;
            }
        }

        public static bool InsideCameraLimits (Vector2 position) {
            return position.X > limits.MinX - MainCamera.pivot.X
                && position.X < limits.MaxX + (Game.Win.OrthoWidth  - MainCamera.pivot.X)
                && position.Y > limits.MinY - MainCamera.pivot.Y
                && position.Y < limits.MaxY + (Game.Win.OrthoHeight - MainCamera.pivot.Y);
        }

        private static void FixPosition () {
            if (MainCamera.position.Y < limits.MinY) {
                MainCamera.position.Y = limits.MinY;
            } else if (MainCamera.position.Y > limits.MaxY) {
                MainCamera.position.Y = limits.MaxY;
            }
            if (MainCamera.position.X < limits.MinX) {
                MainCamera.position.X = limits.MinX;
            } else if (MainCamera.position.X > limits.MaxX) {
                MainCamera.position.X = limits.MaxX;
            }
        }

        public static void ClearAll () {
            cameras.Clear();
            limits = new CameraLimits(MainCamera.pivot.X,
                MainCamera.pivot.X, MainCamera.pivot.Y, MainCamera.pivot.Y);
        }

    }
}
