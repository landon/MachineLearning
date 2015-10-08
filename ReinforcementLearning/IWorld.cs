using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IWorld<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        TState CurrentState { get; }
        List<TAction> GetPossibleActions();
        TState PerformAction(TAction action, out double reward);
    }
}
