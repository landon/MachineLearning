using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    static partial class Ramsey
    {
        public class World : IWorld<State, Action>
        {
            int _agent;

            public State CurrentState { get; private set; }

            public World()
            {
                CurrentState = new State();
                _agent = 1;
            }

            public List<Action> GetPossibleActions()
            {
                return CurrentState.LegalMoves.Select(e => new Action(e, _agent)).ToList();
            }

            public State PerformAction(Action action, out double reward)
            {
                reward = 0;
                action.Agent = _agent;
                CurrentState = CurrentState.PerformAction(action);

                _agent = -_agent;
                return CurrentState;
            }
        }
    }
}
