using System;
using System.Collections.Generic;
using System.Text;

namespace Compress
{
    public static class Arithmetic
    {
        //public static List<int> LongestArithmeticSubsequence(IList<int> a)
        //{
        //    return LongestArithmeticSubsequence(a, 1);


        //}

        public static int LongestArithmeticSubsequence(IList<int> a, int step)
        {
            var n = a.Count;
            var m = new Dictionary<int, int>();
            int maxt = 1;

            for (int i = 0; i < n; i++)
            {
                if (m.ContainsKey(a[i] - i * step))
                {
                    int freq = m[a[i] - i * step];
                    freq++;
                    m.Remove(a[i] - i * step);
                    m.Add(a[i] - i * step, freq);
                }
                else
                {
                    m.Add(a[i] - i * step, 1);
                }
            }

            foreach (var val in m)
            {
                if (maxt < val.Value)
                    maxt = val.Value;
            }
            return maxt;
        }
    }
}
