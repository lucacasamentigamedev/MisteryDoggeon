﻿using Aiv;
using Aiv.Fast2D.Component;
using MisteryDungeon.AivAlgo.Pathfinding;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.MysteryDungeon {
    internal class PlayerController : UserComponent {

        private SearchTree.AStarSearchProgress<MovementGrid.GridMovementState> searchProgress = null;
        private MovementGrid grid;
        private List<Vector2> path = new List<Vector2>();
        private Rigidbody rigidbody;
        private float moveSpeed;
        private bool isMoving = false;
        private float tileUnitWidth;
        private float tileUnitHeight;
        private int mapTileColumns;
        private int mapTileRows;

        public PlayerController(GameObject owner, MovementGrid grid, float moveSpeed, float tileUnitWidth,
            float tileUnitHeight, int mapTileColumns, int mapTileRows) : base(owner) {
            this.grid = grid;
            this.moveSpeed = moveSpeed;
            this.tileUnitWidth = tileUnitWidth;
            this.tileUnitHeight = tileUnitHeight;
            this.mapTileColumns = mapTileColumns;
            this.mapTileRows = mapTileRows;
        }

        public override void Awake() {
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Update() {
            if(Input.GetUserButtonDown("Player_Move") && !isMoving) {
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
            PrintPath();

            //click on wall or obstacle
            if (path.Count <= 0) {
                //TODO: suono di percorso che non c'è
                StopMovement();
                return;
            };

            //convert map position into unit position
            Console.WriteLine("Cella inizio = " + startingCell.ToString());
            Console.WriteLine("Cella fine = " + path[path.Count-1].ToString());
            for (int i = 0; i < path.Count; i++) {
                path[i] = new Vector2(
                    ((Game.Win.OrthoWidth * path[i].X) / mapTileRows) + (tileUnitWidth / 2),
                    ((Game.Win.OrthoHeight * path[i].Y) / mapTileColumns) + (tileUnitHeight / 2)
                );
            }
            isMoving = true;
        }

        public void PrintPath() {
            if(path.Count > 0) {
                Console.Write("Percorso: ");
                foreach (var point in path) {
                    Console.Write("("+point.X+","+point.Y+") ");
                }
                Console.WriteLine();
            } else {
                Console.WriteLine("Nessun percorso disponibile");
            }
        }

        private void StopMovement() {
            rigidbody.Velocity = Vector2.Zero;
            isMoving = false;
        }
    }
}