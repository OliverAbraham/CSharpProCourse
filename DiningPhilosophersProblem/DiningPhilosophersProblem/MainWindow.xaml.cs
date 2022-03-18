using System.Threading;
using System.Windows;

namespace DiningPhilosophersProblem
{
	public partial class MainWindow : Window
	{
        #region Fields
        private Philosopher[] _Philosophers;
        private Thread _StopperThread;
        private bool _Closing_is_in_progress;
        private bool _Closing_should_cancel;
        #endregion

        #region Initialisation
        public MainWindow()
        {
            InitializeComponent();
            Initialize_philosophers();
        }

        private void Initialize_philosophers()
        {
            _Philosophers = new Philosopher[5];
            _Philosophers[0] = new Philosopher(Dispatcher, Plate1, Fork5, Fork1);
            _Philosophers[1] = new Philosopher(Dispatcher, Plate2, Fork1, Fork2);
            _Philosophers[2] = new Philosopher(Dispatcher, Plate3, Fork2, Fork3);
            _Philosophers[3] = new Philosopher(Dispatcher, Plate4, Fork3, Fork4);
            _Philosophers[4] = new Philosopher(Dispatcher, Plate5, Fork4, Fork5);
        }
        #endregion

        #region Program shutdown
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_Closing_is_in_progress)
            {
                Stop_all_workers_wait_and_then_close(e);
                e.Cancel = true;
                _Closing_is_in_progress = true;
                _Closing_should_cancel = true;
            }
            else
            {
                e.Cancel = _Closing_should_cancel;
            }
        }

        private void Stop_all_workers_wait_and_then_close(System.ComponentModel.CancelEventArgs e)
        {
            Title = "Stopping the program...";
            _StopperThread = new Thread(new ParameterizedThreadStart(StopperThreadProcedure));
            _StopperThread.Start();
        }

        private void StopperThreadProcedure(object obj)
        {
            Stop_all_workers_and_wait();
            Close_program();
        }

        private void Stop_all_workers_and_wait()
        {
            foreach (var p in _Philosophers)
            {
                if (p != null) p.Stop();
            }
        }

        private void Close_program()
        {
            _Closing_should_cancel = false;
            Dispatcher.Invoke(() => Close() );
        }
        #endregion
	}
}
