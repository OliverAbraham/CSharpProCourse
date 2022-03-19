using	Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace RichTextBox
{
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public string MyText1 { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			DataContext = this;
			MyText1 = "Text oder nicht sein, das ist hier die Frage!";
		}

		private void DateiLaden_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new CommonOpenFileDialog();
			//dialog.IsFolderPicker = true;
			dialog.DefaultDirectory = "C:\\";
			var result = dialog.ShowDialog();
			BringIntoView();
			if (result == CommonFileDialogResult.Ok)
			{
				MyText1 = File.ReadAllText(dialog.FileName);
				NotifyPropertyChanged(nameof(MyText1));
			}
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
