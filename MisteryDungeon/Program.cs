using Aiv.Fast2D.Component;

namespace MisteryDungeon.MysteryDungeon {

    public enum GameObjectTag {
        Player,
        Enemy,
        Door,
        PlatformButton,
        Weapon,
        PlayerBullet,
        Obstacle,
        Key
    }

    class Program {
        static void Main(string[] args) {
            Input.AddUserButton("Move", new ButtonMatch[] {
                new MouseButtonMatch(MouseButton.LeftMouse)
            });
            Input.AddUserButton("Shoot", new ButtonMatch[] {
                new MouseButtonMatch(MouseButton.RightMouse)
            });
            Game.Init("Mystery Dungeon", 720, 720, new Room_0(), 720, 10, 500);
        }
    }
}
