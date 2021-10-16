using System;
using System.Collections.Generic;
using System.Text;

namespace Pathfinding
{
    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();

        public void Push(T data)
        {
            // store the data ata the end of the heap
            _heap.Add(data);
            int now = _heap.Count - 1;
            while (now > 0)
            {
                int next = (now - 1) / 2;
                if (_heap[now].CompareTo(_heap[next]) < 0)       // compare with the parent node
                    break;
                // exchange two values
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }
        }

        public T Pop()
        {
            // store the data to be returned
            T ret = _heap[0];
            // move the last data to the root
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;
            // compare with the child node
            int now = 0;
            while (true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;
                int next = now;
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                if (next == now)
                    break;

                // exchange two values
                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                // move to next
                now = next;
            }

            return ret;
        }
        public int Count { get { return _heap.Count; } }

    }
}
