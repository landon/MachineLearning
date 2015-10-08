﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public static class LearnerHelper
    {
        public static double FindMaxActionValue<TState, TAction>(TState newState, IActionValueFunction<TState, TAction> Q, List<TAction> actions)
            where TState : IState<TState, TAction>
            where TAction : IAction<TAction>
        {
            TAction action;
            return FindMaxAction(newState, Q, actions, out action);
        }
     
        public static double FindMaxAction<TState, TAction>(TState newState, IActionValueFunction<TState, TAction> Q, List<TAction> actions, out TAction action)
            where TState : IState<TState, TAction>
            where TAction : IAction<TAction>
        {
            action = default(TAction);
            var best = double.MinValue;
            foreach (var a in actions)
            {
                var q = Q.Evaluate(newState, a);
                if (q > best)
                {
                    action = a;
                    best = q;
                }
            }

            return best;
        }

        public static List<TAction> FindMaxActions<TState, TAction>(TState newState, IActionValueFunction<TState, TAction> Q, List<TAction> actions)
            where TState : IState<TState, TAction>
            where TAction : IAction<TAction>
        {
            var maxActions = new List<TAction>();
            var best = double.MinValue;
            foreach (var a in actions)
            {
                var q = Q.Evaluate(newState, a);
                if (q >= best)
                {
                    if (q > best)
                        maxActions.Clear();

                    maxActions.Add(a);
                    best = q;
                }
            }

            return maxActions;
        }
    }
}
