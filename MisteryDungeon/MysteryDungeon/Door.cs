using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class Door : UserComponent {

        private int id;
        public int ID { get { return id; } }
        private int roomToGo;
        public int RoomToGo { get { return roomToGo; } }

        public Door(GameObject owner, int id, int roomToGo) : base(owner) {
            this.id = id;
            this.roomToGo = roomToGo;
        }
    }
}
