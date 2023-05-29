namespace Aiv.Fast2D.Component {
    static class Game {

        private static bool changeScene;
        private static Scene nextScene;

        private static bool firstFrameScene;

        public static float Gravity = 500f;

        public static Window Win;
        public static bool IsRunning;
        public static float DeltaTime {
            get { return firstFrameScene ? 0 :  Win.DeltaTime * timeScale; }
        }
        public static float UnscaledDeltaTime {
            get { return firstFrameScene ? 0 : Win.DeltaTime; }
        }
        private static float timeScale = 1;
        public static float TimeScale {
            get { return timeScale; }
            set { timeScale = value; }
        }
        private static Scene currentScene;
        public static Scene CurrentScene {
            get { return currentScene; }
        }

        public static float WorkingHeight {
            get;
            private set;
        }

        public static float UnitSize {
            get; private set;
        }

        public static void Init (string windowName, int windowWidth, int windowHeight,  
            Scene startScene, float workingHeight, float ortographicSize, float gravity) {
            Win = new Window(windowWidth, windowHeight, windowName);
            Win.SetFullScreen(false);
            Win.SetDefaultViewportOrthographicSize(ortographicSize);
            Win.SetVSync(false);

            WorkingHeight = workingHeight;
            UnitSize = WorkingHeight / ortographicSize;
            Gravity = PixelsToUnit(gravity);
            nextScene = startScene;
            ChangeScene();

            Play();
        }

        public static float PixelsToUnit (float pixelSize) {
            return pixelSize / UnitSize;
        }

        public static float UnitToPixels (float unit) {
            return unit * UnitSize;
        }

        public static void Play () {
            IsRunning = true;

            while (Win.IsOpened && IsRunning) {

                PhysicsMgr.FixedUpdate();
                PhysicsMgr.CheckCollisions();
                currentScene.Update();
                currentScene.LateUpdate();

                CameraMgr.MoveCameras();

                DrawMgr.Draw();
                Input.PerformLastKey();
                Win.Update();
                firstFrameScene = false;
                if (changeScene) {
                    changeScene = false;
                    ChangeScene();
                }

            }
        }

        public static void TriggerChangeScene (Scene nextScene) {
            changeScene = true;
            Game.nextScene = nextScene;
        }

        private static void ChangeScene () {
            if (currentScene != null) {
                currentScene.DestroyScene();
            }
            if (nextScene == null) {
                IsRunning = false;
                return;
            }
            firstFrameScene = true;
            currentScene = nextScene;
            currentScene.InitializeScene();
            currentScene.Awake();
            currentScene.Start();
        }

    }
}
