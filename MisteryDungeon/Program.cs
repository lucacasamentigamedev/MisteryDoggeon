using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    internal class Program {
        static void Main(string[] args) {
            Game.Init("Mystery Dungeon", 720, 720, new Room_0(), 720, 10, 500);
        }
    }
}
