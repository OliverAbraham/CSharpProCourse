using System;
using System.Windows;

namespace PasswordGenerator
{
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            labelCharacterset.Content = "";
            if (CheckBox1.IsChecked == true)
                labelCharacterset.Content += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (CheckBox2.IsChecked == true)
                labelCharacterset.Content += "abcdefghijklmnopqrstuvwxyz";
            if (CheckBox3.IsChecked == true)
                labelCharacterset.Content += "0123456789";
            if (CheckBox4.IsChecked == true)
                labelCharacterset.Content += "<>|,;.:-_#'+*~´^°!\"§$%&/()=?´`\\";
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
		{
			var password = GeneratePassword();
			Result.Text = password;
		}

		private string GeneratePassword()
		{
			var characterSet = (string)labelCharacterset.Content;
			if (string.IsNullOrWhiteSpace(characterSet))
				return "";

			int length;
			if (!int.TryParse(TextboxLength.Text, out length))
				return "";

			var password = "";
			for (int i = 0; i < length; i++)
			{
				var randomNumberGenerator = new Random((int)DateTime.Now.Ticks);
				int randomNumber = randomNumberGenerator.Next(0, characterSet.Length - 1);
				password += characterSet[randomNumber];
			}
			return password;
		}

		private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(Result.Text);
		}
	}
}
