using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using ReinforcementLearning.GameSpecific;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    static partial class TicTacToe
    {
        public class NeuralNetValueFunction : IGameValueFunction<State, Action>
        {
            BasicNetwork _network;

            public NeuralNetValueFunction()
            {
                _network = new BasicNetwork();
                _network.AddLayer(new BasicLayer(null, true, N * N));
                _network.AddLayer(new BasicLayer(new ActivationTANH(), true, 6));
                _network.AddLayer(new BasicLayer(new ActivationTANH(), true, 3));
                _network.AddLayer(new BasicLayer(new ActivationTANH(), false, 1));
                _network.Structure.FinalizeStructure();
                _network.Reset();
            }

            public double Evaluate(State s)
            {
                return Evaluate(s.AsDoubles());
            }

            double Evaluate(double[] input)
            {
                var output = new double[1];
                _network.Compute(input, output);
                return output[0];
            }

            public void Update(State s, double value)
            {
                var pair = new BasicMLDataPair(new BasicMLData(s.AsDoubles()), new BasicMLData(new double[] { value }));
                var trainingSet = new BasicMLDataSet(new[] { pair });

                var train = new Encog.Neural.Networks.Training.Propagation.Back.Backpropagation(_network, trainingSet);
                train.BatchSize = 1;

                train.Iteration(2);
                if (train.Error > 0.01)
                    train.Iteration(3);

                var error = train.Error;
            }

            public void Save(string file)
            {
                var lookup = BuildFakeLookup();

                using (var sw = new StreamWriter(file))
                {
                    foreach (var kvp in lookup.OrderBy(k => k.Key))
                        sw.WriteLine(kvp.Key.Replace(Environment.NewLine, " ") + " " + kvp.Value.ToString("0.00###"));
                }
            }

            Dictionary<string, double> BuildFakeLookup()
            {
                var lookup = new Dictionary<string, double>();

                foreach (var d in EnumerateBoards(new List<double>()))
                {
                    var key = string.Join(Environment.NewLine, Enumerable.Range(0, N).Select(i => string.Join("", Enumerable.Range(0, N).Select(j =>
                    {
                        if (d[j*N + i] == 1)
                            return "X";
                        if (d[j*N + i] == -1)
                            return "O";

                        return "-";
                    }))));

                    lookup[key] = Evaluate(d);
                }

                return lookup;
            }

            IEnumerable<double[]> EnumerateBoards(List<double> list)
            {
                if (list.Count == N * N)
                {
                    yield return list.ToArray();
                }
                else
                {
                    var things = new[] { -1, 0, 1 };
                    foreach (var t in things)
                    {
                        list.Add(t);
                        foreach (var b in EnumerateBoards(list))
                            yield return b;
                        list.RemoveAt(list.Count - 1);
                    }
                }
            }

            public void Load(string file)
            {
            }
        }
    }
}
