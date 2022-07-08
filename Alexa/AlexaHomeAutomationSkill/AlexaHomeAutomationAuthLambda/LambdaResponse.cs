﻿using System.Collections.Generic;
using System.Net;

namespace AlexaHomeAutomationAuthLambda
{
    internal class LambdaResponse
    {
        public LambdaResponse()
        {
        }

        public string Body { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}