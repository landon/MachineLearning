using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IAgent<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        TState Act(IWorld<TState, TAction> world);
    }
}
