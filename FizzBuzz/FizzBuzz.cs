using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizzBuzz
{
    public class FizzBuzz
    {
        public List<string> Output { get; set; }

        private FizzBuzz(int length)
        {
            Output = new List<string>();

            for (int i = 0; i < 100; i++)
            {
                var output = "";
                if (i%3 == 0){output += "fizz";}
                if (i % 5 == 0) { output += "buzz"; }
               Output.Add(output);
            }
        }

        public static FizzBuzz Create(int length)
        {
            return new FizzBuzz(length);
        }
    }
}
