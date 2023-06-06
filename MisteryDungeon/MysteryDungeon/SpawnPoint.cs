using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public enum EnemyType { Blob, Last }

    public class SpawnPoint : UserComponent {

        private Enemy[] enemiesPool;
        private float spawnTimer;
        private float currentSpawnTimer;
        private float currentReadyTimer;
        private float enemyHealth;
        private float enemySpeed;
        private float enemyDamage;

        public SpawnPoint(GameObject owner, int poolSize, EnemyType enemyType, float spawnTimer,
            float readyTimer, float enemyHealth, float enemySpeed, float enemyDamage) : base(owner) {
            currentReadyTimer = readyTimer;
            this.spawnTimer = spawnTimer;
            currentSpawnTimer = spawnTimer;
            enemiesPool = new Enemy[poolSize];
            this.enemyHealth = enemyHealth;
            this.enemySpeed = enemySpeed;
            this.enemyDamage = enemyDamage;
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
            go.Tag = (int)GameObjectTag.Enemy;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "blob", Vector2.One * 0.5f, DrawLayer.Playground);
            go.AddComponent(sr);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Enemy;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfig.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            go.IsActive = false;
            go.transform.Scale = new Vector2((GameConfig.TileUnitWidth / sr.Width), (GameConfig.TileUnitHeight / sr.Height));
            go.AddComponent<HealthModule>(enemyHealth, new Vector2(-0.5f, -0.4f));
            return go.AddComponent<Enemy>(enemySpeed, enemyDamage);
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
                EventManager.CastEvent(EventList.EnemySpawned, EventArgsFactory.EnemySpawnedFactory());
                EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw nemico in posizione " + transform.Position.ToString()));
                enemy.Spawn(transform.Position);
                currentSpawnTimer = spawnTimer;
            }
        }

        public void TakeDamage(float damage) {
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) DestroySpawnPoint();
            else EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw point colpito, vita rimanente " + hm.Health));
        }

        public void DestroySpawnPoint() {
            //TODO: suono distruzione spawn point
            EventManager.CastEvent(EventList.SpawnPointDestroyed, EventArgsFactory.SpawnPointDestroyedFactory());
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw point distrutto"));
            gameObject.IsActive = false;
        }
    }
}
