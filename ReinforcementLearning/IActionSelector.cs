using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IActionSelector<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        TAction ChooseAction(TState s, List<TAction> actions, IActionValueFunction<TState, TAction> Q);
    }
}
