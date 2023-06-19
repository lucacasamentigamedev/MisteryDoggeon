using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class Key : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public Key(GameObject owner, int id) : base(owner) {
            this.id = id;
        }
    }
}
