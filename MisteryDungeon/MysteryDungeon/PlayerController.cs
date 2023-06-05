using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    internal class PlayerController : UserComponent {

        //pathfinding
        private MovementGrid grid;
        private List<Vector2> path = new List<Vector2>();

        //ref
        private Rigidbody rigidbody;

        //working var
        private float moveSpeed;
        private bool isMoving;
        private float tileUnitWidth;
        private float tileUnitHeight;
        private int mapColumns;
        private int mapRows;

        public PlayerController(GameObject owner, MovementGrid grid, float moveSpeed) : base(owner) {
            this.grid = grid;
            this.moveSpeed = moveSpeed;
            tileUnitWidth = GameConfigMgr.TileUnitWidth;
            tileUnitHeight = GameConfigMgr.TileUnitHeight;
            mapColumns = GameConfigMgr.MapColumns;
            mapRows = GameConfigMgr.MapRows;
            isMoving = false;
        }

        public override void Awake() {
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Update() {
            if(Input.GetUserButtonDown("Move") && !isMoving) {
                PerformPathfinding();
            }
            if(isMoving) {
                Vector2 direction = (path[0] - transform.Position);
                if (path.Count == 1) {
                    rigidbody.Velocity = direction.Normalized() * moveSpeed;
                } else if (path.Count > 1) {
                    if ((path[0] - transform.Position).Length < 0.1f) {
                        path.RemoveAt(0);
                    }
                    rigidbody.Velocity = (path[0] - transform.Position).Normalized() * moveSpeed;
                } else {
                    StopMovement();
                }
                if ((path[0] - transform.Position).Length <= 0.1f) {
                    StopMovement();
                }
            }
        }

        public void PerformPathfinding() {
            //starting cell
            Vector2 startingCell = new Vector2(
                (int)Math.Ceiling(transform.Position.X / tileUnitWidth) - 1,
                (int)Math.Ceiling(transform.Position.Y / tileUnitHeight) - 1
            );

            //ending cell
            Vector2 targetCell = new Vector2(
                (int)Math.Ceiling(Game.Win.MousePosition.X / tileUnitWidth) - 1,
                (int)Math.Ceiling(Game.Win.MousePosition.Y / tileUnitHeight) - 1
            );

            path = grid.FindPath(startingCell, targetCell);
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(PrintPath()));

            //click on wall or obstacle
            if (path.Count <= 0) {
                //TODO: suono di percorso che non c'è
                StopMovement();
                return;
            };

            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Cella inizio = " + startingCell.ToString()));
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Cella fine = " + path[path.Count - 1].ToString()));
            //convert map position into unit position
            for (int i = 0; i < path.Count; i++) {
                path[i] = new Vector2(
                    ((Game.Win.OrthoWidth * path[i].X) / mapRows) + (tileUnitWidth / 2),
                    ((Game.Win.OrthoHeight * path[i].Y) / mapColumns) + (tileUnitHeight / 2)
                );
            }
            isMoving = true;
        }

        public string PrintPath() {
            string final = "";
            if(path.Count > 0) {
                final += "Percorso: ";
                foreach (var point in path) {
                    final += "(" + point.X + "," + point.Y + ") ";
                }
                final += "\n";
            } else {
                final += "Nessun percorso disponibile\n";
            }
            return final;
        }

        private void StopMovement() {
            rigidbody.Velocity = Vector2.Zero;
            isMoving = false;
        }

        public override void OnCollide(Collision collisionInfo) {
            switch(collisionInfo.Collider.gameObject.Tag) {
                case (int)GameObjectTag.Door:
                    if (!GameConfigMgr.FirstDoorPassed) GameConfigMgr.FirstDoorPassed = true;
                    int roomId = collisionInfo.Collider.gameObject.GetComponent<Door>().RoomToGo;
                    Scene nextScene = (Scene)Activator.CreateInstance("MisteryDungeon", "MisteryDungeon.Room_" + roomId).Unwrap();
                    Game.SetLoadingScene();
                    Game.TriggerChangeScene(nextScene);
                    break;
                case (int)GameObjectTag.PlatformButton:
                    int seqId = collisionInfo.Collider.gameObject.GetComponent<PlatformButton>().SequenceId;
                    EventManager.CastEvent(EventList.ButtonPressed, EventArgsFactory.ButtonPressedFactory(seqId));
                    break;
                case (int)GameObjectTag.Weapon:
                    Weapon weapon = collisionInfo.Collider.gameObject.GetComponent<Weapon>();
                    GameStats.ActiveWeapon = weapon;
                    ShootModule sm = GetComponent<ShootModule>();
                    sm.Enabled = true;
                    sm.SetWeapon( weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot );
                    if(!GameStats.CanShoot) GameStats.CanShoot = true;
                    switch(weapon.BulletType) {
                        case BulletType.Arrow:
                            GameStats.BowPicked = true;
                            break;
                    }
                    weapon.gameObject.IsActive = false;
                    break;
            }
        }
    }
}
