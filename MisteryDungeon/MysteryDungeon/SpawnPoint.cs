﻿using System;
using Aiv.Fast2D.Component;
using OpenTK;

namespace MisteryDungeon.MysteryDungeon {

    public enum EnemyType { Blob, Last }

    public class SpawnPoint : UserComponent {

        private LittleBlobController[] enemiesPool;
        private float spawnTimer;
        private float currentSpawnTimer;
        private float currentReadyTimer;
        private float enemyHealth;
        private float enemyMinSpeed;
        private float enemyMaxSpeed;
        private float enemyDamage;
        private float deathTimer;

        public SpawnPoint(GameObject owner, int poolSize, EnemyType enemyType, float spawnTimer,
            float readyTimer, float enemyHealth, float enemyMinSpeed, float enemyMaxSpeed, float enemyDamage, float deathTimer) : base(owner) {
            currentReadyTimer = readyTimer;
            this.spawnTimer = spawnTimer;
            currentSpawnTimer = 0;
            enemiesPool = new LittleBlobController[poolSize];
            this.enemyHealth = enemyHealth;
            this.enemyMinSpeed = enemyMinSpeed;
            this.enemyMaxSpeed = enemyMaxSpeed;
            this.enemyDamage = enemyDamage;
            this.deathTimer = deathTimer;
            for (int i = 0; i < enemiesPool.Length; i++) {
                switch (enemyType) {
                    case (int)EnemyType.Blob:
                        enemiesPool[i] = CreateBlob(i);
                        break;
                }
            }
        }

        private LittleBlobController CreateBlob(int index) {
            GameObject go = new GameObject("Enemy_Blob_" + index, Vector2.Zero, false);
            go.IsActive = false;
            go.Tag = (int)GameObjectTag.Enemy;
            Sheet sheet = new Sheet(GfxMgr.GetTexture("greenBlob"), 4, 2);
            SpriteRenderer sr = SpriteRenderer.Factory(go, "greenBlob", Vector2.One * 0.5f, DrawLayer.Playground, sheet.FrameWidth, sheet.FrameHeight);
            go.AddComponent(sr);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.Type = RigidbodyType.Enemy;
            go.AddComponent(ColliderFactory.CreateHalfUnscaledBoxFor(go));
            if (GameConfigMgr.debugBoxColliderWireframe) go.GetComponent<BoxCollider>().DebugMode = true;
            EventManager.CastEvent(EventList.LOG_GameObjectCreation, EventArgsFactory.LOG_Factory("Creato " + go.Name + " in posizione " + Vector2.Zero));
            go.transform.Scale = new Vector2((TiledMapMgr.TileUnitWidth / sr.Width), (TiledMapMgr.TileUnitHeight / sr.Height));
            go.AddComponent<HealthModule>(enemyHealth, enemyHealth, new Vector2(-0.5f, -0.4f));
            CreateBlobAnimations(go, sheet);
            return go.AddComponent<LittleBlobController>(enemyDamage, deathTimer);
        }

        private static void CreateBlobAnimations(GameObject go, Sheet sheet) {
            SheetClip walking = new SheetClip(
                sheet, "walking", new int[] { 0, 1, 2, 3, 4 }, true, 7
            );
            SheetClip death = new SheetClip(
                sheet, "death", new int[] { 5, 6, 7 }, false, 7
            );
            SheetAnimator animator = go.AddComponent<SheetAnimator>(go.GetComponent<SpriteRenderer>());
            animator.AddClip(walking);
            animator.AddClip(death);
        }

        public LittleBlobController GetEnemy() {
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
            LittleBlobController enemy = GetEnemy();
            if (enemy != null) {
                enemy.Spawn(transform.Position, RandomGenerator.GetRandomFloat(enemyMinSpeed, enemyMaxSpeed));
                currentSpawnTimer = spawnTimer;
                EventManager.CastEvent(EventList.EnemySpawned, EventArgsFactory.EnemySpawnedFactory());
                EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw blob nemico"));
            }
        }

        public void TakeDamage(float damage) {
            HealthModule hm = GetComponent<HealthModule>();
            if (hm.TakeDamage(damage)) DestroySpawnPoint();
            else EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw point colpito, vita rimanente " + hm.Health));
        }

        public void DestroySpawnPoint() {
            EventManager.CastEvent(EventList.SpawnPointDestroyed, EventArgsFactory.SpawnPointDestroyedFactory());
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Spaw point distrutto"));
            gameObject.IsActive = false;
        }
    }
}
