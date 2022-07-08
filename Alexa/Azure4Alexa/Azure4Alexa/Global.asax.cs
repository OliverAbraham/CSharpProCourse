using Azure4Alexa.HomeAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Azure4Alexa
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            this.Disposed -= WebApiApplication_Disposed;
            this.Disposed += WebApiApplication_Disposed;
            HomeAutomationGateway.Instance.Start();
        }

        private void WebApiApplication_Disposed(object sender, EventArgs e)
        {
            HomeAutomationGateway.Instance.Shutdown();
        }
    }
}
