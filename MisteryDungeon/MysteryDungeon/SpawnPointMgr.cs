using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public class SpawnPointMgr : UserComponent {

        List<SpawnPoint> spawnPoints;

        public SpawnPointMgr(GameObject owner) : base(owner) {
            spawnPoints = new List<SpawnPoint>();
        }

        public void AddSpawnPoint(SpawnPoint spawnPoint) {
            spawnPoints.Add(spawnPoint);
        }

        public override void Update() {
            CheckHordeActivation();
        }


        public void CheckHordeActivation() {
            if (!GameStats.HordeDefeated && GameStats.ActiveWeapon != null) {
                //attivo gate
                GameObject.Find("Object_2_39").IsActive = true;
                RoomObjectsMgr.SetRoomObjectActiveness(2, 39, true, true, MovementGrid.EGridTile.Wall);
                //attivo spawn point
                foreach (SpawnPoint spawnPoint in spawnPoints) {
                    spawnPoint.gameObject.IsActive = true;
                }
            }
        }
    }
}
