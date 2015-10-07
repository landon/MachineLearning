using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IStateMachine<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        TState InitialState { get; }
        List<TAction> GetPossibleActions();
        TState PerformAction(TAction action, out double reward);
    }
}
