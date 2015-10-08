using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    static partial class TicTacToe
    {
        public class Action : IAction
        {
            public Tuple<int, int> Square { get; private set; }
            public Action(Tuple<int, int> sq)
            {
                Square = sq;
            }
        }
    }
}
