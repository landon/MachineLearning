using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IUpdateRule<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        double GetNewValue(TState oldState, double oldValue, TState newState, double reward, IActionValueFunction<TState, TAction> Q);
    }
}
