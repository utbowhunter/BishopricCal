using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishopric6Cal
{
    public static class Calendars
    {
        private static List<string> skippedCalendars = new List<string>();
        private static List<string> calendars = new List<string>();

        public static List<string> SkippedCalendars { get => skippedCalendars; set => skippedCalendars = value; }
        public static List<string> CalendarList { get => calendars; set => calendars = value; }

        public static void GetCalendars()
        {
            List<string> calenders = new List<string>();

            string[] cals = ConfigurationManager.AppSettings["Calendars"].Split(',');

            foreach (string cal in cals)
            {
                CalendarList.Add(cal);
            }



        }
        public static void GetSkippedCalendars()
        {
            List<string> calenders = new List<string>();

            string[] skippedCals = ConfigurationManager.AppSettings["SkippedCalendars"].Split(',');

            foreach (string skippedCal in skippedCals)
            {
                SkippedCalendars.Add(skippedCal);
            }


        }
        public static bool IsCalSkipped(string id)
        {
            bool skippedCal = SkippedCalendars.Contains(id);

            return skippedCal;
        }

    }
}
