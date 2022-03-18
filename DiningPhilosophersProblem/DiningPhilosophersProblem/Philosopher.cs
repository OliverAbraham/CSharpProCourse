using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace DiningPhilosophersProblem
{
    internal enum ActionState
    {
        Thinking,
        Eating,
        Sleeping
    }

    internal class Philosopher
    {
        #region Resources
        private Button      Plate;
        private Button      Left_fork;
        private Button      Right_fork;
        #endregion

        #region Working stuff
        private ActionState State;
        private Thread      WorkerThread;
        private bool        WorkerShouldStop;
        private bool        WorkerHasStopped;
        private Dispatcher  Dispatcher;
        private Random      RandomTimeGenerator;
        #endregion

        #region Construction
        public Philosopher(Dispatcher dispatcher, Button plate, Button left_fork, Button right_fork)
        {
            Dispatcher          = dispatcher;
            Plate               = plate;
            Left_fork           = left_fork;
            Right_fork          = right_fork;

            RandomTimeGenerator = new Random((int)DateTime.Now.Ticks);
            WorkerShouldStop    = false;
            WorkerHasStopped    = false;
            WorkerThread        = new Thread(new ParameterizedThreadStart(ThreadProcedure));
            WorkerThread.Start();
        }
        #endregion

        #region Methods
        internal void Stop()
        {
            Tell_worker_to_stop();
            Wait_until_worker_has_stopped();
        }
        #endregion

        #region Implementation
        private void ThreadProcedure(object obj)
        {
            while(true)
            {
                Think();
                if (WorkerShouldStop) break;

                Eat();
                if (WorkerShouldStop) break;

                Sleep();
                if (WorkerShouldStop) break;
            }

            WorkerHasStopped = true;
        }

        private void Tell_worker_to_stop()
        {
            WorkerShouldStop = true;
        }

        private void Wait_until_worker_has_stopped()
        {
            while (!WorkerHasStopped)
            {
                Thread.Sleep(100);
            }
        }

        private void Wait()
        {
            int milliseconds = RandomTimeGenerator.Next(minValue: 2000, maxValue:5000);
            Thread.Sleep(milliseconds);
        }

        private void Think()
        {
            State = ActionState.Thinking;
            Dispatcher.Invoke(() => 
            {
                Plate.Content = "T";
                Plate.Background = Brushes.LightGreen;
            });
            Wait();
        }

        private void Sleep()
        {
            State = ActionState.Sleeping;
            Dispatcher.Invoke(() => 
            {
                Plate.Content = "S";
                Plate.Background = Brushes.Gray;
            });
            Wait();
        }

        private void Eat()
        {
            Try_to_grab_both_forks();
            Start_eating();
            Wait();
            Release_forks();
        }

        private void Try_to_grab_both_forks()
        {
            Wait_while_a_fork_is_in_use();
            Grab_both_forks();
        }

        private void Wait_while_a_fork_is_in_use()
        {
            while (ForkIsInUse(Left_fork) ||
                   ForkIsInUse(Right_fork))
            {
                Thread.Sleep(100);
                if (WorkerShouldStop) 
                    break;
            }
        }

        private void Grab_both_forks()
        {
            Dispatcher.Invoke(() => 
            {
                Left_fork .Background = Brushes.Red;
                Right_fork.Background = Brushes.Red;
            });
        }

        private void Release_forks()
        {
            Dispatcher.Invoke(() => 
            {
                Left_fork .Background = Brushes.LightGray;
                Right_fork.Background = Brushes.LightGray;
            });
        }

        private bool ForkIsInUse(Button fork)
        {
            bool Result = false;
            Dispatcher.Invoke(() => 
            {
                Result = fork.Background == Brushes.Red;
            });
            return Result;
        }

        private void Start_eating()
        {
            State = ActionState.Eating;
            Dispatcher.Invoke(() => 
            {
                Plate.Content = "E";
                Plate.Background = Brushes.Red;
            });
        }
        #endregion
   }
}