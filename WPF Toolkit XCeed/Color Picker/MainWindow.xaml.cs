using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Color_Picker
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{

		public Color MeineFarbe { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
		}

		private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			NotifyPropertyChanged(nameof(MeineFarbe));
		}


		#region ------------- INotifyPropertyChanged ---------------------------

		[NonSerialized]
		private PropertyChangedEventHandler _PropertyChanged;
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				_PropertyChanged += value;
			}
			remove
			{
				_PropertyChanged -= value;
			}
		}

		public void NotifyPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler Handler = _PropertyChanged; // avoid race condition
			if (Handler != null)
				Handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
