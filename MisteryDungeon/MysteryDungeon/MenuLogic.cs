using Aiv.Fast2D.Component;
using System;

namespace MisteryDungeon.MysteryDungeon {
    public class MenuLogic : UserComponent {

        private string uiConfirm;
        private string confirmScene;
        private string uiCancel;
        private string cancelScene;
        private bool memoryCard;

        public MenuLogic(GameObject owner, string uiConfirm, string confirmScene,
            string uiCancel, string cancelScene, bool memoryCard) : base(owner) {
            this.uiConfirm = uiConfirm;
            this.confirmScene = confirmScene;
            this.uiCancel = uiCancel;
            this.cancelScene = cancelScene;
            this.memoryCard = memoryCard;
        }

        public override void Update() {
            if (Input.GetUserButtonDown(uiConfirm)) {
                if (memoryCard) EventManager.CastEvent(EventList.NewGame, EventArgsFactory.NewGameFactory());
                Type confirm = !string.IsNullOrEmpty(confirmScene) ? Type.GetType("MisteryDungeon." + confirmScene) : null;
                Game.TriggerChangeScene(confirm != null ? Activator.CreateInstance(confirm) as Scene : null);
            } else if (Input.GetUserButtonDown(uiCancel)) {
                if (memoryCard) {
                    EventManager.CastEvent(EventList.LoadGame, EventArgsFactory.LoadGameFactory());
                    cancelScene = "Room_" + GameStatsMgr.ActualRoom;
                };
                Type cancel = !string.IsNullOrEmpty(cancelScene) ? Type.GetType("MisteryDungeon." + cancelScene) : null;
                Game.TriggerChangeScene(cancel != null ? Activator.CreateInstance(cancel) as Scene : null);
            }
        }
    }
}
