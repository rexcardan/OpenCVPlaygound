using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionsModule.MathHelpers
{
    public static class ClusterDetection1D
    {
        public static double[] Detect(IEnumerable<double> input, double threshold)
        {
            var data = input.OrderBy(i => i).ToArray();
            if (!data.Any())
                return Array.Empty<double>();

            var iLeft = 0;
            var iRight = 0;

            while (data.Length > iRight + 1 && data[iRight + 1] <= data[iLeft] + threshold)
                iRight++;
            
            var clusterSize = iRight - iLeft + 1;
            var biggestCluster = new { L = 0, Count = clusterSize };

            while (data.Length > iLeft + 1)
            {
                iLeft++;
                while (data.Length > iRight + 1 && data[iRight + 1] <= data[iLeft] + threshold)
                    iRight++;
                
                clusterSize = iRight - iLeft + 1;
                if (clusterSize > biggestCluster.Count)
                    biggestCluster = new { L = iLeft, Count = clusterSize };
            }

            return data.Skip(biggestCluster.L).Take(biggestCluster.Count).ToArray();
        }
    }
}
