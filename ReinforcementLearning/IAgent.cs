﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IAgent<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        TState Act(IWorld<TState, TAction> world);
        void BeginEpisode();
        void EndEpisode(double value);

        void Save(string file);
    }
}
