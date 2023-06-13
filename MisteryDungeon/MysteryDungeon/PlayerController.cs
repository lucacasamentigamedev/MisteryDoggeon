using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    public class PlayerController : UserComponent {

        //pathfinding
        private MovementGrid grid;
        private List<Vector2> path = new List<Vector2>();

        //ref
        private Rigidbody rigidbody;
        private SheetAnimator animator;

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
            tileUnitWidth = TiledMapMgr.TileUnitWidth;
            tileUnitHeight = TiledMapMgr.TileUnitHeight;
            mapColumns = TiledMapMgr.MapColumns;
            mapRows = TiledMapMgr.MapRows;
            isMoving = false;
        }

        public override void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<SheetAnimator>();
        }

        public override void Update() {
            if(Input.GetUserButtonDown("Move") && !isMoving) {
                PerformPathfinding();
            }
            if(isMoving) {
                Vector2 direction = (path[0] - transform.Position);
                SetCLip(GetCLipAnimationName(rigidbody.Velocity, true));
                if (path.Count == 1) {
                    rigidbody.Velocity = direction.Normalized() * moveSpeed;
                } else if (path.Count > 1) {
                    if ((path[0] - transform.Position).Length < 0.1f) {
                        path.RemoveAt(0);
                    }
                    rigidbody.Velocity = (path[0] - transform.Position).Normalized() * moveSpeed;
                } else {
                    StopMovement(rigidbody.Velocity);
                }
                if ((path[0] - transform.Position).Length <= 0.1f) {
                    StopMovement(rigidbody.Velocity);
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
                EventManager.CastEvent(EventList.PathUnreachable, EventArgsFactory.PathUnreachableFactory());
                StopMovement(Vector2.Zero);
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

        private void StopMovement(Vector2 direction) {
            SetCLip(GetCLipAnimationName(direction, false));
            rigidbody.Velocity = Vector2.Zero;
            isMoving = false;
        }

        public override void OnCollide(Collision collisionInfo) {
            switch(collisionInfo.Collider.gameObject.Tag) {
                case (int)GameObjectTag.Door:
                    EventManager.CastEvent(EventList.RoomLeft, EventArgsFactory.RoomLeftFactory());
                    Door door = collisionInfo.Collider.gameObject.GetComponent<Door>();
                    if (door.LockedBy >= 0 && !GameStats.collectedKeys.Contains(door.LockedBy)) return;
                    if (!GameStats.FirstDoorPassed) GameStats.FirstDoorPassed = true;
                    int roomId = collisionInfo.Collider.gameObject.GetComponent<Door>().RoomToGo;
                    Scene nextScene = (Scene)Activator.CreateInstance("MisteryDungeon", "MisteryDungeon.Room_" + roomId).Unwrap();
                    Game.SetLoadingScene();
                    Game.TriggerChangeScene(nextScene);
                    break;
                case (int)GameObjectTag.PlatformButton:
                    EventManager.CastEvent(EventList.PlatformButtonPressed, EventArgsFactory.
                        PlatformButtonPressedFactory(collisionInfo.Collider.gameObject.GetComponent<PlatformButton>()));
                    break;
                case (int)GameObjectTag.Weapon:
                    EventManager.CastEvent(EventList.ObjectPicked, EventArgsFactory.
                        ObjectPickedFactory());
                    Weapon weapon = collisionInfo.Collider.gameObject.GetComponent<Weapon>();
                    GameStats.ActiveWeapon = weapon;
                    ShootModule sm = GetComponent<ShootModule>();
                    sm.Enabled = true;
                    sm.SetWeapon( weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot );
                    if(!GameStats.PlayerCanShoot) GameStats.PlayerCanShoot = true;
                    switch(weapon.BulletType) {
                        case BulletType.Arrow:
                            GameStats.ActiveWeapon = weapon;
                            break;
                    }
                    weapon.gameObject.IsActive = false;
                    break;
                case (int)GameObjectTag.Key:
                    EventManager.CastEvent(EventList.ObjectPicked, EventArgsFactory.
                        ObjectPickedFactory());
                    Key key = collisionInfo.Collider.gameObject.GetComponent<Key>();
                    GameStats.collectedKeys.Add(key.ID);
                    key.gameObject.IsActive = false;
                    break;
                case (int)GameObjectTag.Enemy:
                    //TODO: suono danno al player
                    Enemy enemy = collisionInfo.Collider.gameObject.GetComponent<Enemy>();
                    enemy.DestroyEnemy();
                    TakeDamage(enemy.Damage);
                    break;
                case (int)GameObjectTag.EnemyBullet:
                    //TODO: suono danno al player
                    Bullet bullet = collisionInfo.Collider.gameObject.GetComponent<Bullet>();
                    bullet.DestroyBullet();
                    TakeDamage(bullet.Damage);
                    break;
            }
        }

        public void TakeDamage(float damage) {
            if (GetComponent<HealthModule>().TakeDamage(damage)) {
                //Finisce il gioco
                //TODO: schermata di fine gioco
            } else {
                GameStats.PlayerHealth -= damage;
            }
        }

        public void SetCLip(string clipName) {
            if(animator.CurrentClip.AnimationName != clipName) animator.ChangeClip(clipName);
        }

        public string GetCLipAnimationName(Vector2 direction, bool walk) {
            if (direction == Vector2.Zero) return animator.CurrentClip.AnimationName;
            bool considerX = false;
            bool considerY = false;
            if(Math.Abs(direction.X) > Math.Abs(direction.Y)) considerX = true;
            else considerY = true;
            if (considerX && direction.X < 0) return walk ? "walkingLeft" : "idleLeft";
            else if (considerX && direction.X > 0) return walk ? "walkingRight" : "idleRight";
            else if (considerY && direction.Y > 0) return walk ? "walkingDown" : "idleDown";
            else return walk ? "walkingUp" : "idleUp";
        }
    }
}
