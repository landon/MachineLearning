using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RamseyGame
{
    interface IBoard
    {
        MoveResult MakeMove(int move, int color);
        bool IsWon();
        bool MovesExist();
        List<int> LegalMoves { get; }
        int[] State { get; }
    }
}
