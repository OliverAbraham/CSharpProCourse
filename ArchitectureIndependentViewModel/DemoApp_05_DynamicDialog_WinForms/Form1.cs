using Abraham.UI;
using BusinessLogic;
using System;
using System.Windows.Forms;

namespace DemoApp_05_DynamicDialog_WinForms
{
    public partial class Form1 : Form
    {
        DynamicDialogWinFormsAdapter<MyViewModel> DynamicDialog;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 MyWindow = new Form2();
            MyViewModel VM = new MyViewModel();
            DynamicDialog = new DynamicDialogWinFormsAdapter<MyViewModel>(VM);
            DynamicDialog.DoModal(MyWindow);
        }
    }
}
