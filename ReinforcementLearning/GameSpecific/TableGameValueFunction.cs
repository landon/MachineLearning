using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ReinforcementLearning.GameSpecific
{
    public class TableGameValueFunction<TState, TAction> : IGameValueFunction<TState, TAction>
        where TState : IState<TState, TAction>
        where TAction : IAction<TAction>
    {
        Dictionary<string, double> _lookup = new Dictionary<string, double>();
        Func<double> _initialValueSelector;

        public TableGameValueFunction()
        {
            _initialValueSelector = () => 0.0;
        }

        public double Evaluate(TState s)
        {
            var ss = s.ToString();
            double value;
            if (!_lookup.TryGetValue(ss, out value))
            {
                if (s.IsTerminal)
                    value = s.TerminalValue;
                else
                    value = _initialValueSelector();
                _lookup[ss] = value;
            }

            return value;
        }

        public void Update(TState s, double value)
        {
            _lookup[s.ToString()] = value;
        }

        public void Save(string file)
        {
            using (var sw = new StreamWriter(file))
            {
                foreach (var kvp in _lookup.OrderBy(k => k.Key))
                    sw.WriteLine(kvp.Key.Replace(Environment.NewLine, " ") + " " + kvp.Value);
            }
        }

        public void Load(string file)
        {
            using (var sr = new StreamReader(file))
            {
                _lookup = new Dictionary<string, double>();
                var lines = sr.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 4)
                        continue;
                    
                    var key = parts[0] + Environment.NewLine + parts[1] + Environment.NewLine + parts[2];
                    _lookup[key] = double.Parse(parts[3]);
                }
            }
        }
    }
}
