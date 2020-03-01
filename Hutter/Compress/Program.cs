using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Compress
{
    class Program
    {
        static void Main(string[] args)
        {
            //var counts = CharCounts();
            //var inorder = counts.AsEnumerable().OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();

            //Console.WriteLine(string.Join(',', counts.Select(kv => $"{kv.Key},{kv.Value}")));

            // var indices = Indices();

            var list = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });

            var vv = Arithmetic.LongestArithmeticSubsequence(list, 1); ;


        }

        static Dictionary<char, List<int>> Indices()
        {
            var indices = new Dictionary<char, List<int>>();
            using (var sr = new StreamReader("C:/hutter/t1.txt"))
            {
                var index = 0;
                while (true)
                {
                    var buf = new char[1];
                    var numReturned = sr.Read(buf, 0, 1);
                    if (numReturned == 0)
                        break;
                   
                    var c = buf[0];
                    if (!indices.ContainsKey(c))
                        indices[c] = new List<int>();
                    indices[c].Add(index);

                    index++;
                }
            }
            return indices;
        }

        static Dictionary<char, int> CharCounts() 
        {
            var counts = new Dictionary<char, int>();
            using (var sr = new StreamReader("C:/hutter/t1.txt"))
            {
                var index = 0;
                while (true)
                {
                    var buf = new char[1];
                    var numReturned = sr.Read(buf, 0, 1);
                    if (numReturned == 0)
                        break;
                    index++;
                    var c = buf[0];
                    if (!counts.ContainsKey(c))
                        counts[c] = 0;
                    counts[c]++;
                }
            }

            return counts;
        }
    }
}
