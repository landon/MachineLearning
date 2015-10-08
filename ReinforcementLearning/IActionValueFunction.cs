using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IActionValueFunction<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        double Evaluate(TState s, TAction a);
        void Update(TState oldState, TAction a, double value);
    }
}
