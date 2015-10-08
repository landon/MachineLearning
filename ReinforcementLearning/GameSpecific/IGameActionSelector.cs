using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public interface IGameActionSelector<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        bool IsExploring { get; }
        TAction ChooseAction(TState s, List<TAction> actions, IGameValueFunction<TState, TAction> V);
    }
}
