using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public class GameGreedyActionSelector<TState, TAction> : IGameActionSelector<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        public bool IsExploring { get; private set; }

        public static double Epsilon = 0.1;
        Random RNG = new Random(DateTime.Now.Millisecond);
        public TAction ChooseAction(TState s, List<TAction> actions, IGameValueFunction<TState, TAction> V)
        {
            IsExploring = false;
            if (RNG.NextDouble() <= Epsilon)
            {
                IsExploring = true;
                return actions[RNG.Next(0, actions.Count)];
            }

            var best = double.MinValue;
            TAction bestAction = default(TAction);
            foreach (var a in actions)
            {
                var result = s.PerformAction(a);
                var value = V.Evaluate(result);
                if (value > best)
                {
                    best = value;
                    bestAction = a;
                }
            }

            if (bestAction == null)
                System.Diagnostics.Debugger.Break();
            return bestAction;
        }

        //public TAction ChooseAction(TState s, List<TAction> actions, IGameValueFunction<TState, TAction> V)
        //{
        //    IsExploring = false;
        //    if (RNG.NextDouble() <= Epsilon)
        //    {
        //        IsExploring = true;
        //        return actions[RNG.Next(0, actions.Count)];
        //    }

        //    var bestActions = new List<TAction>();
        //    var best = double.MinValue;
        //    foreach (var a in actions)
        //    {
        //        var result = s.PerformAction(a);
        //        var value = V.Evaluate(result);
        //        if (value >= best)
        //        {
        //            if (value > best)
        //                bestActions.Clear();

        //            best = value;
        //            bestActions.Add(a);
        //        }
        //    }

        //    return bestActions[RNG.Next(0, bestActions.Count)];
        //}
    }
}
