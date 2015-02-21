using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler
{
   public class Month
    {
       public DateTime Date { get; set; }
       public List<Day> Days { get; set; }

       private Month(){}

       public static Month Create(DateTime date)
       {
           var month = new Month();
           month.Date = date;
           month.AddDays();
           return month;
       }

       private void AddDays()
       {
           var day = new DateTime(Date.Year, Date.Month, 1);
           while (day.Month == Date.Month)
           {
               Days.Add(Day.Create( null,day));
               day = day.AddDays(1);
           }
       }
    }
}
