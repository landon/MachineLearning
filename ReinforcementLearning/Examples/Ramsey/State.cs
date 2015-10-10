﻿using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples
{
    static partial class Ramsey
    {
        public class State : IState<State, Action>
        {
            static ThreadLocal<Dictionary<int, Dictionary<int, Tuple<int, int>>>> EdgeLookup = new ThreadLocal<Dictionary<int, Dictionary<int, Tuple<int, int>>>>(() => new Dictionary<int, Dictionary<int, Tuple<int, int>>>());
            Dictionary<int, Dictionary<int, List<int>>> _coloredNeighborLookup = new Dictionary<int, Dictionary<int, List<int>>>();

            int[] _edgeColor;
            public List<int> LegalMoves { get; private set; }

            public bool IsWon { get; private set; }
            public bool IsDraw { get; private set; }
            public bool IsTerminal { get { return IsWon || IsDraw; } }
            public double TerminalValue { get; private set; }

            public State()
            {
                _edgeColor = new int[N * (N - 1) / 2];
                LegalMoves = Enumerable.Range(0, _edgeColor.Length).ToList();
            }

            public State(int[] edgeColor, List<int> legalMoves, Dictionary<int, Dictionary<int, List<int>>> coloredNeighborLookup)
            {
                _edgeColor = edgeColor.ToArray();
                LegalMoves = legalMoves.ToList();

                _coloredNeighborLookup = new Dictionary<int, Dictionary<int, List<int>>>();
                foreach (var kvp in coloredNeighborLookup)
                {
                    var d = new Dictionary<int, List<int>>();
                    foreach (var kvp2 in kvp.Value)
                        d[kvp2.Key] = kvp2.Value.ToList();

                    _coloredNeighborLookup[kvp.Key] = d;
                }
            }

            public State PerformAction(Action a)
            {
                var s = CloneState();
                s._edgeColor[a.E] = a.Agent;
                s.LegalMoves.Remove(a.E);

                var edge = LookupEdge(N, a.E);
                var n1 = s.GetNeighborsOfColor(edge.Item1, a.Agent);
                var n2 = s.GetNeighborsOfColor(edge.Item2, a.Agent);

                n1.Add(edge.Item2);
                n2.Add(edge.Item1);

                s.IsWon = IsStateWon(s, a);
                if (s.IsWon)
                {
                    s.TerminalValue = 1.0;
                }
                else if (s.LegalMoves.Count <= 0)
                {
                    s.IsDraw = true;
                    s.TerminalValue = 0.0;
                }

                return s;
            }

            static bool IsStateWon(State s, Action lastAction)
            {
                var winSize = lastAction.Agent == 1 ? A : B;
                var edge = LookupEdge(N, lastAction.E);
                var n1 = s.GetNeighborsOfColor(edge.Item1, lastAction.Agent);
                var n2 = s.GetNeighborsOfColor(edge.Item2, lastAction.Agent);
                var common = ListUtility<int>.Intersection(n1, n2);

                return s.IsCliqueOfSize(common, winSize - 2, lastAction.Agent);
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

            State CloneState()
            {
                return new State(_edgeColor, LegalMoves, _coloredNeighborLookup);
            }

            public override string ToString()
            {
                return string.Join(" ", _edgeColor);
            }

            public double[] AsDoubles()
            {
                return _edgeColor.Select(x => (double)x).ToArray();
            }

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
        }
    }
}
