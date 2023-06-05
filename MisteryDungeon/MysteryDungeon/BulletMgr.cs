using System;
using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public enum BulletType { Arrow, Last }

    internal class BulletMgr : UserComponent {

        private Bullet[,] bulletsPool;

        public BulletMgr(GameObject owner, int poolSize) : base(owner) {
            bulletsPool = new Bullet[(int)BulletType.Last, poolSize];
            for (int i = 0; i < bulletsPool.GetLength(0); i++) {
                for (int j = 0; j < bulletsPool.GetLength(1); j++) {
                    switch (i) {
                        case (int)BulletType.Arrow:
                            bulletsPool[i, j] = CreateArrow(j);
                            break;
                    }
                }
            }
        }

        private Bullet CreateArrow(int index) {
            GameObject bullet = new GameObject("Bullet_Arrow_" + index, Vector2.Zero, false);
            bullet.AddComponent(SpriteRenderer.Factory(bullet, "arrow", new Vector2(1, 0.5f), DrawLayer.Playground));
            Rigidbody rb = bullet.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.PlayerBullet;
            rb.AddCollisionType((uint)RigidbodyType.Enemy);
            rb.AddCollisionType((uint)RigidbodyType.Obstacle);
            bullet.AddComponent(ColliderFactory.CreateBoxFor(bullet));
            bullet.AddComponent<FaceVelocity>();
            return bullet.AddComponent<Bullet>(5, BulletType.Arrow, 5);
        }

        public Bullet GetBullet(BulletType bulletType) {
            for (int i = 0; i < bulletsPool.GetLength(1); i++) {
                if (bulletsPool[(int)bulletType, i].gameObject.IsActive) { Console.WriteLine("indice " + i + "gia attivo"); continue; };
                Console.WriteLine("sparo il " + i);
                return bulletsPool[(int)bulletType, i];
            }
            return null;
        }
    }
}
