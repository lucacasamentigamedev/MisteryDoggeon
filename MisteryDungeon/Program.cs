using Aiv.Fast2D;
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
        EnemyBullet,
        MemoryCard,
        Hearth
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
            Input.AddUserButton("Pause", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.Esc)
            });
            Input.AddUserButton("UI_Confirm", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.Return),
                new KeyButtonMatch(KeyCode.Y)
            });
            Input.AddUserButton("UI_Cancel", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.Esc),
                new KeyButtonMatch(KeyCode.N)
            });
            Input.AddUserButton("UI_1", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.Num1)
            });
            Input.AddUserButton("UI_2", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.Num2)
            });
            Input.AddUserButton("UI_N", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.N)
            });
            Input.AddUserButton("UI_L", new ButtonMatch[] {
                new KeyButtonMatch(KeyCode.L)
            });
            AudioMgr.AddVolume((int)AudioLayer.music, 0.7f); 
            AudioMgr.AddVolume((int)AudioLayer.sfx, 0.5f);
            Game.Init("Mystery Dungeon", 720, 720, new MenuScene(), 720, 10, 500);
        }
    }
}
