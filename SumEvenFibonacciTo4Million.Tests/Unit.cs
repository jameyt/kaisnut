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
            for (var i = 0; i < expectedSequence.Count; i++)
            {
                Assert.AreEqual(expectedSequence[i],fibonacciSequence[i]);
            }
        }

        [TestMethod]
        public void Creates10thFibonacciNumber()
        {
            Assert.AreEqual(89,Fibonacci.CreateNumber(10));
        }

        [TestMethod]
        public void Creates10thFibonacciNumberToSize()
        {
            var fibonacci = Fibonacci.CreateSequenceToSize(89);
            Assert.AreEqual(89,fibonacci[fibonacci.Count-1] );
        }

        [TestMethod]
        public void SumsEvensTo89()
        {
            var fibonacci = Fibonacci.SumFibonacciMultiples(89,2);
            Assert.AreEqual(44, fibonacci);
        }

        [TestMethod]
        public void SumsEvensTo4000000()
        {
            var fibonacci = Fibonacci.SumFibonacciMultiples(4000000,2);
            Assert.AreEqual(4613732, fibonacci);
        }
    }
}
