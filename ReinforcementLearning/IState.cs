using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IState<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        TState PerformAction(TAction a);
        bool IsTerminal { get; }
        double TerminalValue { get; }
        double[] AsDoubles();
    }
}
