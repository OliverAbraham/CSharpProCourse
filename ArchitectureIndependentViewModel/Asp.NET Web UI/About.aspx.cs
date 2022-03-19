using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Asp.NET_Web_UI
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "Save button clicked!";
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TextBox1.Text = "Cancel button clicked!";
        }
    }
}