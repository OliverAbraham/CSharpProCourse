using Geschäftslogik;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Taschenrechner
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string Anzeigewert { get; set; }

        private static ICalculator CalculatorEngine;
        private static CalculatorUI Ui;

        public MainWindow()
        {
            CalculatorEngine = new Calculator();
            Ui = new CalculatorUI(CalculatorEngine); // <--- Dependency Injection
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string taste = (sender as Button).Tag as string;
            Anzeigewert = Ui.Process_key_pressure_and_return_new_display_text(taste);
            NotifyPropertyChanged("Anzeigewert");
        }

        #region ------------- INotifyPropertyChanged ----------------------------------------------

        private PropertyChangedEventHandler propertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        public void NotifyPropertyChanged(String info)
        {
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(info));
        }

        #endregion
    }
}
