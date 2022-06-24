using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace BruteForceAttacks
{
	public partial class MainWindow : Window
	{
		private Thread _thread;
		private volatile bool _run;
		private Stopwatch _updateStopwatch;
		private Stopwatch _searchStopwatch;
		private string _PasswordToFind;
		private string _charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜabcdefghijklmnopqrstuvwxyzäöüß0123456789!\"§$%&/()=?,;.:-_#'+*~^<>|";
		private int _maxLength;

		public MainWindow()
		{
			InitializeComponent();
			entryfieldCharset.Text = _charset;
		}

		private void Button_Start_Click(object sender, RoutedEventArgs e)
		{
			_updateStopwatch = Stopwatch.StartNew();
			_searchStopwatch = Stopwatch.StartNew();
			_charset        = entryfieldCharset.Text;
			_PasswordToFind = entryfieldPassword.Text;
			_run = true;
			_thread = new Thread(new ThreadStart(ThreadProc));
			_thread.Start();
		}

		private void Button_Stop_Click(object sender, RoutedEventArgs e)
		{
			_run = false;
		}

		private void ThreadProc()
		{
			_maxLength = 1;
			string password = _charset[0].ToString();
			while (_run)
			{
				Dispatcher.Invoke(() => 
				{ 
					labelLength.Content = _maxLength.ToString();
				});

				IterateOneDigit("", _maxLength);
				_maxLength++;
			}
		}

		private void IterateOneDigit(string part, int length)
		{
			if (length == 0)
			{
				Try(part);
				return;
			}

			for (int i = 0; i < _charset.Length && _run; i++)
			{
				char c = _charset[i];
				IterateOneDigit(part + c.ToString(), length-1);
			}
		}

		private void Try(string v)
		{
			if (v == _PasswordToFind)
			{
				DisplayDuration();
				_run = false;
				return;
			}

			// Update UI only every second
			if (_updateStopwatch.ElapsedMilliseconds < 1000)
				return;
			_updateStopwatch.Restart();

			DisplayCurrentPasswort(v);
		}

		private void DisplayCurrentPasswort(string v)
		{
			try
			{
				Dispatcher.Invoke(() =>
				{
					labelCurrentPassword.Content = v;
					labelDuration.Content = _searchStopwatch.Elapsed.ToString(@"hh\:mm\:ss");

					if (!string.IsNullOrWhiteSpace(v))
						entryfieldCharset.Select(entryfieldCharset.Text.IndexOf(v[0]), 1);
					entryfieldCharset.SelectionLength = 1;
				});
			}
			catch (Exception)
			{
			}
		}

		private void DisplayDuration()
		{
			Dispatcher.Invoke(() =>
			{
				labelCurrentPassword.Content = $"Passwort gefunden nach {_searchStopwatch.Elapsed.ToString(@"hh\:mm\:ss")}!";
			});
		}
	}
}
