using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class Key : UserComponent {

        private int id;
        public int ID { get { return id; } }

        private int gateId;
        public int GateId { get { return gateId; } }

        public Key(GameObject owner, int id, int gateId) : base(owner) {
            this.id = id;
            this.gateId = gateId;
        }
    }
}
