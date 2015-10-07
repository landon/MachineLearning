using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RamseyGame
{
    class CompleteRamseyBoard : IBoard
    {
        static ThreadLocal<Dictionary<int, Dictionary<int, Tuple<int, int>>>> EdgeLookup = new ThreadLocal<Dictionary<int, Dictionary<int, Tuple<int, int>>>>(() => new Dictionary<int, Dictionary<int, Tuple<int, int>>>());
        static Tuple<int, int> LookupEdge(int n, int e)
        {
            Dictionary<int, Tuple<int, int>> d;
            if (!EdgeLookup.Value.TryGetValue(n, out d))
            {
                d = new Dictionary<int, Tuple<int, int>>();
                EdgeLookup.Value[n] = d;

                int k = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        d[k] = new Tuple<int, int>(i, j);
                        k++;
                    }
                }
            }

            return d[e];
        }

        Dictionary<int, Dictionary<int, List<int>>> _coloredNeighborLookup = new Dictionary<int, Dictionary<int, List<int>>>();
        Dictionary<int, List<int>> GetNeighbors(int v)
        {
            Dictionary<int, List<int>> d;
            if (!_coloredNeighborLookup.TryGetValue(v, out d))
            {
                d = new Dictionary<int, List<int>>();
                _coloredNeighborLookup[v] = d;
            }

            return d;
        }
        List<int> GetNeighborsOfColor(int v, int color)
        {
            var d = GetNeighbors(v);

            List<int> neighbors;
            if (!d.TryGetValue(color, out neighbors))
            {
                neighbors = new List<int>();
                d[color] = neighbors;
            }

            return neighbors;
        }

        int _n, _a, _b;
        public int[] State { get; private set; }
        public List<int> LegalMoves { get; private set; }

        MoveResult _lastMoveResult = MoveResult.Illegal;
        int _lastMoveEdge;
        int _lastMoveColor;
        int _movesPlayed;

        public CompleteRamseyBoard(int n, int a, int b)
        {
            _n = n;
            _a = a;
            _b = b;

            State = new int[n * (n - 1) / 2];
            LegalMoves = Enumerable.Range(0, State.Length).ToList();
        }

        public MoveResult MakeMove(int e, int color)
        {
            _movesPlayed++;
            _lastMoveEdge = e;
            _lastMoveColor = color;

            if (!LegalMoves.Contains(e))
            {
                _lastMoveResult = MoveResult.Illegal;
            }
            else
            {
                State[e] = color;
                LegalMoves.Remove(e);

                var edge = LookupEdge(_n, e);
                var n1 = GetNeighborsOfColor(edge.Item1, color);
                var n2 = GetNeighborsOfColor(edge.Item2, color);

                n1.Add(edge.Item2);
                n2.Add(edge.Item1);
                _lastMoveResult = MoveResult.OK;
            }

            return _lastMoveResult;
        }

        public bool IsWon()
        {
            if (_lastMoveResult == MoveResult.Illegal)
                return false;

            var winSize = _movesPlayed % 2 == 1 ? _a : _b;
            var edge = LookupEdge(_n, _lastMoveEdge);
            var n1 = GetNeighborsOfColor(edge.Item1, _lastMoveColor);
            var n2 = GetNeighborsOfColor(edge.Item2, _lastMoveColor);
            var common = ListUtility<int>.Intersection(n1, n2);

            return IsCliqueOfSize(common, winSize - 2, _lastMoveColor);
        }

        bool IsCliqueOfSize(List<int> vertices, int size, int color)
        {
            if (vertices.Count < size)
                return false;
            if (size <= 1)
                return true;

            for (int i = 0; i < vertices.Count; i++)
            {
                var neighbors = ListUtility<int>.Intersection(vertices.Skip(i + 1), GetNeighborsOfColor(vertices[i], color));
                if (IsCliqueOfSize(neighbors, size - 1, color))
                    return true;
            }

            return false;
        }

        public bool MovesExist()
        {
            return LegalMoves.Count > 0;
        }

        public override string ToString()
        {
            return string.Join(" ", State);
        }
    }
}
