using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RamseyGame
{
    class RandomPlayer : IPlayer
    {
        Random RNG = new Random(DateTime.Now.Millisecond);
        int _color;

        public RandomPlayer(int color)
        {
            _color = color;
        }

        public void Move(IBoard board)
        {
            var mi = RNG.Next(0, board.LegalMoves.Count);
            board.MakeMove(board.LegalMoves[mi], _color);
        }
    }
}
