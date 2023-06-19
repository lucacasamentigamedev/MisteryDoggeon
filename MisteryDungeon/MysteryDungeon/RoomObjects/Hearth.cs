using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon.RoomObjects {
    public class Hearth : UserComponent {

        private int id;
        public int ID { get { return id; } }

        private int roomId;
        public int RoomId { get { return roomId; } }

        public Hearth(GameObject owner, int id, int roomId) : base(owner) {
            this.id = id;
            this.roomId = roomId;
        }
    }
}
