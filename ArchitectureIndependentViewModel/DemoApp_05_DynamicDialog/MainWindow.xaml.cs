using Abraham.UI;
using BusinessLogic;
using System.Windows;

namespace DemoApp_05_DynamicDialog
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DynamicDialogWpfAdapter<MyViewModel> DynamicDialog;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyViewModel VM = new MyViewModel();
            DynamicDialog = new DynamicDialogWpfAdapter<MyViewModel>(VM);

            BaseWindow MyWindow = new BaseWindow();
            MyWindow.DataContext = VM;
            DynamicDialog.DoModal(MyWindow);
        }
    }
}
