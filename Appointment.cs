using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishopric6Cal
{
    public class Appointment
    {
        public string CalendarName { get; set; }
        public string Text { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Recurring { get; set; }


    }
}
