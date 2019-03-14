using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bishopric6Cal
{
    public static class AppointmentList
    {
        private static List<Appointment> _list = new  List<Appointment>();

        public static List<Appointment> List { get => _list; set => _list = value; }

        public static void Add(Appointment Appointment)
        {
            _list.Add(Appointment);
        }
        public static void Clear()
        {
            _list.Clear();
        }
    }
}
