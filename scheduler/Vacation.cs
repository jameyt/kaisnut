using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Vacation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        
        public TimeSpan Length{get{return End - Start;}}

        public double Days{get{return Length.Days + Length.Hours / 24.0;}}

        private Vacation() { }

        public static Vacation Create(DateTime start, DateTime end)
        {
            return new Vacation() {Start = start, End = end};
        }


    }
}
