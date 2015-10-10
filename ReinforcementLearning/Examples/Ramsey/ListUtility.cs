using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Examples
{
    static class ListUtility<T>
    {
        static ThreadLocal<HashSet<T>> _set = new ThreadLocal<HashSet<T>>(() => new HashSet<T>());
        static ThreadLocal<Dictionary<T, int>> _multiSet = new ThreadLocal<Dictionary<T, int>>(() => new Dictionary<T, int>());

        public static List<T> Intersection(IEnumerable<T> A, IEnumerable<T> B)
        {
            if (A == null || B == null)
                return new List<T>();

            var inB = _set.Value;
            inB.Clear();

            foreach (var t in B)
                inB.Add(t);

            var intersection = new List<T>(inB.Count);
            foreach (var t in A)
            {
                if (inB.Contains(t))
                    intersection.Add(t);
            }

            return intersection;
        }

        public static int IntersectionCount(IEnumerable<T> A, IEnumerable<T> B)
        {
            if (A == null || B == null)
                return 0;

            var inB = _set.Value;
            inB.Clear();

            foreach (var t in B)
                inB.Add(t);

            var count = 0;
            foreach (var t in A)
            {
                if (inB.Contains(t))
                    count++;
            }

            return count;
        }

        public static List<T> Difference(IEnumerable<T> A, IEnumerable<T> B)
        {
            if (A == null)
                return new List<T>();
            if (B == null)
                B = new List<T>();

            var inB = _set.Value;
            inB.Clear();

            foreach (var t in B)
                inB.Add(t);

            var difference = new List<T>(inB.Count);
            foreach (var t in A)
            {
                if (!inB.Contains(t))
                    difference.Add(t);
            }

            return difference;
        }

        public static List<T> Union(List<T> A, List<T> B)
        {
            if (A == null && B == null)
                return new List<T>();
            if (A == null)
                return B;
            if (B == null)
                return A;

            var inB = _set.Value;
            inB.Clear();

            foreach (T t in B)
                inB.Add(t);

            var union = new List<T>(B);
            foreach (T t in A)
            {
                if (!inB.Contains(t))
                    union.Add(t);
            }

            return union;
        }

        public static List<T> Union(List<T> A, T b)
        {
            var union = new List<T>(A);
            if (!union.Contains(b))
                union.Add(b);

            return union;
        }

        public static List<T> MultiSetIntersection(IEnumerable<T> A, IEnumerable<T> B)
        {
            if (A == null || B == null)
                return new List<T>();

            var inB = _multiSet.Value;
            inB.Clear();

            foreach (var t in B)
            {
                int count;
                inB.TryGetValue(t, out count);

                inB[t] = count + 1;
            }

            var intersection = new List<T>(inB.Count);
            foreach (var t in A)
            {
                int count;
                inB.TryGetValue(t, out count);

                if (count > 0)
                {
                    intersection.Add(t);
                    inB[t] = count - 1;
                }
            }

            return intersection;
        }

        public static int MultiSetIntersectionCount(IEnumerable<T> A, IEnumerable<T> B)
        {
            if (A == null || B == null)
                return 0;

            var inB = _multiSet.Value;
            inB.Clear();

            foreach (var t in B)
            {
                int count;
                inB.TryGetValue(t, out count);

                inB[t] = count + 1;
            }

            var total = 0;
            foreach (var t in A)
            {
                int count;
                inB.TryGetValue(t, out count);

                if (count > 0)
                {
                    total++;
                    inB[t] = count - 1;
                }
            }

            return total;
        }
    }
}
