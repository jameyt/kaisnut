using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
    public class Assignment:IAssignment
    {
        public Role Role { get; set; }
        public IEmployee Employee { get; set; }
        public DateTime Date { get; set; }
        public string ShortDate { get { return Date.ToLongDateString(); } }

        private Assignment(){}

        public static Assignment Create(Role role, Employee employee, DateTime date)
        {
            return new Assignment
            {
                Role = role, 
                Employee = employee, 
                Date = date
            };
        }

        public static Assignment CreateEmpty() { return new Assignment();}

        private string FormatRole()
        {
            switch (Role)
            {
                case Role.AM:
                    return "AM";
                case Role.Any:
                    return "Any";
                case Role.C2:
                    return "C2";
                case Role.CV:
                    return "CV";
                case Role.Num01:
                    return "01";
                case Role.Num02:
                    return "02";
                case Role.Num03:
                    return "03";
                case Role.Num04:
                    return "04";
                case Role.Num05:
                    return "05";
                case Role.Num06:
                    return "06";
                case Role.Num07:
                    return "07";
                case Role.Num08:
                    return "08";
                case Role.Num09:
                    return "09";
                case Role.Num010:
                    return "10";
                case Role.Num011:
                    return "11";
                case Role.Num012:
                    return "12";
                case Role.Num013:
                    return "13";
                case Role.Off:
                    return "Off";
                case Role.PC:
                    return "PC";
                case Role.PD:
                    return "PD";
                case Role.PM:
                    return "PM";
                case Role.SevenToFive:
                    return "7-5";
                case Role.SevenToOne:
                    return "7-1";
                case Role.SevenToTen:
                    return "7-10";
                case Role.SevenToThree:
                    return "7-3";
                case Role.V:
                    return "V";
                default:
                    return "";
            }
        }


        public string FormattedRole
        {
            get { return FormatRole(); }
        }
    }
}
