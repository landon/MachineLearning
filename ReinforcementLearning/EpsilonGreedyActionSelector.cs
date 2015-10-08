using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class EpsilonGreedyActionSelector<TState, TAction> : IActionSelector<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        public static double Epsilon = 0.1;
        Random RNG = new Random(DateTime.Now.Millisecond);
        public TAction ChooseAction(TState s, List<TAction> actions, IActionValueFunction<TState, TAction> Q)
        {
            if (RNG.NextDouble() <= Epsilon)
                return actions[RNG.Next(0, actions.Count)];

            var maxActions = LearnerHelper.FindMaxActions(s, Q, actions);

            return maxActions[RNG.Next(0, maxActions.Count)];
        }
    }
}
