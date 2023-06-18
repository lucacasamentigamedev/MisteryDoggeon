using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon.RoomObjects {
    public class Hearth : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public Hearth(GameObject owner, int id) : base(owner) {
            this.id = id;
        }
    }
}
