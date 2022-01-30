namespace Logging_simple
{
	public class Logger : ILogger
    {
        private const string LOGFILENAME = "Logfile.log";

        public void Log(string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            File.AppendAllText(LOGFILENAME, $"{timestamp}  -  {message}\n");
        }
    }
}
