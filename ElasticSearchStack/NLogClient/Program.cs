using System;
using NLog;

namespace NLogClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("NLog Core client");
            Logger NLogger = LogManager.GetLogger("cpo_logger");
                    
            var theEvent = new LogEventInfo(LogLevel.Warn, "cpo_logger", "testmessage1 ************************");
            theEvent.Properties["index"] = "people3";
            theEvent.Properties["SchemaName"] = "CPO_COGON";
            theEvent.Properties["Message"        ] = "testmessage1 ************************";
            theEvent.Properties["Timestamp"      ] = DateTime.Now;
            theEvent.Properties["LogLevel"       ] = LogLevel.Warn;
            theEvent.Properties["MessageKey"     ] = "1";
            theEvent.Properties["Parameters"     ] = string.Empty;
            theEvent.Properties["User"           ] = "Testuser";
            theEvent.Properties["ApplicationName"] = "CpoApp";
            theEvent.Properties["Action"         ] = (int)ActionType.Warning;
            theEvent.Properties["ObjectId"       ] = 2;
            theEvent.Properties["ObjectName"     ] = "people3";

            try
            {
                NLogger.Log(theEvent);
                Console.WriteLine("OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

    public enum ActionType
    {
        Undefined = 0,
        Create = 1,
        Update = 2,
        Delete = 3,
        Approve = 4,
        Information = 5,
        Error = 6,
        Debug = 7,
        Warning = 8
    }
}
