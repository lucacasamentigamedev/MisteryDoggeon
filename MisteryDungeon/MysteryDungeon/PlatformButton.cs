using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    internal class PlatformButton : UserComponent {

        private int id;
        private int sequenceStep;
        public int ID { get { return id; } }
        public int SequenceStep { get { return sequenceStep; } }

        public PlatformButton(GameObject go, int id, int sequenceStep) : base(go) {
            this.id = ID;
            this.sequenceStep = sequenceStep;
        }
    }
}
