using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    static partial class TicTacToe
    {
        public class Action : IAction<Action>
        {
            public Tuple<int, int> Square { get; private set; }
            public int Agent { get; set; }

            public Action(Tuple<int, int> sq)
            {
                Square = sq;
            }

            public override string ToString()
            {
                return string.Format("{0},{1}", Square.Item1, Square.Item2);
            }
        }
    }
}
