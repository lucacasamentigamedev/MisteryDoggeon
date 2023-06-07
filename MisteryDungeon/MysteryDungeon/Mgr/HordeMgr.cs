using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public class HordeMgr : UserComponent {

        List<SpawnPoint> spawnPoints;
        private bool hordeActive;
        public static int EnemiesActive { get; set; }
        public static int SpawnPointsActive { get; set; }

        public HordeMgr(GameObject owner) : base(owner) {
            spawnPoints = new List<SpawnPoint>();
            hordeActive = false;
            EnemiesActive = 0;
            SpawnPointsActive = 0;
        }

        public void AddSpawnPoint(SpawnPoint spawnPoint) {
            spawnPoints.Add(spawnPoint);
            SpawnPointsActive++;
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Aggiunto spawn point, spawn point attivi " + SpawnPointsActive));
        }

        public override void Update() {
            CheckHordeActivation();
        }

        public override void Start() {
            EventManager.AddListener(EventList.EnemySpawned, OnEnemySpawned);
            EventManager.AddListener(EventList.EnemyDestroyed, OnEnemyDestroyed);
            EventManager.AddListener(EventList.SpawnPointDestroyed, OnSpawnPointDestroyed);
        }

        public override void OnDestroy() {
            EventManager.RemoveListener(EventList.EnemySpawned, OnEnemySpawned);
            EventManager.RemoveListener(EventList.EnemyDestroyed, OnEnemyDestroyed);
            EventManager.RemoveListener(EventList.SpawnPointDestroyed, OnSpawnPointDestroyed);
        }

        private void OnEnemySpawned(EventArgs message) {
            EnemiesActive++;
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Nemici attivi: " + EnemiesActive + " SpawnPoint attivi: " + SpawnPointsActive));
        }

        private void OnEnemyDestroyed(EventArgs message) {
            EnemiesActive--;
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Nemici attivi: " + EnemiesActive + " SpawnPoint attivi: " + SpawnPointsActive));
            CheckHordeDefeated();
        }
        
        private void OnSpawnPointDestroyed(EventArgs message) {
            SpawnPointsActive--;
            EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Nemici attivi: " + EnemiesActive + " SpawnPoint attivi: " + SpawnPointsActive));
            CheckHordeDefeated();
        }

        private void CheckHordeDefeated() {
            if (EnemiesActive <= 0 && SpawnPointsActive <= 0) {
                //orda sconfitta tolgo i gate
                EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Orda sconfitta"));
                GameStats.HordeDefeated = true;
                GameObject.Find("Object_2_39").IsActive = false;
                RoomObjectsMgr.SetRoomObjectActiveness(2, 39, false, true, MovementGrid.EGridTile.Floor);
                GameObject.Find("Object_2_38").IsActive = false;
                RoomObjectsMgr.SetRoomObjectActiveness(2, 38, false, true, MovementGrid.EGridTile.Floor);
            }
        }

        public void CheckHordeActivation() {
            if (GameStats.HordeDefeated || GameStats.ActiveWeapon == null || hordeActive) return;
            //attivo gate
            GameObject.Find("Object_2_39").IsActive = true;
            RoomObjectsMgr.SetRoomObjectActiveness(2, 39, true, true, MovementGrid.EGridTile.Wall);
            //attivo spawn point
            foreach (SpawnPoint spawnPoint in spawnPoints) {
                spawnPoint.gameObject.IsActive = true;
            }
            hordeActive = true;
        }
    }
}
