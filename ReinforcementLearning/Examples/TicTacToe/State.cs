using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    static partial class TicTacToe
    {
        public class State : IState<State, Action>
        {
            int[,] _square = new int[N, N];
            public List<Tuple<int, int>> EmptySquares { get; private set; }
            
            bool _isWon;
            bool _isDraw;

            public bool IsWon { get { return _isWon; } }
            public bool IsDraw { get { return _isDraw; } }
            public bool IsTerminal { get { return _isDraw || _isWon; } }
            public double TerminalValue { get; private set; }

            public State()
            {
                EmptySquares = Enumerable.Range(0, N).SelectMany(i => Enumerable.Range(0, N).Select(j => new Tuple<int, int>(i, j))).ToList();
            }

            public State(int[,] square, List<Tuple<int, int>> emptySquares)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        _square[i, j] = square[i, j];
                    }
                }

                EmptySquares = emptySquares.ToList();
            }

            public State PerformAction(Action a)
            {
                var state = CloneState();
                state._square[a.Square.Item1, a.Square.Item2] = a.Agent;
                state.EmptySquares.Remove(a.Square);
                CheckTerminalStates(state, a.Agent, a.Square);

                if (state.IsTerminal)
                    state.TerminalValue = state.IsWon ? 1.0 : 0.0;

                return state;
            }

            static void CheckTerminalStates(State state, int player, Tuple<int, int> sq)
            {
                state._isWon = CheckWon(state, player, sq);
                if (!state._isWon)
                {
                    if (state.EmptySquares.Count <= 0)
                        state._isDraw = true;
                }
            }

            static bool DoesOtherPlayerWin(State state, int player, Tuple<int, int> sq)
            {
                state._square[sq.Item1, sq.Item2] = -player;
                var otherWins = CheckWon(state, -player, sq);
                state._square[sq.Item1, sq.Item2] = 0;

                return otherWins;
            }

            static bool CheckWon(State state, int player, Tuple<int, int> sq)
            {
                if (Enumerable.Range(0, N).All(i => state._square[sq.Item1, i] == player))
                    return true;
                if (Enumerable.Range(0, N).All(i => state._square[i, sq.Item2] == player))
                    return true;
                if (sq.Item1 == sq.Item2 || sq.Item1 + sq.Item2 == N - 1)
                {
                    if (Enumerable.Range(0, N).All(i => state._square[i, i] == player))
                        return true;
                    if (Enumerable.Range(0, N).All(i => state._square[N - 1 - i, i] == player))
                        return true;
                }

                return false;
            }

            State CloneState()
            {
                return new State(_square, EmptySquares);
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, Enumerable.Range(0, N).Select(i => string.Join("", Enumerable.Range(0, N).Select(j =>
                    {
                        if (_square[j, i] == 1)
                            return "X";
                        if (_square[j, i] ==- 1)
                            return "O";

                        return "-";
                    }))));
            }
        }
    }
}
