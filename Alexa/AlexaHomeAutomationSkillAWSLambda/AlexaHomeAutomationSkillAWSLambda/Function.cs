using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Alexa.NET.Response;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Newtonsoft.Json;
using System.Globalization;
using RestSharp;
using REST_API_Request;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaHomeAutomationSkillAWSLambda
{
    public class Function
    {
        private static List<AlexaFactResource> allResources = null;
        private static int _CurrentDate = 1;

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;
            log.LogLine($"Skill Request Object:");
            log.LogLine(JsonConvert.SerializeObject(input)); 

            if (allResources == null)
                allResources = GetResources(log);
            var resource = allResources.FirstOrDefault();

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, open LentfördenFacts");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = emitNewFact(resource, true);
            }

            else if (input.GetRequestType() == typeof(IntentRequest))
            {
               var intentRequest = (IntentRequest)input.Request;
               switch (intentRequest.Intent.Name)
               {
                  case "AMAZON.CancelIntent":
                     log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = resource.StopMessage;
                     response.Response.ShouldEndSession = true;
                     break;
                  case "AMAZON.StopIntent":
                     log.LogLine($"AMAZON.StopIntent: send StopMessage");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = resource.StopMessage;
                     response.Response.ShouldEndSession = true;
                     break;
                  case "AMAZON.HelpIntent":
                     log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = resource.HelpMessage;
                     break;
                  case "GetFactIntent":
                     log.LogLine($"GetFactIntent sent: send new fact");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = emitNewFact(resource, false);
                     break;
                  case "GetNewFactIntent":
                     log.LogLine($"GetFactIntent sent: send new fact");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = emitNewFact(resource, false);
                     break;
                  case "GetDatesIntent":
                     log.LogLine($"GetDatesIntent sent: send new date");
                     innerResponse = new PlainTextOutputSpeech();
                     (innerResponse as PlainTextOutputSpeech).Text = emitNewDate(resource);
                     break;
                  default:
                     log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                     innerResponse = new PlainTextOutputSpeech();
                    (innerResponse as PlainTextOutputSpeech).Text = resource.HelpReprompt;
                    break;
               }
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            log.LogLine($"Skill Response Object...");
            log.LogLine(JsonConvert.SerializeObject(response));
            return response;
        }

        public List<AlexaFactResource> GetResources(ILambdaLogger logger)
        {
            logger.LogLine($"GetResources");

            List<AlexaFactResource> resources = new List<AlexaFactResource>();
            AlexaFactResource deDEResource = new AlexaFactResource("de-DE");
            deDEResource.SkillName      = "Lentföhrden Fakten";
            deDEResource.GetFactMessage = "Hier sind aktuelle Termine: ";
            deDEResource.HelpMessage    = "Du kannst zu mir sagen: Nächste Termine, Erzähl mir was über Lentföhrden, oder Du kannst sagen: Ende. Was soll ich tun?";
            deDEResource.HelpReprompt   = "Was soll ich tun?";
            deDEResource.StopMessage    = "Bis bald!";
            deDEResource.Facts.Add("Dies ist die Alexa App der Gemeinde Lentföhrden in Schleswig-Holstein. Sage zum Beispiel nächste Termine, um aktuelles aus dem Dorf zu erfahren.");
            deDEResource.Facts.Add("Du kannst auch sagen: Alexa frag Lentföhrden nach Terminen.");
            //deDEResource.Facts.Add("Der Verein Kola kümmert sich viel um Kinder.");
            //deDEResource.Facts.Add("Lentföhrden Open Air organisiert jedes Jahr einen Event mit Rock- und Popmusik.");
            //deDEResource.Facts.Add("Die Lentertainer treffen sich jeden Montag zum Singen.");
            //deDEResource.Facts.Add("Die Feuerwehr bekommt wahrscheinlich bald ein neues Gerätehaus.");
            //deDEResource.Facts.Add("Der Förderverein Freibad kümmert sich um den Erhalt unseres Freibads.");
            //deDEResource.Facts.Add("Der Angelverein zieht immer dicke Dinger ausm Teich.");
            //deDEResource.Facts.Add("Der TSV bietet unserem Nachwuchs Fußball und Handball.");
            //deDEResource.Facts.Add("Es gibt drei Parteien in der Gemeinde: Die Wählergemeinschaft, die SPD und die CDU.");
            //deDEResource.Facts.Add("Die Leseratten treffen sich regelmäßig im Kulturzentrum.");
            //deDEResource.Facts.Add("Die Wollmäuse machen im Kulturzentrum regelmäßig Handarbeiten.");
            //deDEResource.Facts.Add("Der Seniorenclub Lentförden bietet immer wieder Aktivitäten für unsere Älteren.");
            //deDEResource.Facts.Add("Das waren die Fakten. Sage zum Beispiel nächste Termine, um aktuelles zu erfahren.");

            logger.LogLine($"GetResources: Loaded {deDEResource.Facts.Count} facts");
            


            // REST-Endpunkt der Gemeinde-Homepage abfragen (Wordpress)
            string Url = "http://lentfoehrden.de";
            logger.LogLine($"GetResources: Querying Wordpress REST API for '{Url}'");
            var WordpressQueryClient = new RestClient(Url);
            if (WordpressQueryClient == null)
                logger.LogLine($"WordpressQueryClient NOT created!");
            // Wir brauchen keine Authentifizierung, weil wir öffentliche Daten abfragen
            // client.Authenticator = new HttpBasicAuthenticator(username, password);
            
            if (WordpressQueryClient != null)
            {
                // Wir sprechen die REST-API von Wordpress an, um eine festgelegte Seite zu holen
                var Request = new RestRequest("/wp-json/wp/v2/pages/71", Method.Get);
                if (Request == null)
                    logger.LogLine($"WordpressQueryClient Request unsuccesful!");
                else
                {
                    RestResponse<WordpressPageResponse> Response = WordpressQueryClient.Execute<WordpressPageResponse>(Request);
                    if (Response == null)
                        logger.LogLine($"WordpressQueryClient Execute unsuccesful!");
                    else
                    {
                        var ContentAsString = Response?.Data?.content;
                        if (ContentAsString == null || ContentAsString.rendered == null)
                            logger.LogLine($"WordpressQueryClient ContentAsString unsuccesful!");
                        else
                        {
                            logger.LogLine($"WordpressQueryClient ContentAsString = '{ContentAsString.rendered}'");
                            var DateCollection = AlexaDate.DeserializeContent(ContentAsString.rendered);
                            if (DateCollection == null)
                                logger.LogLine($"WordpressQueryClient DateCollection is null!");
                            else
                            {
                                logger.LogLine($"Loaded Dates: {DateCollection.Count}");
                                foreach (var Date in DateCollection)
                                {
                                    logger.LogLine($"Date: {Date.Datum} {Date.Terminbezeichnung}");
                                    deDEResource.Dates.Add(new AlexaFactResource.AlexaDate(Date.Datum, Date.Terminbezeichnung));
                                }
                            }
                        }
                    }
                }
            }

            resources.Add(deDEResource);
            return resources;
        }

        public string emitNewFact(AlexaFactResource resource, bool withPreface)
        {
              Random r = new Random();
              if (withPreface)
                 return resource.GetFactMessage + resource.Facts[r.Next(resource.Facts.Count)];
              return resource.Facts[r.Next(resource.Facts.Count)];
        }

        public string emitNewDate(AlexaFactResource resource)
        {
            if (_CurrentDate > resource.Dates.Count)
                _CurrentDate = 1;

            int SkipDate = 1;
            foreach(var Entry in resource.Dates)
            {
                if (Entry.Date.Date >= DateTime.Now.Date)
                {
                    if (SkipDate < _CurrentDate)
                    {
                        SkipDate++;
                    }
                    else
                    {
                         _CurrentDate++;
                        return Entry.Generate_natural_language();

                    }
                }
            }
            return null;
        }
    }
}
