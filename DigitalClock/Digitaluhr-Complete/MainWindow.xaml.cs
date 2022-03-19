using System;
using System.Windows;
using System.Windows.Threading;

namespace Digitaluhr_Complete
{
	public partial class MainWindow : Window
    {
        private DispatcherTimer _Timer;

        public MainWindow()
        {
            InitializeComponent();
            _Timer = new DispatcherTimer();
            _Timer.Tick += Timer_Tick;
            _Timer.Interval = new TimeSpan(hours:0, minutes:0, seconds:1);
            _Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var Now = DateTime.Now;
            Hour10  .Value = Now.Hour / 10;
            Hour1   .Value = Now.Hour % 10;
            Minute10.Value = Now.Minute / 10;
            Minute1 .Value = Now.Minute % 10;
        }
    }
}
