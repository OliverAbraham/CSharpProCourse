using System;
using System.Windows;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int value1 = Convert.ToInt32(operand1.Text);
            int value2 = Convert.ToInt32(operand2.Text);

            int resultValue = value1 + value2;

            result.Text = resultValue.ToString();
        }
    }
}
