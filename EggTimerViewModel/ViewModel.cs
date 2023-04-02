using System.ComponentModel;

namespace EggTimerViewModel
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region ------------- Properties ----------------------------------------------------------
        public string CurrentTime
        {
            get { return _seconds.ToString(@"mm\:ss"); }
        }

        public TimeSpan Seconds 
        {
            get { return _seconds; }
            set { _seconds = value; NotifyPropertyChanged(nameof(CurrentTime)); }
        }

        public string StartButtonContent
        {
            get { return _startButtonContent; }
            set { _startButtonContent = value; NotifyPropertyChanged(nameof(StartButtonContent)); }
        }

        public string StartButtonBackground
        {
            get { return _startButtonBackground; }
            set { _startButtonBackground = value; NotifyPropertyChanged(nameof(StartButtonBackground)); }
        }

        public bool IsRunning => _running;
        public Action PlaySound  { get; set; }
        public Action StartTimer { get; set; }
        public Action StopTimer  { get; set; }
        #endregion

        #region ------------- Fields --------------------------------------------------------------
        private TimeSpan _seconds = TimeSpan.FromSeconds(5);
        private string _startButtonContent = "Start";
        private string _startButtonBackground = "#FF90EE90";
        private bool _running;
        #endregion

        #region ------------- Methods -------------------------------------------------------------
        public void PreselectTime(string buttonText)
        { 
            var newValue = Convert.ToInt32(buttonText) * 60;
            Seconds = TimeSpan.FromSeconds(newValue);
        }

        public void Button_Start_Click()
        {
            if (!_running)
                Start();
            else
                Stop();
        }

        public void Count()
        {
            Seconds = Seconds.Add(TimeSpan.FromSeconds(-1));
            if (Seconds.TotalSeconds == 0)
            {
                Stop();
                PlaySound();
            }
        }
        #endregion

        #region ------------- Implementation ------------------------------------------------------
        private void Start()
        {
            _running = true;
            StartTimer();
            StartButtonContent = "Stop";
            StartButtonBackground = "#FFFFA07A";
        }

        private void Stop()
        {
            _running = false;
            StopTimer();
            StartButtonContent = "Start";
            StartButtonBackground = "#FF90EE90";
        }
        #endregion

        #region ------------- INotifyPropertyChanged ---------------------------
        [NonSerialized]
        private PropertyChangedEventHandler? _propertyChanged;
 
        public event PropertyChangedEventHandler? PropertyChanged 
        {
            add 
            {
                _propertyChanged += value;
            }
            remove 
            {
                _propertyChanged -= value;
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            var handler = _propertyChanged; // avoid race condition
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion       
    }
}