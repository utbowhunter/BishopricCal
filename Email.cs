using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishopric6Cal
{
    public static class Email
    {
        public static string BuildEmail(List<Appointment> recurringBishopricAppt, List<Appointment> calAppts)
        {
          
            List<Appointment> fullList = recurringBishopricAppt.Concat(calAppts).OrderBy(x => x.StartTime).ToList();
            
            string emailMessage = string.Empty;

            foreach (Appointment appt in fullList)
            {
                
                emailMessage += string.Format("{0, -20}{1, -20}\t{2}\n\r", appt.StartTime?.ToString("h:mm tt"), appt.EndTime?.ToString("h:mm tt"), appt.Text);
            }


            Console.WriteLine(emailMessage);

            return emailMessage;

        }

        internal static void SendEmail(string emailMessage)
        {
            throw new NotImplementedException();
        }

        public static string ToString(this DateTime? dt, string format)    => dt == null ? "n/a" : ((DateTime)dt).ToString(format);

    }
}
