using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
	public class Spines : UserComponent {

		private int id;
		public int ID { get { return id; } }

		public Spines(GameObject owner, int id) : base(owner) {
			this.id = id;
		}
	}
}
