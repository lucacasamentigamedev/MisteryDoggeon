using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class PauseLogic : UserComponent {

        //Reference
        private GameObject[] pauseObjects;

        //WorkingVariable
        private string[] pauseObjectsName;
        public bool InPause { get; set; }
        private string playAction;

        public PauseLogic(GameObject owner, string[] pauseObjectsName, string pauseAction) : base(owner) {
            this.pauseObjectsName = pauseObjectsName;
            this.playAction = pauseAction;
        }

        public override void Awake() {
            pauseObjects = new GameObject[pauseObjectsName.Length];
            for (int i = 0; i < pauseObjects.Length; i++) {
                pauseObjects[i] = GameObject.Find(pauseObjectsName[i]);
                pauseObjects[i].IsActive = false;
            }
        }

        public override void Update() {
            if (Input.GetUserButtonDown(playAction)) {
                InPause = !InPause;
                for (int i = 0; i < pauseObjects.Length; i++) {
                    pauseObjects[i].IsActive = InPause;
                }
                Game.TimeScale = InPause ? 0 : 1;
                if (InPause) EventManager.CastEvent(EventList.GamePause, EventArgsFactory.GamePauseFactory());
                else EventManager.CastEvent(EventList.GamePlay, EventArgsFactory.GamePlayFactory());
            }
        }
    }
}
