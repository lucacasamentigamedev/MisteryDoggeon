using Aiv.Fast2D.Component;
using OpenTK;
using System;

namespace MisteryDungeon.MysteryDungeon {

    public enum BulletType { Default, Last }

    public class BulletMgr : UserComponent {

        private Bullet[,] pool;
        private float defaultBulletDamage;

        public BulletMgr(GameObject owner, int poolSize, float defaultBulletDamage) : base(owner) {
            this.defaultBulletDamage = defaultBulletDamage;
            pool = new Bullet[(int)BulletType.Last, poolSize];
            for (int i = 0; i < pool.GetLength(0); i++) {
                for (int j = 0; j < pool.GetLength(1); j++) {
                    switch (i) {
                        case (int)BulletType.Default:
                            pool[i, j] = CreateDefaultBullet(j);
                            break;
                    }
                }
            }

        }

        public Bullet GetBullet(BulletType bulletType) {
            for (int i = 0; i < pool.GetLength(1); i++) {
                if (pool[(int)bulletType, i].gameObject.IsActive) continue;
                return pool[(int)bulletType, i];
            }
            return null;
        }

        private Bullet CreateDefaultBullet(int index) {
            GameObject bullet = new GameObject("Default_Bullet_" + index, Vector2.Zero);
            bullet.AddComponent(SpriteRenderer.Factory(bullet, "default_bullet", Vector2.One * 0.5f,
                DrawLayer.Playground));
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Bullet;
            rb.AddCollisionType((uint)RigidbodyType.Box);
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            bullet.AddComponent(ColliderFactory.CreateBoxFor(bullet));
            bullet.AddComponent<FaceVelocity>();
            Console.WriteLine("Creato " + bullet.Name + " in posizione " + Vector2.Zero.ToString());
            return bullet.AddComponent<Bullet>(defaultBulletDamage, BulletType.Default);
        }
    }
}
