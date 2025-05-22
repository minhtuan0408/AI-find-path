using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> elements = new List<(T, int)>();

    public void Enqueue(T item, int priority)
    {
        elements.Add((item, priority));
        elements = elements.OrderBy(e => e.priority).ToList(); // slow but simple
    }

    public T Dequeue()
    {
        var item = elements[0];
        elements.RemoveAt(0);
        return item.item;
    }

    public int Count => elements.Count;
}
