using System.Timers;
using System.Windows;

namespace BusyIndicator
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Aus_Click(object sender, RoutedEventArgs e)
		{
			_myBusyIndicator.IsBusy = false;
		}

		private void Button_An_Click(object sender, RoutedEventArgs e)
		{
			_myBusyIndicator.IsBusy = true;

            var myTimer = new Timer();
            myTimer.Interval = 3 * 1000;
            myTimer.Elapsed += 
                delegate(object sender2, ElapsedEventArgs e2)
                {
                    Dispatcher.Invoke(() => { _myBusyIndicator.IsBusy = false; });
                };

            myTimer.Start();
		}
	}
}
