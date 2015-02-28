using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SumEvenFibonacciTo4Million.Tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void CreatesFibonacciSequenceTo10()
        {
            var expectedSequence = new List<int>() {1, 2, 3, 5, 8, 13, 21, 34, 55, 89};
            var fibonacciSequence = Fibonacci.CreateSequence(10);

        }
    }
}
