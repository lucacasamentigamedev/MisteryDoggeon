using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    internal class LampGate : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public LampGate(GameObject owner, int ID) : base(owner) {
            id = ID;
        }
    }
}
