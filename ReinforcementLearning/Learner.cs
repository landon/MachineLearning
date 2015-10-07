using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class Learner<TState, TAction, TStateMachine, TActionValueFunction>
        where TState : IState
        where TAction : IAction
        where TStateMachine : IStateMachine<TState, TAction>, new()
        where TActionValueFunction : IActionValueFunction<TState, TAction>, new()
    {
        TActionValueFunction Q;
        IActionSelector<TState, TAction> ActionSelector;
        IUpdateRule<TState, TAction> UpdateRule;

        public Learner(IActionSelector<TState, TAction> actionSelector, IUpdateRule<TState, TAction> updateRule)
        {
            ActionSelector = actionSelector;
            UpdateRule = updateRule;
            Q = new TActionValueFunction();
        }

        public void RunEpisode()
        {
            var machine = new TStateMachine();
            var s = machine.InitialState;

            while (!s.IsTerminal)
            {
                var actions = machine.GetPossibleActions();
                var a = ActionSelector.ChooseAction(s, actions, Q);
                var oldValue = Q.Evaluate(s, a);
                double reward;
                var newState = machine.PerformAction(a, out reward);
                var newValue = UpdateRule.GetNewValue(s, oldValue, newState, reward, Q);
                Q.Update(s, a, newValue);
                s = newState;
            }
        }
    }
}
