using Aiv.Fast2D.Component;
using System;

namespace MisteryDungeon.MysteryDungeon {
    public class BackgroundMusicLogic : UserComponent {

        private AudioSourceComponent audioSourceComponent;

        public BackgroundMusicLogic(GameObject owner) : base(owner) {}

        public override void Awake() {
            audioSourceComponent = GetComponent<AudioSourceComponent>();
        }

        public override void Start() {
            EventManager.AddListener(EventList.GamePlay, OnGamePlay);
            EventManager.AddListener(EventList.GamePause, OnGamePause);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.GamePlay, OnGamePlay);
            EventManager.RemoveListener(EventList.GamePause, OnGamePause);
        }

        public void OnGamePlay(EventArgs message) {
            audioSourceComponent.Play();
        }
        
        public void OnGamePause(EventArgs message) {
            audioSourceComponent.Pause();
        }
    }
}
