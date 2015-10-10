using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    static class GraphUtility
    {
        public static bool[,] GetAdjacencyMatrix(IEnumerable<int> edgeWeights)
        {
            var ew = edgeWeights.ToList();
            var n = (int)((1 + Math.Sqrt(1 + 8 * ew.Count)) / 2);

            var adjacencyMatrix = new bool[n, n];

            int k = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    if (ew[k] != 0)
                    {
                        adjacencyMatrix[i, j] = true;
                        adjacencyMatrix[j, i] = true;
                    }

                    k++;
                }
            }

            return adjacencyMatrix;
        }
    }
}
