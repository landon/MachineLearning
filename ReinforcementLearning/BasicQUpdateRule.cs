using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class BasicQUpdateRule<TState, TAction> : IUpdateRule<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        public static double DiscountFactor = 0.9;
        public static double LearningRate = 0.2;

        public double GetNewValue(TState oldState, double oldValue, TState newState, double reward, IActionValueFunction<TState, TAction> Q, List<TAction> actions)
        {
            return (1.0 - LearningRate) * oldValue + LearningRate * (reward + DiscountFactor * LearnerHelper.FindMaxActionValue(newState, Q, actions));
        }
    }
}
