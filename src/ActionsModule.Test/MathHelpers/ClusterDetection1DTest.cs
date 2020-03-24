using System;
using ActionsModule.MathHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

namespace ActionsModule.Test.MathHelpers
{
    public class CusterDetection1DTest
    {
        [Theory]
        [InlineData(new double[0], new double[0])]
        [InlineData(new[] { 0, 1, 2, 5, 6.01, 6.02, 6.03, 7, 8, 9, 10 }, new[] { 6.01, 6.02, 6.03 })]
        [InlineData(new[] { 6.01, 6.02, 6.03, 7, 8, 9, 10 }, new[] { 6.01, 6.02, 6.03 })]
        [InlineData(new[] { 1, 2, 5, 6.01, 6.02, 6.03 }, new[] { 6.01, 6.02, 6.03 })]
        public void DetectTest(double[] input, double[] expectedOutput)
        {
            var cluster = ClusterDetection1D.Detect(input, 0.1);

            cluster.Should().BeEquivalentTo(expectedOutput);
        }
    }
}
