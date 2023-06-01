using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon.Rooms {
    internal class Obstacle: UserComponent {
        private int id;
        public int ID { get { return id; } }

        public Obstacle(GameObject owner, int id) : base(owner) {
            this.id = id;
        }
    }
}
