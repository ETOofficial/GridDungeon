using System;
using System.Collections.Generic;

public class AStarPathfinding
{
    public static List<Tuple<int, int>> AStar(int[][] grid, Tuple<int, int> start, Tuple<int, int> end)
    {
        int rows = grid.Length;
        if (rows == 0) return new List<Tuple<int, int>>();
        int cols = grid[0].Length;

        // 方向数组：上，下，左，右
        var directions = new Tuple<int, int>[4]
        {
            Tuple.Create(-1, 0),
            Tuple.Create(1, 0),
            Tuple.Create(0, -1),
            Tuple.Create(0, 1)
        };

        // 初始化优先队列（按f值排序）
        var openHeap = new PriorityQueue<Tuple<int, int>, int>();
        var gScore = new Dictionary<Tuple<int, int>, int>();
        var fScore = new Dictionary<Tuple<int, int>, int>();
        var cameFrom = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
        var closed = new HashSet<Tuple<int, int>>();

        // 初始化起点的分数
        gScore[start] = 0;
        fScore[start] = Heuristic(start, end);
        openHeap.Enqueue(start, fScore[start]);

        while (openHeap.Count > 0)
        {
            var current = openHeap.Dequeue();
            if (current.Equals(end))
            {
                return ReconstructPath(cameFrom, current);
            }

            if (closed.Contains(current)) continue;
            closed.Add(current);

            foreach (var dir in directions)
            {
                var neighbor = Tuple.Create(current.Item1 + dir.Item1, current.Item2 + dir.Item2);

                if (!IsValidCoordinate(neighbor, rows, cols)) continue;
                if (grid[neighbor.Item1][neighbor.Item2] == 1) continue;

                int tentativeG = gScore.GetValueOrDefault(current, int.MaxValue) + 1;
                if (tentativeG < gScore.GetValueOrDefault(neighbor, int.MaxValue))
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, end);
                    openHeap.Enqueue(neighbor, fScore[neighbor]);
                }
            }
        }
        return new List<Tuple<int, int>>();
    }

    private static int Heuristic(Tuple<int, int> a, Tuple<int, int> b)
    {
        return Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }

    private static List<Tuple<int, int>> ReconstructPath(
        Dictionary<Tuple<int, int>, Tuple<int, int>> cameFrom, 
        Tuple<int, int> current)
    {
        var path = new List<Tuple<int, int>> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private static bool IsValidCoordinate(Tuple<int, int> point, int rows, int cols)
    {
        return point.Item1 >= 0 && point.Item1 < rows && 
               point.Item2 >= 0 && point.Item2 < cols;
    }


}

// 需要.NET 6+ 的PriorityQueue实现
public class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
{
    private readonly List<(TElement Element, TPriority Priority)> _elements = new();

    public int Count => _elements.Count;

    public void Enqueue(TElement element, TPriority priority)
    {
        _elements.Add((element, priority));
        int ci = _elements.Count - 1;
        while (ci > 0)
        {
            int pi = (ci - 1) / 2;
            if (_elements[ci].Priority.CompareTo(_elements[pi].Priority) >= 0) break;
            (_elements[ci], _elements[pi]) = (_elements[pi], _elements[ci]);
            ci = pi;
        }
    }

    public TElement Dequeue()
    {
        var last = _elements.Count - 1;
        var front = _elements[0];
        _elements[0] = _elements[last];
        _elements.RemoveAt(last);

        int pi = 0;
        while (true)
        {
            int ci = pi * 2 + 1;
            if (ci >= _elements.Count) break;
            int rc = ci + 1;
            if (rc < _elements.Count && 
                _elements[rc].Priority.CompareTo(_elements[ci].Priority) < 0)
                ci = rc;
            if (_elements[pi].Priority.CompareTo(_elements[ci].Priority) <= 0) break;
            (_elements[pi], _elements[ci]) = (_elements[ci], _elements[pi]);
            pi = ci;
        }
        return front.Element;
    }
}
