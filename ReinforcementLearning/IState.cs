using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public interface IState
    {
        bool IsTerminal { get; }
    }
}
