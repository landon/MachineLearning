using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class RandomAgent<TState, TAction> : IAgent<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        Random RNG = new Random(DateTime.Now.Millisecond);

        public TState Act(IWorld<TState, TAction> world)
        {
            var actions = world.GetPossibleActions();
            double reward;
            return world.PerformAction(actions[RNG.Next(0, actions.Count)], out reward);
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
