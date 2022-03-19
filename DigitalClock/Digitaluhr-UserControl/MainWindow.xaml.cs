using System;
using System.Windows;
using System.Windows.Threading;

namespace Digitaluhr_UserControl
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _Timer;
        private int _Digit = 0;

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
            MyControl1.Value = _Digit;
            _Digit = (++_Digit) % 10;
        }
    }
}
