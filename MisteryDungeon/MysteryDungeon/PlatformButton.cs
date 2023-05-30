using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    internal class PlatformButton : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public PlatformButton(GameObject go, int ID) : base(go) {
            id = ID;
        }
    }
}
