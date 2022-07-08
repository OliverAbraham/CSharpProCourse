using Azure4Alexa.HomeAutomation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;

namespace Azure4Alexa.Controllers
{
    public class AlexaController : ApiController
    {
        public AlexaController()
        {
            System.Diagnostics.Debug.WriteLine("AlexaController started.");
        }

        // ABRAHAM hier kucken:
        // https://stackoverflow.com/questions/15718741/405-method-not-allowed-web-api

        // you can set an explicit route if you want ...
        // [Route("alexa/alexa-session")]
        [HttpPost]
        public async Task<HttpResponseMessage> AlexaSession()
        {
            var alexaSpeechletAsync = new Alexa.AlexaSpeechletAsync();
            return await alexaSpeechletAsync.GetResponseAsync(Request);
        }

        [Route("an")]
        [HttpGet]
        public async Task<HttpResponseMessage> TestAn()
        {
            HomeAutomationGateway.Instance.Switch_data_object("AZ_DECKENLAMPE", "1");
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent("<html><head></head><body>HTTP GET</body></html>", Encoding.UTF8, "text/html");
            return httpResponse;
        }

        [Route("aus")]
        [HttpGet]
        public async Task<HttpResponseMessage> TestAus()
        {
            HomeAutomationGateway.Instance.Switch_data_object("AZ_DECKENLAMPE", "0");
            HttpResponseMessage httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent("<html><head></head><body>HTTP GET</body></html>", Encoding.UTF8, "text/html");
            return httpResponse;
        }

    }
}

