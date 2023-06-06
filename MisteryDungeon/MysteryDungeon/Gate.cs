using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class Gate : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public Gate(GameObject owner, int id) : base(owner) {
            this.id = id;
        }
    }
}
