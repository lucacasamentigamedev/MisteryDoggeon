using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    public class PlatformButton : UserComponent {

        private int id;
        private int sequenceId;
        public int ID { get { return id; } }
        public int SequenceId { get { return sequenceId; } }

        public PlatformButton(GameObject go, int id, int sequenceId) : base(go) {
            this.id = ID;
            this.sequenceId = sequenceId;
        }
    }
}
