using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Mgr;

namespace MisteryDungeon.MysteryDungeon {
    public class MemoryCard : UserComponent {

        private int id;
        public int ID { get { return id; } }
        private int roomId;
        public int RoomId { get { return roomId; } }
        private float respawnTimer;
        private float currentRespawnTimer;
        private bool ready;
        public bool Ready { get { return ready; } }
        private SpriteRenderer sr;

        public MemoryCard(GameObject owner, int id, int roomId, float respawnTimer) : base(owner) {
            this.id = id;
            this.roomId = roomId;
            this.respawnTimer = respawnTimer;
        }

        public override void Awake() {
            sr = GetComponent<SpriteRenderer>();
            ResetTimer();
        }

        public override void Update() {
            if (ready) return;
            currentRespawnTimer -= Game.DeltaTime;
            if (currentRespawnTimer > 0) return;
            ResetTimer();
        }

        public void PickedMemoryCard() {
            EventManager.CastEvent(EventList.StartLoading, EventArgsFactory.StartLoadingFactory());
            EventManager.CastEvent(EventList.ObjectPicked, EventArgsFactory.ObjectPickedFactory());
            EventManager.CastEvent(EventList.SaveGame, EventArgsFactory.SaveGameFactory());
            sr.Enabled = false;
            ready = false;
        }

        public void ResetTimer() {
            sr.Enabled = true;
            ready = true;
            currentRespawnTimer = respawnTimer;
        }
    }
}