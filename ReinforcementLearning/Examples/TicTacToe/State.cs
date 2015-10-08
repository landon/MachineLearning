using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    static partial class TicTacToe
    {
        public class State : IState
        {
            int[,] _square = new int[N, N];
            public List<Tuple<int, int>> EmptySquares { get; private set; }
            bool _isWon;
            bool _isDraw;

            public bool IsWon { get { return _isWon; } }
            public bool IsDraw { get { return _isDraw; } }

            public State()
            {
                EmptySquares = Enumerable.Range(0, N).SelectMany(i => Enumerable.Range(0, N).Select(j => new Tuple<int, int>(i, j))).ToList();
            }

            public void FillSquare(Tuple<int, int> sq, int player)
            {
                _square[sq.Item1, sq.Item2] = player;
                EmptySquares.Remove(sq);

                _isWon = CheckWon(player, sq);
                if (!_isWon && EmptySquares.Count <= 0)
                    _isDraw = true;
            }

            bool CheckWon(int player, Tuple<int, int> sq)
            {
                if (_isWon)
                    return true;
                if (Enumerable.Range(0, N).All(i => _square[sq.Item1, i] == player))
                    return true;
                if (Enumerable.Range(0, N).All(i => _square[i, sq.Item2] == player))
                    return true;
                if (sq.Item1 == sq.Item2 || sq.Item1 + sq.Item2 == N - 1)
                {
                    if (Enumerable.Range(0, N).All(i => _square[i, i] == player))
                        return true;
                    if (Enumerable.Range(0, N).All(i => _square[N - 1 - i, i] == player))
                        return true;
                }
                return false;
            }

            public override string ToString()
            {
                return string.Join(Environment.NewLine, Enumerable.Range(0, N).Select(i => string.Join("", Enumerable.Range(0, N).Select(j =>
                    {
                        if (_square[j, i] == 1)
                            return "X";
                        if (_square[j, i] ==- 1)
                            return "O";
                        return " ";
                    }))));
            }
        }
    }
}
