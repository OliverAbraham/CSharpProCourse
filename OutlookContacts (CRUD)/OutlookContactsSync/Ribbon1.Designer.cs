
using Microsoft.Office.Tools.Ribbon;
using System;

namespace OutlookContactsSync
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen, andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.btnCreateContact = this.Factory.CreateRibbonButton();
            this.btnUpdateContact = this.Factory.CreateRibbonButton();
            this.btnDeleteContact = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.btnCreateContact);
            this.group1.Items.Add(this.btnUpdateContact);
            this.group1.Items.Add(this.btnDeleteContact);
            this.group1.Label = "Outlook Contacts Demo";
            this.group1.Name = "group1";
            // 
            // btnSettings
            // 
            this.btnCreateContact.Label = "Create Contact";
            this.btnCreateContact.Name = "btnCreateContact";
            this.btnCreateContact.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCreateContact_Click);
            // 
            // btnSettings
            // 
            this.btnUpdateContact.Label = "Update Contact";
            this.btnUpdateContact.Name = "btnUpdateContact";
            this.btnUpdateContact.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnUpdateContact_Click);
            // 
            // btnSettings
            // 
            this.btnDeleteContact.Label = "Delete Contact";
            this.btnDeleteContact.Name = "btnDeleteContact";
            this.btnDeleteContact.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteContact_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateContact;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnUpdateContact;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteContact;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
