using Aiv;
using Aiv.Tiled;
using OpenTK;
using System;
using System.Collections.Generic;

namespace MisteryDungeon.AivAlgo.Pathfinding {
    public class MovementGrid {
        public enum EGridTile {
            Floor = 1,
            Obstacle = -1
        }

        public class GridMovementState : Aiv.ISearchTreeState<GridMovementState> {
            public int X;
            public int Y;

            public EGridTile[,] map;

            public float ComputeHeuristic(GridMovementState target) {
                return Aiv.Algo.ManhattanDistance(X, Y, target.X, target.Y);
            }

            public bool Equals(GridMovementState state) {
                return X == state.X && Y == state.Y;
            }

            public List<Aiv.SearchTreeAction<GridMovementState>> GetActions() {
                List<Aiv.SearchTreeAction<GridMovementState>> actions = new List<Aiv.SearchTreeAction<GridMovementState>>();

                if (X > 0 && map[X - 1, Y] != EGridTile.Obstacle) actions.Add(new Aiv.SearchTreeAction<GridMovementState> { Cost = (float)map[X - 1, Y], NewState = new GridMovementState { X = X - 1, Y = Y, map = map } });
                if (X < map.GetLength(0) - 1 && map[X + 1, Y] != EGridTile.Obstacle) actions.Add(new Aiv.SearchTreeAction<GridMovementState> { Cost = (float)map[X + 1, Y], NewState = new GridMovementState { X = X + 1, Y = Y, map = map } });
                if (Y > 0 && map[X, Y - 1] != EGridTile.Obstacle) actions.Add(new Aiv.SearchTreeAction<GridMovementState> { Cost = (float)map[X, Y - 1], NewState = new GridMovementState { X = X, Y = Y - 1, map = map } });
                if (Y < map.GetLength(1) - 1 && map[X, Y + 1] != EGridTile.Obstacle) actions.Add(new Aiv.SearchTreeAction<GridMovementState> { Cost = (float)map[X, Y + 1], NewState = new GridMovementState { X = X, Y = Y + 1, map = map } });

                return actions;
            }

            public override bool Equals(object obj) {
                return obj is GridMovementState state && Equals(state);
            }

            public override int GetHashCode() {
                int hashCode = 1861411795;
                hashCode = hashCode * -1521134295 + X.GetHashCode();
                hashCode = hashCode * -1521134295 + Y.GetHashCode();
                return hashCode;
            }
        }

        public EGridTile[,] Map;

        public MovementGrid(int width, int height) {
            System.Random rnd = new System.Random();
            Map = new EGridTile[width, height];
            for (int x = 0; x < Map.GetLength(0); ++x) {
                for (int y = 0; y < Map.GetLength(1); ++y) {
                    Map[x, y] = rnd.NextDouble() < 0.8 ? EGridTile.Floor : EGridTile.Obstacle;
                }
            }
        }
        
        public MovementGrid(int width, int height, Layer collisionLayer) {
            Map = new EGridTile[width, height];
            for (int x = 0; x < Map.GetLength(0); ++x) {
                for (int y = 0; y < Map.GetLength(1); ++y) {
                    Map[x, y] = collisionLayer.Tiles[x, y].Gid != 0 ? EGridTile.Obstacle : EGridTile.Floor;
                }
            }
        }

        public SearchTree.AStarSearchProgress<GridMovementState> FindPathProgressive(Vector2 from, Vector2 to) {
            return SearchTree.MakeAStarSerachProgress(
                new GridMovementState { X = (int)(from.X), Y = (int)(from.Y), map = Map },
                new GridMovementState { X = (int)(to.X), Y = (int)(to.Y), map = Map });
        }

        public List<Vector2> FindPath(Vector2 from, Vector2 to) {
            List<Vector2> result = new List<Vector2>();

            var path = SearchTree.AStarSearch(
                new GridMovementState { X = (int)(from.X), Y = (int)(from.Y), map = Map },
                new GridMovementState { X = (int)(to.X), Y = (int)(to.Y), map = Map });

            foreach (var step in path.Steps) {
                result.Add(new Vector2(step.X, step.Y));
            }

            return result;
        }
    }
}
