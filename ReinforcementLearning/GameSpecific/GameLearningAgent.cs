using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public class GameLearningAgent<TState, TAction, TGameValueFunction, TGameActionSelector, TGameUpdateRule> : IAgent<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
        where TGameValueFunction : IGameValueFunction<TState, TAction>, new()
        where TGameActionSelector : IGameActionSelector<TState, TAction>, new()
        where TGameUpdateRule : IGameUpdateRule<TState, TAction>, new()
    {
        TGameValueFunction V;
        IGameActionSelector<TState, TAction> ActionSelector;
        IGameUpdateRule<TState, TAction> UpdateRule;
        bool _firstMove = true;
        bool _previousStateWasExploring;
        TState _previousState;

        public GameLearningAgent(string file = null)
        {
            ActionSelector = new TGameActionSelector();
            UpdateRule = new TGameUpdateRule();
            V = new TGameValueFunction();

            if (file != null)
                V.Load(file);
        }

        public void BeginEpisode()
        {
            _firstMove = true;
        }

        public void EndEpisode(double value)
        {
            if (_firstMove || _previousStateWasExploring)
                return;

            var v = UpdateRule.GetNewValue(V.Evaluate(_previousState), value, 0);
            V.Update(_previousState, v);
        }

        public TState Act(IWorld<TState, TAction> world)
        {
            var s = world.CurrentState;

            var actions = world.GetPossibleActions();
            var a = ActionSelector.ChooseAction(s, actions, V);

            double reward;
            var newState = world.PerformAction(a, out reward);

            if (!_firstMove)
            {
                var currentStateValue = V.Evaluate(s);

                if (newState.IsTerminal)
                {
                    currentStateValue = UpdateRule.GetNewValue(currentStateValue, newState.TerminalValue, reward);
                    V.Update(s, currentStateValue);
                }

                if (!_previousStateWasExploring)
                {
                    var previousValue = V.Evaluate(_previousState);
                    var updatedPreviousStateValue = UpdateRule.GetNewValue(previousValue, currentStateValue, reward);
                    V.Update(_previousState, updatedPreviousStateValue);
                }
            }

            _previousState = s;
            _previousStateWasExploring = ActionSelector.IsExploring;
            _firstMove = false;
            return newState;
        }

        public void Save(string file)
        {
            V.Save(file);
        }
    }
}
