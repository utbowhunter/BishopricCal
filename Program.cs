using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bishopric6Cal
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Bishopric 6th Ward Calendar";

        
        static void Main(string[] args)
        {
            // ***** Get the skipped calendars
            Calendars.GetSkippedCalendars();

            // ***** Get the calendars
            Calendars.GetCalendars();


            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            // Get the list of calendars for the bishopic
            var list = service.CalendarList.List().Execute().Items;

            //DateTime startDateTime = DateTime.Now.AddDays(-1);
            // Loop through the calendars
            foreach (var c in list)
            {


                bool isCalSkipped = Calendars.IsCalSkipped(c.Id);

                if (!isCalSkipped)
                {
                    Console.WriteLine("{0} : ID: {1}", c.Summary, c.Id);

                    // Get the events of the calendar
                    var calEvents = service.Events.List(c.Id).Execute().Items;//.Where(i => startDateTime >= DateTime.Now).ToList();

                    //calEvents.TimeMin = DateTime.Now.AddDays(-1);
                    //calEvents.ShowDeleted = false;
                    //calEvents.SingleEvents = true;
                    //calEvents.MaxResults = 10;
                    //calEvents.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                    int count = 0;
                    try
                    {
                        // Loop through the events
                        foreach (var calEvent in calEvents)
                        {
                            Appointment appointment = new Appointment();

                            if (calEvent.Status != "cancelled")
                            {
                                if (calEvent.Start.DateTime > DateTime.Now)
                                {
                                    // ***** Create the Event
                                    appointment.CalendarName = c.Summary;
                                    appointment.Text = calEvent.Summary;
                                    appointment.StartTime = calEvent.Start.DateTime;
                                    appointment.EndTime = calEvent.End.DateTime;

                                    if (calEvent.Recurrence == null)
                                    {
                                        appointment.Recurring = false;
                                    }
                                    else
                                    {
                                        appointment.Recurring = true;
                                    }

                                    Console.WriteLine("{0}:{1}:{2}:{3}:{4}", appointment.CalendarName, appointment.Text, appointment.StartTime.ToString(), appointment.EndTime.ToString(), appointment.Recurring.ToString());
                                    // ***** Add event to the List
                                    AppointmentList.Add(appointment);
                                }
                            }
                            count++;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }


            

            RunSchedule();

            Console.Read();

        }

 
        private static bool IsCalSkipped()
        {
            throw new NotImplementedException();
        }

 
        private static void RunSchedule()
        {
            // Get the list bishopric recurring appointments
            List<Appointment> _recurringBishopricAppt = AppointmentList.List.FindAll(x => x.Recurring = true && x.CalendarName.Contains("Bishopric") && x.StartTime <= DateTime.Now.AddDays(7));

          
            foreach (string calendar in Calendars.CalendarList)
            {

                if (!calendar.Contains("Bishopric"))
                {
                    List<Appointment> calAppts = AppointmentList.List.FindAll(x => x.CalendarName == calendar && x.StartTime <= DateTime.Now.AddDays(7));

                    string _emailMessage = Email.BuildEmail(_recurringBishopricAppt, calAppts);

                    //Email.SendEmail(_emailMessage);
                }
            }            
        }
    }

}
