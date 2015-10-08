using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public class GameUpdateRule<TState, TAction> : IGameUpdateRule<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        public static double LearningRate = 0.1;

        public double GetNewValue(double previousValue, double value, double reward)
        {
            return (1.0 - LearningRate) * previousValue + LearningRate * value;
        }
    }
}
