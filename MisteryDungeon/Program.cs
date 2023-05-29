using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {
    internal class Program {
        static void Main(string[] args) {
            Game.Init("Tankz", 1280, 720, new Room_1(), 720, 10, 500);
        }
    }
}
