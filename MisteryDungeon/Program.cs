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
        Key,
        SpawnPoint,
        Wall,
        Boss,
        EnemyBullet
    }

    public enum AudioLayer {
        music,
        sfx,
        last
    }

    class Program {
        static void Main(string[] args) {
            Input.AddUserButton("Move", new ButtonMatch[] {
                new MouseButtonMatch(MouseButton.LeftMouse)
            });
            Input.AddUserButton("Shoot", new ButtonMatch[] {
                new MouseButtonMatch(MouseButton.RightMouse)
            });
            AudioMgr.AddVolume((int)AudioLayer.music, 0.7f); 
            AudioMgr.AddVolume((int)AudioLayer.sfx, 0.5f);
            Game.Init("Mystery Dungeon", 720, 720, new Room_0(), 720, 10, 500);
        }
    }
}
