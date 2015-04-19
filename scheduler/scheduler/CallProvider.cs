using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class CallProvider : ICallProvider
    {
        private CallProvider() { }

        public static CallProvider Create() { return new CallProvider(); }

        public static CallProvider Create(int id, string provider, string name, string notes, string inTime, Hospital hospital, DateTime date) { return new CallProvider() { Id = id, Provider = provider, Name = name, Notes = notes, InTime = inTime, Hospital = hospital, Date = date }; }

        public int Id { get; set; }

        public string Provider { get; set; }

        public string Name { get; set; }

        public string Notes { get; set; }

        public string InTime { get; set; }

        public Hospital Hospital { get; set; }

        public DateTime Date { get; set; }

    }
}