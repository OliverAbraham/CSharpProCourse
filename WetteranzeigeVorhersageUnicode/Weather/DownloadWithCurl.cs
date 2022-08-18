using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wetteranzeige
{
	class DownloadWithCurl
	{
        public int WaitTimeoutInSeconds { get; set; } = 30;

        public string DownloadFromUrl(string url)
        {
            return CallProcessAndReturnConsoleOutput("cmd" , "/K dir & curl " + url + " & pause & exit");
        }

        private string CallProcessAndReturnConsoleOutput(string filename, string arguments)
        {
            string Output = "";
            using (Process p = new Process())
            {
                p.StartInfo.FileName = filename;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = false;//true;
                p.StartInfo.CreateNoWindow = false;
                p.Start();

                //Output = p.StandardOutput.ReadToEnd();
                int MaxLineCount = 1000;
                DateTime Timeout = DateTime.Now.AddSeconds(WaitTimeoutInSeconds);
                while (!p.StandardOutput.EndOfStream)
                {
                    Output += p.StandardOutput.ReadLine();
                    MaxLineCount--;
                    if (MaxLineCount <= 0 || DateTime.Now > Timeout)
                        break; // prevent from looping endless
                }
                if (DateTime.Now > Timeout)
                {
                    p.Kill();
                    throw new Exception($"Error, possible endless loop! killing the subprocess after {WaitTimeoutInSeconds} seconds");
                }
                if (MaxLineCount <= 0)
                {
                    p.Kill();
                    throw new Exception("Error, possible endless loop! killing the subprocess after reading 1000 lines");
                }

                bool ProcessHasExited = p.WaitForExit(WaitTimeoutInSeconds * 1000); // at max 5 seconds!
                if (!ProcessHasExited)
                    throw new Exception($"Error in Method CallProcessAndReturnConsoleOutput! Process hasn't exited after {WaitTimeoutInSeconds} seconds!");
            }

            return Output;
        }

        private string GetPartOfStringBetween(string input, string beg, string end)
        {
            try
            {
                int startpos = input.IndexOf(beg);
                if (startpos == -1)
                    return "";
                int endpos = input.IndexOf(end, startpos + beg.Length + 1);
                if (endpos == -1)
                    return "";
                return input.Substring(startpos + beg.Length, endpos - startpos - beg.Length).Trim();
            }
            catch (Exception)
            {
                return "";
            }
        }
	}
}
