using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FizzBuzz.Tests
{
    [TestClass]
    public class Unit
    {
        [TestMethod]
        public void Create()
        {
            var fizzBuzz = FizzBuzz.Create(100);
        }

        [TestMethod]
        public void OutputMade()
        {
            var fizzBuzz = FizzBuzz.Create(100);
            Assert.IsNotNull(fizzBuzz.Output);
        }

        [TestMethod]
        public void OutputFilled()
        {
            var fizzBuzz = FizzBuzz.Create(100);
            Assert.IsTrue(fizzBuzz.Output.Count == 100);
        }

        [TestMethod]
        public void OutputsFizzOnce()
        {

            var fizzBuzz = FizzBuzz.Create(100);
            Assert.AreEqual("fizz", fizzBuzz.Output[3]);
        }

        [TestMethod]
        public void OutputsBuzzOnce()
        {
            var fizzBuzz = FizzBuzz.Create(100);
            Assert.AreEqual("buzz", fizzBuzz.Output[5]);
        }

        [TestMethod]
        public void OutputsFizzAll()
        {
            const int size = 100;
            var fizzBuzz = FizzBuzz.Create(size);
            for (var i = 0; i < size; i++)
            {
                if (i % 3 == 0 && i % 5 != 0)
                {
                    Assert.AreEqual("fizz", fizzBuzz.Output[i]);
                }
            }

        }

        [TestMethod]
        public void OutputsBuzzAll()
        {
            const int size = 100;
            var fizzBuzz = FizzBuzz.Create(size);
            for (var i = 0; i < size; i++)
            {
                if (i % 5 == 0 && i % 3 != 0)
                {
                    Assert.AreEqual("buzz", fizzBuzz.Output[i]);
                }
            }
        }
    }
}
