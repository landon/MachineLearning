using ReinforcementLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    static partial class Ramsey
    {
        public class Action : IAction<Action>
        {
            public int E { get; private set; }
            public int Agent { get; set; }

            public Action(int e, int agent)
            {
                E = e;
                Agent = agent;
            }
        }
    }
}
