using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;

namespace CoogleCalendarAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = Authenticate();
            var contacts = ReadAllMyContacts(credentials);
            Print(contacts);
        }

        private static UserCredential Authenticate()
        {
            // If modifying these scopes, delete your previously saved credentials
            // at ~/.credentials/calendar-dotnet-quickstart.json
            //static string[] Scopes = { Google.Apis.PeopleService.v1.PeopleServiceService.Scope.Contacts };
            string[] scopes = new string[] { "https://www.googleapis.com/auth/contacts.readonly" };

            GoogleClientSecrets secrets;
            using (var stream = new FileStream(@"C:\Credentials\CsharpProCourse-GooglePeopleApiDemo-credentials.json", FileMode.Open, FileAccess.Read))
            {
                secrets = GoogleClientSecrets.Load(stream);
            }

            string credPath = @"C:\Credentials\CsharpProCourse-GooglePeopleApiDemo-token.json";
            var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(secrets.Secrets, scopes, "user", CancellationToken.None, new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
            return credentials;
        }

        private static List<Person> ReadAllMyContacts(UserCredential credentials)
        {
            var results = new List<Person>();

            // Create Google Calendar API service.
            var service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Google People API .NET Quickstart"
            });

            string pageToken = null;
            ListConnectionsResponse people;
            do
            {
                var peopleRequest = service.People.Connections.List("people/me");
                peopleRequest.PersonFields = "names,emailAddresses";
                if (pageToken != null)
                    peopleRequest.PageToken = pageToken;

                people = peopleRequest.Execute();
                if (people != null && people.Connections != null && people.Connections.Count > 0)
                {
                    results.AddRange(people.Connections);
                }
                pageToken = people.NextPageToken;
            }
            while (people.NextPageToken is not null);

            return results;
        }

        private static void Print(List<Person> contacts)
        {
            foreach (var person in contacts)
            {
                var name = person.Names != null ? (person.Names[0].DisplayName ?? "n/a") : "n/a";
                var email = person.EmailAddresses != null ? (person.EmailAddresses[0].Value ?? "n/a") : "n/a";
                Console.WriteLine($"{name.Substring(0, Math.Min(name.Length, 50)),-50} {email}");
            }
        }
    }
}
