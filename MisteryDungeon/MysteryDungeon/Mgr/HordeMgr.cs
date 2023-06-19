using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public class HordeMgr : UserComponent {

        List<SpawnPoint> spawnPoints;
        private bool hordeActive;
        public static int EnemiesActive { get; set; }
        public static int SpawnPointsActive { get; set; }
        private Vector2[] gatesToActiveOnHordeStart;
        private Vector2[] objectsToDisActiveOnHordeDefeated;
        private Vector2[] objectsToActiveOnHordeDefeated;
        private int hordeNumber;

        public HordeMgr(GameObject owner, Vector2[] gatesToActiveOnHordeStart,
            Vector2[] objectsToDisActiveOnHordeDefeated,
            Vector2[] objectsToActiveOnHordeDefeated, int hordeNumber) : base(owner) {
            spawnPoints = new List<SpawnPoint>();
            hordeActive = false;
            EnemiesActive = 0;
            SpawnPointsActive = 0;
            this.gatesToActiveOnHordeStart = gatesToActiveOnHordeStart;
            this.objectsToDisActiveOnHordeDefeated = objectsToDisActiveOnHordeDefeated;
            this.objectsToActiveOnHordeDefeated = objectsToActiveOnHordeDefeated;
            this.hordeNumber = hordeNumber;
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
                GameStatsMgr.HordesDefeated++;
                EventManager.CastEvent(EventList.HordeDefeated, EventArgsFactory.HordeDefeatedFactory());
                EventManager.CastEvent(EventList.LOG_EnemyHorde, EventArgsFactory.LOG_Factory("Orda sconfitta"));
                //disattivo oggetti dopo che l'orda è stata sconfitta (gates)
                foreach (Vector2 v in objectsToDisActiveOnHordeDefeated){
                    GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = false;
                    RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, false);
                }
                //attivo oggetti dopo che l'orda è stata sconfitta (memory card. hearth)
                foreach (Vector2 v in objectsToActiveOnHordeDefeated) {
                    GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = true;
                    RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, true);
                }
            }
        }

        public void CheckHordeActivation() {
            if (GameStatsMgr.ActiveWeapon == null || hordeActive || hordeNumber == GameStatsMgr.HordesDefeated) return;
            //attivo oggetti quando l'orda  parte (gates)
            foreach (Vector2 v in gatesToActiveOnHordeStart) {
                GameObject.Find("Object_" + v.X + "_" + v.Y).IsActive = true;
                RoomObjectsMgr.SetRoomObjectActiveness((int)v.X, (int)v.Y, true, true, MovementGrid.EGridTile.Wall);
            }
            //attivo l'orda (spawn point)
            foreach (SpawnPoint spawnPoint in spawnPoints) {
                spawnPoint.gameObject.IsActive = true;
            }
            hordeActive = true;
        }
    }
}
