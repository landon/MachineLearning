using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IAction<TAction>
        where TAction : IAction<TAction>
    {
        int Agent { get; set; }
    }
}
