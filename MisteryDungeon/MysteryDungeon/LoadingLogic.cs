using Aiv.Fast2D.Component;
using System;

namespace MisteryDungeon.MysteryDungeon.Mgr {
    public class LoadingLogic : UserComponent {

        SpriteRenderer sr;

        public LoadingLogic(GameObject owner) : base(owner) { }

        public override void Awake() {
            sr = GetComponent<SpriteRenderer>();
        }

        public override void Start() {
            EventManager.AddListener(EventList.StartLoading, OnStartLoading);
            EventManager.AddListener(EventList.EndLoading, OnEndLoading);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.StartLoading, OnStartLoading);
            EventManager.RemoveListener(EventList.EndLoading, OnEndLoading);
        }

        public void OnStartLoading(EventArgs message) {
            sr.Enabled = true;
            //Game.TimeScale = 0;
        }

        public void OnEndLoading(EventArgs message) {
            sr.Enabled = false;
            //Game.TimeScale = 1;
        }
    }
}
