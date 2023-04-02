using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorTemplateForKata
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region ------------- Properties ----------------------------------------------------------
        public string DisplayValue { get; set; }
        #endregion

        #region ------------- Init ----------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }
        #endregion

        #region ------------- Implementation ------------------------------------------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var key = (sender as Button).Tag as string;
            DisplayValue = $"You have pressed {key}";
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
        #endregion
    }
}
