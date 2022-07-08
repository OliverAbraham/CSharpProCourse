using System;
using System.Collections.Generic;
using System.Globalization;

namespace AlexaHomeAutomationSkillAWSLambda
{
    public class AlexaFactResource
    {
        #region Types

        public class AlexaDate
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }

            public AlexaDate(DateTime date, string description)
            {
                Date = date;
                Description = description;
            }

            //public AlexaDate(string date, string description)
            //{
            //    Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            //    Description = description;
            //}

            public string Generate_natural_language()
            {
                return $"Am {Date.Day}ten {Date.Month}ten {Description}";
            }
        }

        #endregion

        #region Properties

        public string Language { get; set; }
        public string SkillName { get; set; }
        public List<string> Facts { get; set; }
        public List<AlexaDate> Dates { get; set; }
        public string GetFactMessage { get; set; }
        public string HelpMessage { get; set; }
        public string HelpReprompt { get; set; }
        public string StopMessage { get; set; }

        #endregion

        #region Constructors

        public AlexaFactResource(string language)
        {
            this.Language = language;
            this.Facts = new List<string>();
            this.Dates = new List<AlexaDate>();
        }

        #endregion

        #region Methods
        #endregion

        #region Implementation
        #endregion
    }
}
