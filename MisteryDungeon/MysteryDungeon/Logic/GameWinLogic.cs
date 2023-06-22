using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Scenes;
using System;

namespace MisteryDungeon.MysteryDungeon.Logic {
    public class GameWinLogic : UserComponent {
        public GameWinLogic(GameObject owner) : base(owner) {}
        public override void Start() {
            EventManager.AddListener(EventList.HordeDefeated, OnCheckWinCondition);
            EventManager.AddListener(EventList.BossDefeated, OnCheckWinCondition);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.HordeDefeated, OnCheckWinCondition);
            EventManager.RemoveListener(EventList.BossDefeated, OnCheckWinCondition);
        }

        public void OnCheckWinCondition(EventArgs message) {
            if(GameStats.HordesDefeated == GameConfigMgr.HordesNumber && GameStats.BossDefeated) {
                Game.TriggerChangeScene(new WinScene());
            }
        }
    }
}
