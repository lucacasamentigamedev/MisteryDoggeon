using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public enum BulletType { Arrow, Globe, GoldArrow, Last }
    public enum WeaponType { Bow, Blaster, GoldBow, Last }

    internal class BulletMgr : UserComponent {

        private float arrowDamage;
        private float arrowSpeed;
        private float globeDamage;
        private float globeSpeed;
        private Bullet[,] bulletsPool;

        public BulletMgr(GameObject owner, int poolSize,float arrowDamage,
            float arrowSpeed, float globeDamage, float globeSpeed) : base(owner) {
            this.arrowDamage = arrowDamage;
            this.globeDamage = globeDamage;
            this.arrowSpeed = arrowSpeed;
            this.globeSpeed = globeSpeed;
            bulletsPool = new Bullet[(int)BulletType.Last, poolSize];
            for (int i = 0; i < bulletsPool.GetLength(0); i++) {
                for (int j = 0; j < bulletsPool.GetLength(1); j++) {
                    switch (i) {
                        case (int)BulletType.Arrow:
                            bulletsPool[i, j] = CreateArrow(j);
                            break;
                        case (int)BulletType.Globe:
                            bulletsPool[i, j] = CreateGlobe(j);
                            break;
                        case (int)BulletType.GoldArrow:
                            bulletsPool[i, j] = CreateGoldArrow(j);
                            break;
                    }
                }
            }
        }

        private Bullet CreateArrow(int index) {
            GameObject go = new GameObject("Bullet_Arrow_" + index, Vector2.Zero, false);
            go.Tag = (int)GameObjectTag.PlayerBullet;
            go.AddComponent(SpriteRenderer.Factory(go, "arrow", new Vector2(0.5f, 0.5f), DrawLayer.Playground));
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlayerBullet;
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            rb.AddCollisionType((uint)RigidbodyType.Obstacle);
            rb.AddCollisionType((uint)RigidbodyType.SpawnPoint);
            rb.AddCollisionType((uint)RigidbodyType.Wall);
            rb.AddCollisionType((uint)RigidbodyType.Boss);
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            return go.AddComponent<Bullet>(arrowDamage, BulletType.Arrow, arrowSpeed);
        }
        
        private Bullet CreateGlobe(int index) {
            GameObject go = new GameObject("Bullet_Globe_" + index, Vector2.Zero, false);
            go.Tag = (int)GameObjectTag.EnemyBullet;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "redGlobe", new Vector2(0.5f, 0.5f), DrawLayer.Playground);
            go.AddComponent(sr);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.EnemyBullet;
            rb.AddCollisionType((uint)RigidbodyType.Player);
            rb.AddCollisionType((uint)RigidbodyType.Wall);
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            go.transform.Scale = new Vector2(TiledMapMgr.TileUnitWidth / sr.Width / 2, TiledMapMgr.TileUnitHeight / sr.Height / 2);
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            return go.AddComponent<Bullet>(globeDamage, BulletType.Globe, globeSpeed);
        }

        private Bullet CreateGoldArrow(int index) {
            GameObject go = new GameObject("Bullet_Arrow_" + index, Vector2.Zero, false);
            go.Tag = (int)GameObjectTag.PlayerBullet;
            SpriteRenderer sr = SpriteRenderer.Factory(go, "goldArrow", new Vector2(0.5f, 0.5f), DrawLayer.Playground);
            go.AddComponent(sr);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlayerBullet;
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            rb.AddCollisionType((uint)RigidbodyType.Obstacle);
            rb.AddCollisionType((uint)RigidbodyType.SpawnPoint);
            rb.AddCollisionType((uint)RigidbodyType.Wall);
            rb.AddCollisionType((uint)RigidbodyType.Boss);
            go.AddComponent(ColliderFactory.CreateUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            return go.AddComponent<Bullet>(arrowDamage, BulletType.Arrow, arrowSpeed);
        }

        public Bullet GetBullet(BulletType bulletType) {
            for (int i = 0; i < bulletsPool.GetLength(1); i++) {
                if (bulletsPool[(int)bulletType, i].gameObject.IsActive) continue;
                return bulletsPool[(int)bulletType, i];
            }
            return null;
        }
    }
}
