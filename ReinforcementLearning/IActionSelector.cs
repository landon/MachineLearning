﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IActionSelector<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        TAction ChooseAction(TState s, List<TAction> actions, IActionValueFunction<TState, TAction> Q);
    }
}
