using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReinforcementLearning
{
    public class TableActionValueFunction<TState, TAction> : IActionValueFunction<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        Dictionary<string, Dictionary<string, double>> _lookup = new Dictionary<string, Dictionary<string, double>>();
        Func<double> _initialValueSelector;

        public TableActionValueFunction()
        {
            var rng = new Random();
            //_initialValueSelector = () => rng.NextDouble();
            _initialValueSelector = () => 0.1;
        }

        public double Evaluate(TState s, TAction a)
        {
            var astr = a.ToString();
            var table = GetActionTable(s);
            double value;
            if (!table.TryGetValue(astr, out value))
            {
                value = _initialValueSelector();
                table[astr] = value;
            }

            return value;
        }

        public void Update(TState oldState, TAction a, double value)
        {
            GetActionTable(oldState)[a.ToString()] = value;
        }

        Dictionary<string, double> GetActionTable(TState state)
        {
            var s = state.ToString();
            Dictionary<string, double> table;
            if (!_lookup.TryGetValue(s, out table))
            {
                table = new Dictionary<string, double>();
                _lookup[s] = table;
            }

            return table;
        }
    }
}
