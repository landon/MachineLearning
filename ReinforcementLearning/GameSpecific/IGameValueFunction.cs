using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public interface IGameValueFunction<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        double Evaluate(TState s);
        void Update(TState s, double value);

        void Save(string file);
        void Load(string file);
    }
}
