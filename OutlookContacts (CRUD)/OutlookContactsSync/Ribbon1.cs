using Microsoft.Office.Tools.Ribbon;
using System;
using System.Windows.Forms;

namespace OutlookContactsSync
{
    public partial class Ribbon1
    {
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
        }

        private void btnCreateContact_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.Current.CreateAContact();
            MessageBox.Show($"A new contact was created!", ThisAddIn.Title);
        }

        private void btnUpdateContact_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.Current.UpdateContact();
            MessageBox.Show($"The contact was updated!", ThisAddIn.Title);
        }

        private void btnDeleteContact_Click(object sender, RibbonControlEventArgs e)
        {
            ThisAddIn.Current.DeleteContact();
            MessageBox.Show($"The contact was deleted!", ThisAddIn.Title);
        }
    }
}
