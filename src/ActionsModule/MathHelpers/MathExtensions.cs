using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActionsModule.MathHelpers
{
    public static class MathExtensions
    {
        public static double RadToDeg(this double deg)
            => deg * 180 / Math.PI;
        
        public static double DegToRad(this double rad)
            => rad * Math.PI / 180;

        public static double DistanceToRad(this double from, double to, bool ignoreRotationDirection = false)
        {
            var mod = ignoreRotationDirection
                            ? Math.PI
                            : Math.PI * 2;

            var n1 = from % mod;
            var n2 = to % mod;
            if (n1 > n2)
            {
                var t = n1;
                n1 = n2;
                n2 = t;
            }

            return Math.Min(n2 - n1, (n1 + mod) - n2);
        }

        public static (List<T> Left, List<T> Right) Split<T>(this IEnumerable<T> source, Func<T, bool> isLeft)
        {
            var left = new List<T>();
            var right = new List<T>();
            foreach (var item in source)
            {
                if (isLeft(item))
                    left.Add(item);
                else
                    right.Add(item);
            }

            return (left, right);
        }
    }
}
