using System;
using System.Collections.Generic;

namespace Aiv.Collections.Generic
{
    public class PriorityQueue<T>
    {
        private IComparer<T> comparer = Comparer<T>.Default;

        private T[] values;
        public int Count { get; private set; }
        public int Capacity { get { return values.Length; } }

        public PriorityQueue()
        {
            values = new T[4];
            Count = 0;
        }

        public PriorityQueue(IComparer<T> comparer) : this()
        {
            this.comparer = comparer;
        }

        public PriorityQueue(T[] values)
        {
            this.values = new T[values.Length];
            Array.Copy(values, this.values, values.Length);
            Count = values.Length;
            Algo.MakeHeap(values, Count, comparer);
        }

        public PriorityQueue(T[] values, IComparer<T> comparer) : this(values)
        {
            this.comparer = comparer;
        }

        public void Enqueue(T value)
        {
            if (Count == Capacity) Grow(Capacity * 2);
            values[Count] = value;
            Algo.PushHeap(values, Count++, comparer);
        }

        public T Dequeue()
        {
            Algo.Swap(ref values[--Count], ref values[0]);
            Algo.PopHeap(values, 0, Count, comparer);
            return values[Count];
        }

        public void Empty()
        {
            Count = 0;
        }

        private void Grow(int capacity)
        {
            var newValues = new T[capacity];
            Array.Copy(values, newValues, Capacity);
            values = newValues;
        }
    }
}
