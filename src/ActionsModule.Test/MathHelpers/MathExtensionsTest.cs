using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionsModule.MathHelpers;
using FluentAssertions;
using Xunit;

namespace ActionsModule.Test.MathHelpers
{
    public class MathExtensionsTest
    {
        [Theory]
        [InlineData(0, 0.1, 0.1)]
        [InlineData(0, Math.PI * 2, 0)]
        [InlineData(0, Math.PI * 2 - 0.1, 0.1)]
        [InlineData(0.1, 0.0, 0.1)]
        [InlineData(Math.PI * 2, 0, 0)]
        [InlineData(Math.PI * 2 - 0.1, 0, 0.1)]
        [InlineData(0, 0.1, 0.1, true)]
        [InlineData(0, Math.PI, 0, true)]
        [InlineData(0, Math.PI - 0.1, 0.1, true)]
        [InlineData(0.1, 0.0, 0.1, true)]
        [InlineData(Math.PI, 0, 0, true)]
        [InlineData(Math.PI - 0.1, 0, 0.1, true)]
        public void DistanceToRadTest(double from, double to, double expected, bool ignoreRot = false)
        {
            from.DistanceToRad(to, ignoreRot).Should().BeApproximately(expected, 0.000001);
        }
    }
}
