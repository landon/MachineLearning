using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class BasicQUpdateRule<TState, TAction> : IUpdateRule<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        public static double DiscountFactor = 1.0;
        public static double LearningRate = 0.3;

        public double GetNewValue(TState oldState, double oldValue, TState newState, double reward, IActionValueFunction<TState, TAction> Q, List<TAction> actions)
        {
            return (1.0 - LearningRate) * oldValue + LearningRate * (reward + DiscountFactor * LearnerHelper.FindMaxActionValue(newState, Q, actions));
        }
    }
}
