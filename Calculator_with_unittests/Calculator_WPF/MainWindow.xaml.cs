using BusinessLogic;
using Contracts;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using UserInterfaceLogic;

namespace Calculator_WPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string DisplayValue { get; set; }

        private static ICalculator _engine;
        private static CalculatorUI _ui;

        public MainWindow()
        {
            _engine = new Calculator();
            _ui = new CalculatorUI(_engine); // <--- Dependency Injection
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var key = (sender as Button).Tag as string;
            DisplayValue = _ui.Process_key_pressure_and_return_new_display_text(key);
            NotifyPropertyChanged(nameof(DisplayValue));
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
