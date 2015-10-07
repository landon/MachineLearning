using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IPolicy<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        TAction ChooseAction(TState s, List<TAction> actions, IActionValueFunction<TState, TAction> Q);
        double GetNewValue(TState oldState, double oldValue, TState newState, double reward, IActionValueFunction<TState, TAction> Q);
    }
}
