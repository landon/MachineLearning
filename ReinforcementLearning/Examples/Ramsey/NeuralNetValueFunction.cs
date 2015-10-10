using Encog.Engine.Network.Activation;
using Encog.ML.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using ReinforcementLearning.GameSpecific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    static partial class Ramsey
    {
        public class NeuralNetValueFunction : IGameValueFunction<State, Action>
        {
            BasicNetwork _network;

            public NeuralNetValueFunction()
            {
                _network = new BasicNetwork();
                _network.AddLayer(new BasicLayer(null, true, N * (N - 1) / 2));
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

                train.Iteration();
            }

            public void Save(string file)
            {
            }

            public void Load(string file)
            {
            }
        }
    }
}
