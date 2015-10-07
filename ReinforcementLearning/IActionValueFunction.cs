using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IActionValueFunction<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        double Evaluate(TState s, TAction a);
        void Update(TState s, TAction a, double value);
    }
}
