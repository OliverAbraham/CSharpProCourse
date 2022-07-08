using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abraham.Threading
{
    public class ThreadExtensions
    {
        #region ------------- Eigenschaften -------------------------------------------------------

        public Thread thread { get; set; }

        public int Timeout_Seconds { get; set; }

        /// <summary>
        /// The Thread Procedure should check this flag often and stop working if false
        /// </summary>
        public volatile bool Run;

        #endregion



        #region ------------- Felder --------------------------------------------------------------
        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public ThreadExtensions(ThreadStart threadProc, string name = "MyThread")
        {
            thread = new Thread(threadProc);
            thread.Name = name;
            Timeout_Seconds = 1;
            Run = true;
        }

        public ThreadExtensions(ParameterizedThreadStart threadProc, string name = "MyThread")
        {
            thread = new Thread(threadProc);
            thread.Name = name;
            Timeout_Seconds = 1;
            Run = true;
        }

        #endregion



        #region ------------- Methoden ------------------------------------------------------------

        public void SendStopSignalAndWait()
        {
            Run = false;

            int i = 0;
            while (thread.IsAlive && i < (10 * Timeout_Seconds))
            {
                Thread.Sleep(100);
                i++;
            }

            if (thread.IsAlive)
                thread.Abort();
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------
        #endregion
    }
}
