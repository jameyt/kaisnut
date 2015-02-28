using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumEvenFibonacciTo4Million
{
    public class Fibonacci
    {
        private Fibonacci() { }

        public Fibonacci Create() { return new Fibonacci(); }

        public static int CreateNumber(int length)
        {
            switch (length)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                default:
                    return CreateNumber(length - 1) + CreateNumber(length - 2);
            }
        }

        private static List<int> _sequence;

        public static  List<int> CreateSequence(int length)
        {
            if (_sequence == null)
            {
                _sequence = new List<int> {1, 2};
            }

            while (_sequence.Count < length)
            {
                _sequence.Add(_sequence[_sequence.Count-1]+_sequence[_sequence.Count-2]);
            }

            return _sequence;
        }

        public static List<int> CreateSequenceToSize(int size)
        {
            if (_sequence == null)
            {
                _sequence = new List<int> { 1, 2 };
            }

            while (_sequence[_sequence.Count - 1] <= size)
            {
                var nextNumber = _sequence[_sequence.Count - 1] + _sequence[_sequence.Count - 2];
                if (nextNumber > size) { break;}
                _sequence.Add(nextNumber);
            } 

            return _sequence;
        }

        public static int SumFibonacciMultiples(int size,int multiple)
        {
            var sequence = CreateSequenceToSize(size);

            return sequence.Where(number => number%multiple == 0).Where(number => number<=size).Sum();
        }

    }
}
