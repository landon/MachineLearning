using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public interface IGameUpdateRule<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        double GetNewValue(double previousValue, double value, double reward);
    }
}
