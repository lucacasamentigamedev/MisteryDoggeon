using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using MisteryDungeon.Scenes;
using OpenTK;
using System;
using System.Collections.Generic;
using static Aiv.SearchTree;

namespace MisteryDungeon.MysteryDungeon {
    public class PlayerController : UserComponent {

        //pathfinding
        private MovementGrid grid;
        private List<Vector2> path = new List<Vector2>();
        private AStarSearchProgress<MovementGrid.GridMovementState> searchProgress = null;
        private Result<MovementGrid.GridMovementState>? partial;

        //ref
        private Rigidbody rigidbody;
        private SheetAnimator animator;
        private HealthModule healthModule;
        private ShootModule shootModule;

        //working var
        private float moveSpeed;
        private bool isMoving;
        private string moveAction;
        private float currentDeathTimer;
        bool considerX;
        bool considerY;
        bool dead;
        Vector2 startingCell;
        Vector2 targetCell;
        Vector2 direction;

        public PlayerController(GameObject owner, MovementGrid grid,
            float moveSpeed, string moveAction, float currentDeathTimer) : base(owner) {
            this.grid = grid;
            this.moveSpeed = moveSpeed;
            this.moveAction = moveAction;
            this.currentDeathTimer = currentDeathTimer;
        }

        public override void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<SheetAnimator>();
            healthModule = GetComponent<HealthModule>();
            shootModule = GetComponent<ShootModule>();
        }

        public override void Update() {
            if (dead) {
                currentDeathTimer -= Game.DeltaTime;
                if (currentDeathTimer > 0) return;
                Game.TriggerChangeScene(new GameOverScene());
                return;
            }
            if (Input.GetUserButtonDown(moveAction) && !isMoving) PerformPathfinding();
            if (searchProgress != null && !isMoving) PerformStep();
            else PerformMovement();
        }

