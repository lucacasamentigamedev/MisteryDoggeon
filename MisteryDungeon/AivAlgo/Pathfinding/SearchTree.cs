using System;
using System.Collections.Generic;
using Aiv.Collections.Generic;

namespace Aiv
{
    public struct SearchTreeAction<T> {
        public T NewState;
        public float Cost;
    }

    public interface ISearchTreeState<T> {
        List<SearchTreeAction<T>> GetActions();
        float ComputeHeuristic(T target);
    }

    public static class SearchTree {
        public class SearchNode<T> : IComparable<SearchNode<T>> where T : ISearchTreeState<T> {
            public SearchNode<T> Parent;

            public T State;
            public float Cost = 0;
            public float Heuritic = 0;

            public float Priority { get { return Cost + Heuritic; } }

            public SearchNode(T state, SearchNode<T> parent) {
                State = state;
                Parent = parent;
            }

            public SearchNode(T state, SearchNode<T> parent, float cost, float heuristic) {
                State = state;
                Parent = parent;
                Cost = cost;
                Heuritic = heuristic;
            }

            public int CompareTo(SearchNode<T> other) {
                return Math.Sign(Priority - other.Priority);
            }
        }

        public struct Result<T> where T : ISearchTreeState<T> {
            public SearchNode<T> Solution;
            public List<T> Steps;
            public int Iterations;
        }

        private static Result<T> MakeResult<T>(SearchNode<T> solution, int iterations = 0) where T : ISearchTreeState<T> {
            Result<T> result = new Result<T>();

            result.Steps = new List<T>();
            result.Solution = solution;
            result.Iterations = iterations;

            while (solution != null) {
                result.Steps.Add(solution.State);
                solution = solution.Parent;
            }

            result.Steps.Reverse();

            return result;
        }

        public static Result<T> BreadthFirstSearch<T>(T from, T to) where T : ISearchTreeState<T> {
            Queue<SearchNode<T>> openList = new Queue<SearchNode<T>>();
            List<T> closedList = new List<T>();

            int iterations = 0;
            openList.Enqueue(new SearchNode<T>(from, null));
            while (openList.Count > 0) {
                var current = openList.Dequeue();
                ++iterations;

                if (current.State.Equals(to)) return MakeResult(current, iterations);

                closedList.Add(current.State);

                foreach (var action in current.State.GetActions()) {
                    if (closedList.Contains(action.NewState)) continue;
                    openList.Enqueue(new SearchNode<T>(action.NewState, current));
                }
            }
            return MakeResult<T>(null);
        }

        public static Result<T> DijkstraSearch<T>(T from, T to) where T : ISearchTreeState<T> {
            PriorityQueue<SearchNode<T>> openList = new PriorityQueue<SearchNode<T>>();
            List<T> closedList = new List<T>();

            int iterations = 0;
            openList.Enqueue(new SearchNode<T>(from, null));
            while (openList.Count > 0) {
                var current = openList.Dequeue();
                ++iterations;

                if (closedList.Contains(current.State)) continue;
                closedList.Add(current.State);

                if (current.State.Equals(to)) return MakeResult(current, iterations);

                foreach (var action in current.State.GetActions()) {
                    openList.Enqueue(new SearchNode<T>(action.NewState, current, current.Cost + action.Cost, 0));
                }
            }
            return MakeResult<T>(null);
        }

        public static Result<T> GreedySearch<T>(T from, T to) where T : ISearchTreeState<T> {
            PriorityQueue<SearchNode<T>> openList = new PriorityQueue<SearchNode<T>>();
            List<T> closedList = new List<T>();

            int iterations = 0;
            openList.Enqueue(new SearchNode<T>(from, null));
            while (openList.Count > 0) {
                var current = openList.Dequeue();
                ++iterations;

                if (closedList.Contains(current.State)) continue;
                closedList.Add(current.State);

                if (current.State.Equals(to)) return MakeResult(current, iterations);

                foreach (var action in current.State.GetActions()) {
                    openList.Enqueue(new SearchNode<T>(action.NewState, current, 0, action.NewState.ComputeHeuristic(to)));
                }
            }
            return MakeResult<T>(null);
        }

        public class AStarSearchProgress<T> where T : ISearchTreeState<T> {
            PriorityQueue<SearchNode<T>> openList = new PriorityQueue<SearchNode<T>>();
            List<T> closedList = new List<T>();

            int iterations = 0;
            T to;

            public AStarSearchProgress(T from, T to) {
                this.to = to;
                openList.Enqueue(new SearchNode<T>(from, null));
            }

            public Result<T>? Step(int maxStep) {
                while (openList.Count > 0 && maxStep > 0) {
                    --maxStep;
                    var current = openList.Dequeue();
                    ++iterations;

                    if (closedList.Contains(current.State)) continue;
                    closedList.Add(current.State);

                    if (current.State.Equals(to)) return MakeResult(current, iterations);

                    foreach (var action in current.State.GetActions()) {
                        openList.Enqueue(new SearchNode<T>(action.NewState, current, current.Cost + action.Cost, action.NewState.ComputeHeuristic(to)));
                    }
                }
                if (maxStep == 0) return null;
                return MakeResult<T>(null);
            }
        }

        public static AStarSearchProgress<T> MakeAStarSerachProgress<T>(T from, T to) where T : ISearchTreeState<T> {
            return new AStarSearchProgress<T>(from, to);
        }

        public static Result<T> AStarSearch<T>(T from, T to) where T : ISearchTreeState<T> {
            var progress = MakeAStarSerachProgress<T>(from, to);
            Result<T>? result = null;
            for (result = progress.Step(100); result == null; result = progress.Step(100)) ;
            return result.Value;
        }
    }
}
