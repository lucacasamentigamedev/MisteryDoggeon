using System;
using System.Collections.Generic;

namespace Aiv
{
    public static class Algo
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T tmp = lhs;
            lhs = rhs;
            rhs = tmp;
        }

        public static int Clamp(int v, int min, int max)
        {
            return Math.Max(Math.Min(v, max), min);
        }

        public static float Clamp(float v, float min, float max)
        {
            return Math.Max(Math.Min(v, max), min);
        }

        public static double Clamp(double v, double min, double max)
        {
            return Math.Max(Math.Min(v, max), min);
        }

        public static int ManhattanDistance(int x0, int y0, int x1, int y1)
        {
            return Math.Abs(x0 - x1) + Math.Abs(y0 - y1);
        }

        public static float ManhattanDistance(float x0, float y0, float x1, float y1)
        {
            return Math.Abs(x0 - x1) + Math.Abs(y0 - y1);
        }

        public static double ManhattanDistance(double x0, double y0, double x1, double y1)
        {
            return Math.Abs(x0 - x1) + Math.Abs(y0 - y1);
        }

        public static void Reverse<T>(T[] vals, int start, int end)
        {
            for (--end; start < end; ++start, --end)
            {
                Swap(ref vals[start], ref vals[end]);
            }
        }

        public static void Rotate<T>(T[] vals, int start, int mid, int end)
        {
            Reverse(vals, start, mid);
            Reverse(vals, mid, end);
            Reverse(vals, start, end);
        }

        public static bool NextPermutation<T>(T[] vals)
        {
            return NextPermutation(vals, Comparer<T>.Default);
        }

        public static bool NextPermutation<T>(T[] vals, IComparer<T> comparer)
        {
            var next = vals.Length - 1;
            do
            {
                var next1 = next;
                if (comparer.Compare(vals[--next], vals[next1]) < 0)
                {
                    var mid = vals.Length;
                    do
                    {
                        --mid;
                    } while (comparer.Compare(vals[next], vals[mid]) >= 0);

                    Swap(ref vals[next], ref vals[mid]);
                    Reverse(vals, next1, vals.Length);
                    return true;
                }
            }
            while (next != 0);

            Reverse(vals, 0, vals.Length);
            return false;
        }

        public static void PushHeap<T>(T[] vals, int hole, int top = 0)
        {
            PushHeap(vals, hole, top, Comparer<T>.Default);
        }

        public static void PushHeap<T>(T[] vals, int hole, IComparer<T> comparer)
        {
            PushHeap(vals, hole, 0, comparer);
        }

        public static void PushHeap<T>(T[] vals, int hole, int top, IComparer<T> comparer)
        {
            if (hole > 0)
            {
                var tmp = vals[hole];
                for (int idx = (hole - 1) / 2; hole > top && comparer.Compare(tmp, vals[idx]) < 0; idx = (idx - 1) / 2)
                {
                    vals[hole] = vals[idx];
                    hole = idx;
                }
                vals[hole] = tmp;
            }
        }

        public static void PopHeap<T>(T[] vals, int hole)
        {
            PopHeap(vals, hole, vals.Length, Comparer<T>.Default);
        }

        public static void PopHeap<T>(T[] vals, int hole, IComparer<T> comparer)
        {
            PopHeap(vals, hole, vals.Length, comparer);
        }

        public static void PopHeap<T>(T[] vals, int hole, int bottom)
        {
            PopHeap(vals, hole, bottom, Comparer<T>.Default);
        }

        public static void PopHeap<T>(T[] vals, int hole, int bottom, IComparer<T> comparer)
        {
            if (bottom == 0) return;

            var top = hole;

            var tmp = vals[hole];

            var lastNonLeaf = (bottom - 1) / 2;
            for (int idx = 2 * hole + 2; hole < lastNonLeaf; idx = idx * 2 + 2)
            {
                if (comparer.Compare(vals[idx - 1], vals[idx]) < 0)
                {
                    --idx;
                }
                vals[hole] = vals[idx];
                hole = idx;
            }

            if (hole == lastNonLeaf && bottom % 2 == 0)
            {
                vals[hole] = vals[bottom - 1];
                hole = bottom - 1;
            }

            vals[hole] = tmp;
            PushHeap(vals, hole, top, comparer);
        }

        public static void MakeHeap<T>(T[] vals)
        {
            MakeHeap(vals, vals.Length, Comparer<T>.Default);
        }

        public static void MakeHeap<T>(T[] vals, int bottom, IComparer<T> comparer)
        {
            for (int hole = (bottom / 2) - 1; hole >= 0; --hole)
            {
                PopHeap(vals, hole, bottom, comparer);
            }
        }
    }
}
