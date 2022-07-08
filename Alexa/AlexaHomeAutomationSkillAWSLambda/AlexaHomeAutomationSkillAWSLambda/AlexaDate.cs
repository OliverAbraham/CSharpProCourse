using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_API_Request
{
    class AlexaDate
    {
        private const int MAX_TEXT_LENGTH = 200;

        /// <summary>
        /// obligatorisch
        /// </summary>
        public DateTime Datum { get; set; }
        
        /// <summary>
        /// obligatorisch
        /// </summary>
        public string Terminbezeichnung { get; set; }
        
        /// <summary>
        /// optional
        /// </summary>
        public string Veranstalter { get; set; }
        
        /// <summary>
        /// optional
        /// </summary>
        public DateTime Uhrzeit { get; set; }
        
        /// <summary>
        /// optional
        /// </summary>
        public string Ort { get; set; }
        
        public bool IsValid 
        { 
            get
            {
                return Datum.Year >= 1900 && Terminbezeichnung.Length > 0;
            }
        }

        public static List<AlexaDate> DeserializeContent(string contentAsString)
        {
            List<AlexaDate> DateCollection = new List<AlexaDate>();

            try
            {
                string[] Lines = contentAsString.Split(new char[]{ '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (Lines.GetLength(0) > 0)
                {
                    foreach (string Line in Lines)
                    {
                        string Line2 = Remove_formatting(Line);
                        if (!IsCommentLine(Line2))
                        {
                            AlexaDate Date = DeserializeDate(Line2);
                            if (Date.IsValid)
                                DateCollection.Add(Date);
                        }
                    }
                }
            }
            catch (Exception)
            {}

            return DateCollection;
        }

        private static AlexaDate DeserializeDate(string line)
        {
            AlexaDate Date = new AlexaDate();

            try
            {
                string[] Fields = line.Split(new char[] { '|' }, StringSplitOptions.None);
                int FieldCount = Fields.GetLength(0);

                if (FieldCount >= 2) // don't allow only one field!
                    Date.Datum             = Deserialize_date_field    (Fields[1 - 1]);
                if (FieldCount >= 2)
                    Date.Terminbezeichnung = Deserialize_string_field  (Fields[2 - 1]);
                if (FieldCount >= 3)
                    Date.Veranstalter      = Deserialize_string_field  (Fields[3 - 1]);
                if (FieldCount >= 4)
                    Date.Uhrzeit           = Deserialize_time_field    (Fields[4 - 1]);
                if (FieldCount >= 5)
                    Date.Ort               = Deserialize_string_field  (Fields[5 - 1]);
            }
            catch (Exception)
            {}

            return Date;
        }

        private static string Remove_formatting(string line)
        {
            line = line.Replace("<p>", "");
            line = line.Replace("</p>", "");
            line = line.Trim();
            return line;
        }

        private static bool IsCommentLine(string line)
        {
            return line.StartsWith("--") || 
                   line.StartsWith("&#8212;");
        }

        private static DateTime Deserialize_date_field(string field)
        {
            try
            {
                var Value = DateTime.ParseExact(field, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                return new DateTime(Value.Year, Value.Month, Value.Day, 0, 0, 0);
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        private static DateTime Deserialize_time_field(string field)
        {
            try
            {
                var Value = DateTime.ParseExact(field, "hh.mm.ss", CultureInfo.InvariantCulture);
                return new DateTime(1900, 1, 1, Value.Hour, Value.Minute, 0);
            }
            catch (Exception)
            {
                return new DateTime();
            }
        }

        private static string Deserialize_string_field(string field)
        {
            if (field.Length <= MAX_TEXT_LENGTH)
            {
                return field;
            }
            else
            {
                return "";
            }
        }
    }
}
