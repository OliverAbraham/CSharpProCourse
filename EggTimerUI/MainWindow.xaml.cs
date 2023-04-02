using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using EggTimerViewModel;

namespace EggTimerUI
{
    public partial class MainWindow : Window
    {
        #region ------------- Fields --------------------------------------------------------------
        private DispatcherTimer _timer;
        private ViewModel _viewModel;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public MainWindow()
        {
            _timer = SetupTimer();
            _viewModel = new ViewModel();
            _viewModel.StartTimer = delegate() { _timer.Start(); };
            _viewModel.StopTimer  = delegate() { _timer.Stop(); };
            _viewModel.PlaySound  = delegate() { PlaySound(); };
            InitializeComponent();
            DataContext = _viewModel;
        }
        #endregion



        #region ------------- Implementation ------------------------------------------------------
        private void Button_Preselect_Click(object sender, RoutedEventArgs e)
        { 
            if (sender is System.Windows.Controls.Button myButton)
            {
                var buttonText = (myButton.Tag as string) ?? "0";
                _viewModel.PreselectTime(buttonText);
            }
        }

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Button_Start_Click();
        }

        private void PlaySound()
        {
             Uri uri = new Uri(System.Environment.CurrentDirectory + @"\dampflok.mp3");
             var player = new MediaPlayer();
             player.Open(uri);
             player.Play();
        }

        private DispatcherTimer SetupTimer()
        {
            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += delegate(object? sender, EventArgs e) { _viewModel.Count(); };
            return timer;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer.Stop();
        }
        #endregion
    }
}
