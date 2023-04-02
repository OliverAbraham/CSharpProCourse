using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace CoogleCalendarAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = Authenticate();
            var events = ReadAllEvents(credentials);
            Print(events);
            Console.ReadKey();
        }

        private static UserCredential Authenticate()
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/calendar-dotnet-quickstart.json
            string[] scopes = { CalendarService.Scope.CalendarReadonly };

            GoogleClientSecrets secrets;
            using (var stream = new FileStream(@"C:\Credentials\CsharpProCourse-GoogleCalenderApiDemo-credentials.json", FileMode.Open, FileAccess.Read))
            {
                secrets = GoogleClientSecrets.Load(stream);
            }

            string credPath = @"C:\Credentials\CsharpProCourse-GoogleCalenderApiDemo-token.json";
            var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets.Secrets, scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
            return credentials;
        }

        private static List<Event> ReadAllEvents(UserCredential credentials)
        {
            var results = new List<Event>();

            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Google Calendar API .NET Quickstart"
            });

            var request          = service.Events.List("primary");
            request.TimeMin      = DateTime.Now;
            request.ShowDeleted  = false;
            request.SingleEvents = true;
            request.MaxResults   = 100;
            request.OrderBy      = EventsResource.ListRequest.OrderByEnum.StartTime;

            Events events = request.Execute();
            if (events.Items != null && events.Items.Count > 0)
            {
                results.AddRange(events.Items);
            }

            return results;
        }

        private static void Print(List<Event> events)
        {
            foreach (var @event in events)
            {
                var summary = @event.Summary;
                Console.WriteLine($"{summary.Substring(0, Math.Min(summary.Length, 50)),-50} {@event?.Start?.Date,-20} {@event?.Start?.DateTime,-20} - {@event?.End?.DateTime,-20}");
            }
        }
    }
}
