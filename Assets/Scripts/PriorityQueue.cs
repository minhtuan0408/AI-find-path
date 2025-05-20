using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap = new List<(T, int)>();

    private int Compare(int a, int b)
    {
        return heap[a].priority.CompareTo(heap[b].priority);
    }

    private void Swap(int a, int b)
    {
        var temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
    }

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        int i = heap.Count - 1;
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (Compare(i, parent) >= 0) break;
            Swap(i, parent);
            i = parent;
        }
    }

    public T Dequeue()
    {
        if (heap.Count == 0) throw new InvalidOperationException("Queue is empty");
        T result = heap[0].item;
        heap[0] = heap[^1];
        heap.RemoveAt(heap.Count - 1);

        int i = 0;
        while (true)
        {
            int left = i * 2 + 1;
            int right = i * 2 + 2;
            int smallest = i;

            if (left < heap.Count && Compare(left, smallest) < 0) smallest = left;
            if (right < heap.Count && Compare(right, smallest) < 0) smallest = right;

            if (smallest == i) break;
            Swap(i, smallest);
            i = smallest;
        }

        return result;
    }

    public int Count => heap.Count;
}
