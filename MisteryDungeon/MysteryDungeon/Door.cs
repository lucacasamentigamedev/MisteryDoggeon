using System;
using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {
    internal class Door : UserComponent {

        private int id;
        public int ID { get { return id; } }

        public Door(GameObject owner, int ID) : base(owner) {
            id = ID;
        }
    }
}
