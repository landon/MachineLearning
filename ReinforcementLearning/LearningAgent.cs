using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class LearningAgent<TState, TAction, TActionValueFunction, TActionSelector, TUpdateRule> : IAgent<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
        where TActionValueFunction : IActionValueFunction<TState, TAction>, new()
        where TActionSelector : IActionSelector<TState, TAction>, new()
        where TUpdateRule : IUpdateRule<TState, TAction>, new()
    {
        TActionValueFunction Q;
        IActionSelector<TState, TAction> ActionSelector;
        IUpdateRule<TState, TAction> UpdateRule;

        public LearningAgent()
        {
            ActionSelector = new TActionSelector();
            UpdateRule = new TUpdateRule();
            Q = new TActionValueFunction();
        }

        public TState Act(IWorld<TState, TAction> world)
        {
            var s = world.CurrentState;
            var actions = world.GetPossibleActions();
            var a = ActionSelector.ChooseAction(s, actions, Q);
            var oldValue = Q.Evaluate(s, a);
            double reward;
            var newState = world.PerformAction(a, out reward);
            var newValue = UpdateRule.GetNewValue(s, oldValue, newState, reward, Q, world.GetPossibleActions());
            Q.Update(s, a, newValue);

            return newState;
        }

        public void BeginEpisode()
        {
        }

        public void EndEpisode(double value)
        {
        }

        public void Save(string file)
        {
        }
    }
}
