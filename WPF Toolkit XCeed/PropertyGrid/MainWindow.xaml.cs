using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace WPF_Toolkit_XCeed
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public string MeineTextProperty { get; set; }
		public int MeineIntProperty { get; set; }
		public Color MeineFarbProperty { get; set; }
		public Brush MeineFarbAnzeige { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			_propertyGrid.SelectedObject = this;

			MeineTextProperty = "Audi";
			MeineIntProperty = 4711;
			MeineFarbAnzeige = Brushes.LightGreen;
			NotifyPropertyChanged(nameof(MeineFarbAnzeige));
		}

		private void _propertyGrid_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			NotifyPropertyChanged(nameof(MeineTextProperty));
			NotifyPropertyChanged(nameof(MeineIntProperty));

			// Das PropertyGrid braucht eine "Color", aber der Hintergrund 
			// von meinem "TextBlock" braucht einen "Brush"
			MeineFarbAnzeige = new SolidColorBrush(MeineFarbProperty);
			NotifyPropertyChanged(nameof(MeineFarbAnzeige));
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
