using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class TableActionValueFunction<TState, TAction> : IActionValueFunction<TState, TAction>
        where TState : IState
        where TAction : IAction
    {
        Dictionary<TState, Dictionary<TAction, double>> _lookup = new Dictionary<TState, Dictionary<TAction, double>>();
        Func<double> _initialValueSelector;

        public TableActionValueFunction()
        {
            var rng = new Random();
            _initialValueSelector = () => rng.NextDouble();
        }

        public double Evaluate(TState s, TAction a)
        {
            var table = GetActionTable(s);
            double value;
            if (!table.TryGetValue(a, out value))
            {
                value = _initialValueSelector();
                table[a] = value;
            }

            return value;
        }

        public void Update(TState s, TAction a, double value)
        {
            GetActionTable(s)[a] = value;
        }

        Dictionary<TAction, double> GetActionTable(TState state)
        {
            Dictionary<TAction, double> table;
            if (!_lookup.TryGetValue(state, out table))
            {
                table = new Dictionary<TAction, double>();
                _lookup[state] = table;
            }

            return table;
        }
    }
}
