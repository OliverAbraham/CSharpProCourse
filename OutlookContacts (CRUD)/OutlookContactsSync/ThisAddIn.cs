using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;

namespace OutlookContactsSync
{
    public partial class ThisAddIn
    {
        public static string Title { get; internal set; }
        public static ThisAddIn Current => _currentInstance;
        private static ThisAddIn _currentInstance;


        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _currentInstance = this;
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Hinweis: Outlook löst dieses Ereignis nicht mehr aus. Wenn Code vorhanden ist, der 
            //    muss ausgeführt werden, wenn Outlook heruntergefahren wird. Weitere Informationen finden Sie unter https://go.microsoft.com/fwlink/?LinkId=506785.
        }

        #region Von VSTO generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        

        public void CreateAContact()
        {
            var contact                       = Application.CreateItem(Outlook.OlItemType.olContactItem) as Outlook.ContactItem;
            contact.FirstName                 = "Mellissa";
            contact.LastName                  = "MacBeth";
            contact.JobTitle                  = "Account Representative";
            contact.CompanyName               = "Contoso Ltd.";
            contact.OfficeLocation            = "36/2529";
            contact.BusinessTelephoneNumber   = "+49 171 4255551212";
            contact.WebPage                   = "https://www.contoso.com";
            contact.BusinessAddressStreet     = "1 Microsoft Way";
            contact.BusinessAddressCity       = "Redmond";
            contact.BusinessAddressState      = "WA";
            contact.BusinessAddressPostalCode = "98052";
            contact.BusinessAddressCountry    = "United States of America";
            contact.Email1Address             = "melissa@contoso.com";
            contact.Email1AddressType         = "SMTP";
            contact.Email1DisplayName         = "Melissa MacBeth (mellissa@contoso.com)";
            //contact.Display(false);
            //contact.ShowCheckPhoneDialog(Outlook.OlContactPhoneNumber.olContactPhoneBusiness);
            contact.Save();
        }

        internal void UpdateContact()
        {
            Outlook.Application app = new Outlook.Application();
            var ns = app.GetNamespace("MAPI");

            // Get the user's default contacts folder
            var contactsFolder = ns.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts);
            var contacts = contactsFolder.Items;

            //var contact  = contacts.Where(x => x.Name == "Mellissa").FirstOrDe
        }

        internal void DeleteContact()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
