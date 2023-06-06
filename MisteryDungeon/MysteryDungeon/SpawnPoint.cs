using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public enum EnemyType { Blob, Last }

    public class SpawnPoint : UserComponent {

        private Enemy[] enemiesPool;
        private float spawnTimer;
        private float currentSpawnTimer;
        private float currentReadyTimer;


        public SpawnPoint(GameObject owner, int poolSize, EnemyType enemyType, float spawnTimer, float readyTimer) : base(owner) {
            currentReadyTimer = readyTimer;
            this.spawnTimer = spawnTimer;
            currentSpawnTimer = spawnTimer;
            enemiesPool = new Enemy[poolSize];
            for (int i = 0; i < enemiesPool.Length; i++) {
                switch (enemyType) {
                    case (int)EnemyType.Blob:
                        enemiesPool[i] = CreateBlob(i);
                        break;
                }
            }
        }

        private Enemy CreateBlob(int index) {
            GameObject go = new GameObject("Enemy_Blob_" + index, Vector2.Zero, false);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "blob", Vector2.One * 0.5f, DrawLayer.Playground);
            go.AddComponent(sr);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Enemy;
            go.AddComponent(ColliderFactory.CreateBoxFor(go));
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            go.IsActive = false;
            go.transform.Scale = new Vector2((GameConfigMgr.TileUnitWidth / sr.Width), (GameConfigMgr.TileUnitHeight / sr.Height));
            return go.AddComponent<Enemy>(1.5f, 5);
        }

        public Enemy GetEnemy() {
            for (int i = 0; i < enemiesPool.Length; i++) {
                if (enemiesPool[i].gameObject.IsActive) continue;
                return enemiesPool[i];
            }
            return null;
        }

        public override void Update() {
            currentReadyTimer -= Game.DeltaTime;
            if (currentReadyTimer > 0) return;
            currentSpawnTimer -= Game.DeltaTime;
            if (currentSpawnTimer > 0) return;
            Enemy enemy = GetEnemy();
            if (enemy != null) {
                enemy.Spawn(transform.Position);
                currentSpawnTimer = spawnTimer;
            }
        }
    }
}
