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
            int Operand1 = Convert.ToInt32(operand1.Text);
            int Operand2 = Convert.ToInt32(operand2.Text);

            int Ergebnis = Operand1 + Operand2;

            ergebnis.Text = Ergebnis.ToString();
        }
    }
}