        public void PerformPathfinding() {
            startingCell = new Vector2(
                (int)Math.Ceiling(transform.Position.X / TiledMapMgr.TileUnitWidth) - 1,
                (int)Math.Ceiling(transform.Position.Y / TiledMapMgr.TileUnitHeight) - 1
            );
            targetCell = new Vector2(
                (int)Math.Ceiling(Game.Win.MousePosition.X / TiledMapMgr.TileUnitWidth) - 1,
                (int)Math.Ceiling(Game.Win.MousePosition.Y / TiledMapMgr.TileUnitHeight) - 1
            );
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Cella inizio = " + startingCell.ToString()));
            EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory("Cella fine = " + targetCell.ToString()));
            searchProgress = grid.FindPathProgressive(startingCell, targetCell);
        }

        public void PerformStep() {
            partial = searchProgress.Step(30);
            if (partial.HasValue) {
                path.Clear();
                foreach (var step in partial.Value.Steps) {
                    path.Add(new Vector2(
                        ((Game.Win.OrthoWidth * step.X) / TiledMapMgr.MapRows) + (TiledMapMgr.TileUnitWidth / 2),
                        ((Game.Win.OrthoHeight * step.Y) / TiledMapMgr.MapColumns) + (TiledMapMgr.TileUnitHeight / 2)
                    ));
                }
                searchProgress = null;
                EventManager.CastEvent(EventList.LOG_Pathfinding, EventArgsFactory.LOG_Factory(MovementGridMgr.PrintPathfindingPath(path)));
                if (path.Count <= 0) {
                    //click on wall or obstacle
                    EventManager.CastEvent(EventList.PathUnreachable, EventArgsFactory.PathUnreachableFactory());
                    StopMovement(Vector2.Zero);
                } else {
                    isMoving = true;
                };
            }
        }

        public void PerformMovement() {
            if (path.Count <= 0) return;
            direction = (path[0] - transform.Position);
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

        private void StopMovement(Vector2 direction) {
            SetCLip(GetCLipAnimationName(direction, false));
            rigidbody.Velocity = Vector2.Zero;
            isMoving = false;
        }

        public override void OnCollide(Collision collisionInfo) {
            switch(collisionInfo.Collider.gameObject.Tag) {
                case (int)GameObjectTag.Door:
                    Door door = collisionInfo.Collider.gameObject.GetComponent<Door>();
                    if (door.LockedBy >= 0 && !GameStatsMgr.CollectedKeys.Contains(door.LockedBy)) return;
                    GameStatsMgr.PreviousRoom = GameStatsMgr.ActualRoom;
                    GameStatsMgr.ActualRoom = door.RoomToGo;
                    EventManager.CastEvent(EventList.StartLoading, EventArgsFactory.StartLoadingFactory());
                    EventManager.CastEvent(EventList.RoomLeft, EventArgsFactory.RoomLeftFactory());
                    if (!GameStatsMgr.FirstDoorPassed) GameStatsMgr.FirstDoorPassed = true;
                    int roomId = collisionInfo.Collider.gameObject.GetComponent<Door>().RoomToGo;
                    Scene nextScene = (Scene)Activator.CreateInstance("MisteryDungeon", "MisteryDungeon.Room_" + roomId).Unwrap();
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
                    GameStatsMgr.ActiveWeapon = weapon;
                    ShootModule sm = GetComponent<ShootModule>();
                    sm.Enabled = true;
                    sm.SetWeapon( weapon.BulletType, weapon.ReloadTime, weapon.OffsetShoot );
                    if(!GameStatsMgr.PlayerCanShoot) GameStatsMgr.PlayerCanShoot = true;
                    switch(weapon.BulletType) {
                        case BulletType.Arrow:
                            GameStatsMgr.ActiveWeapon = weapon;
                            break;
                    }
                    weapon.gameObject.IsActive = false;
                    break;
                case (int)GameObjectTag.Key:
                    EventManager.CastEvent(EventList.ObjectPicked, EventArgsFactory.ObjectPickedFactory());
                    Key key = collisionInfo.Collider.gameObject.GetComponent<Key>();
                    GameStatsMgr.CollectedKeys.Add(key.ID);
                    key.gameObject.IsActive = false;
                    break;
                case (int)GameObjectTag.Enemy:
                    LittleBlobController enemy = collisionInfo.Collider.gameObject.GetComponent<LittleBlobController>();
                    if (enemy.Dead) break;
                    enemy.TakeDamage(enemy.GetComponent<HealthModule>().Health);
                    TakeDamage(enemy.Damage);
                    break;
                case (int)GameObjectTag.EnemyBullet:
                    Bullet bullet = collisionInfo.Collider.gameObject.GetComponent<Bullet>();
                    bullet.DestroyBullet();
                    TakeDamage(bullet.Damage);
                    break;
                case (int)GameObjectTag.Boss:
                    BossController boss = collisionInfo.Collider.gameObject.GetComponent<BossController>();
                    if (boss.Dead) break;
                    TakeDamage(healthModule.Health);
                    break;
                case (int)GameObjectTag.MemoryCard:
                    ClearPathFindingAndStopPlayer();
                    EventManager.CastEvent(EventList.StartLoading, EventArgsFactory.StartLoadingFactory());
                    EventManager.CastEvent(EventList.ObjectPicked, EventArgsFactory.ObjectPickedFactory());
                    EventManager.CastEvent(EventList.SaveGame, EventArgsFactory.SaveGameFactory());
                    collisionInfo.Collider.gameObject.IsActive = false;
                    break;
            }
        }

        private void ClearPathFindingAndStopPlayer() {
            SetCLip(GetCLipAnimationName(direction, false));
            rigidbody.Velocity = Vector2.Zero;
            isMoving = false;
            searchProgress = null;
            path.Clear();
        }

        public void TakeDamage(float damage) {
            if (dead) return;
            EventManager.CastEvent(EventList.PlayerTakesDamage, EventArgsFactory.PlayerTakesDamageFactory());
            if (healthModule.TakeDamage(damage)) {
                EventManager.CastEvent(EventList.PlayerDead, EventArgsFactory.PlayerDeadFactory());
                animator.ChangeClip("death");
                dead = true;
                shootModule.Enabled = false;
                rigidbody.Velocity = Vector2.Zero;
            } else {
                GameStatsMgr.PlayerHealth -= damage;
            }
        }

        public void SetCLip(string clipName) {
            if(animator.CurrentClip.AnimationName != clipName) animator.ChangeClip(clipName);
        }

        public string GetCLipAnimationName(Vector2 direction, bool walk) {
            if (direction == Vector2.Zero) return animator.CurrentClip.AnimationName;
            considerX = false;
            considerY = false;
            if(Math.Abs(direction.X) > Math.Abs(direction.Y)) considerX = true;
            else considerY = true;
            if (considerX && direction.X < 0) return walk ? "walkingLeft" : "idleLeft";
            else if (considerX && direction.X > 0) return walk ? "walkingRight" : "idleRight";
            else if (considerY && direction.Y > 0) return walk ? "walkingDown" : "idleDown";
            else return walk ? "walkingUp" : "idleUp";
        }

    }
}
