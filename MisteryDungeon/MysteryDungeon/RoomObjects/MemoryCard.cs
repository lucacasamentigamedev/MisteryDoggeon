using Aiv.Fast2D.Component;
using MisteryDungeon.MysteryDungeon.Mgr;

namespace MisteryDungeon.MysteryDungeon {
    public class MemoryCard : UserComponent {

        private int id;
        public int ID { get { return id; } }

        private int roomId;
        public int RoomId { get { return roomId; } }

        public MemoryCard(GameObject owner, int id, int roomId) : base(owner) {
            this.id = id;
            this.roomId = roomId;
        }
    }
}