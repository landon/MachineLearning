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
            int Agent { get; set; }

            public World()
            {
                CurrentState = new State();
                Agent = 1;
            }

            public List<Action> GetPossibleActions()
            {
                return CurrentState.EmptySquares.Select(sq => new Action(sq) { Agent = this.Agent }).ToList();
            }

            public State PerformAction(Action action, out double reward)
            {
                action.Agent = Agent;
                CurrentState = CurrentState.PerformAction(action);

                if (CurrentState.IsWon)
                    reward = 1.0;
                else if (CurrentState.IsDraw)
                    reward = 0.0;
                else
                    reward = 0.0;

                Agent = -Agent;
                return CurrentState;
            }
        }
    }
}
