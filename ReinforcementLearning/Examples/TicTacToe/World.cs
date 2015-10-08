using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Examples
{
    static partial class TicTacToe
    {
        public class World : IWorld<State, Action>
        {
            public State CurrentState { get; private set; }
            int Player { get; set; }

            public World()
            {
                CurrentState = new State();
                Player = 1;
            }

            public List<Action> GetPossibleActions()
            {
                return CurrentState.EmptySquares.Select(sq => new Action(sq)).ToList();
            }

            public State PerformAction(Action action, out double reward)
            {
                CurrentState.FillSquare(action.Square, Player);

                if (CurrentState.IsWon)
                    reward = 1.0;
                else if (CurrentState.IsDraw)
                    reward = 0.5;
                else
                    reward = 0.0;

                Player = -Player;
                return CurrentState;
            }
        }
    }
}
