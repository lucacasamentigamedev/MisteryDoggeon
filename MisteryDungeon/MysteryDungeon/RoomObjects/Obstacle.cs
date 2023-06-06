using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon.Rooms {
    public class Obstacle: UserComponent {

        private int id;
        public int ID { get { return id; } }

        private int roomId;
        public int RoomId { get { return roomId; } }

        public Obstacle(GameObject owner, int id, int roomId) : base(owner) {
            this.id = id;
            this.roomId = roomId;
        }
    }
}
