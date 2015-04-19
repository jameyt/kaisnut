using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class DetailedAssignment:IDetailedAssignment
    {
        private DetailedAssignment() { }

        public static DetailedAssignment Create() { return new DetailedAssignment();}

        public static DetailedAssignment Create(int id, string location, string surgeon, string time, string provider, string peelOut, Hospital hospital, DateTime date) { return new DetailedAssignment(){Id=id,Location=location,Surgeon = surgeon,Time = time,Provider=provider,PeelOut=peelOut,Hospital=hospital,Date=date};}

        public int Id { get; set; }

        public string Location{ get; set; }

        public string Surgeon{ get; set; }

        public string Time{ get; set; }

        public string Provider{ get; set; }

        public string PeelOut{ get; set; }

        public Hospital Hospital{ get; set; }

        public DateTime Date { get; set; }
       
    }
}
