using System;
using System.Collections.Generic;


namespace Abraham.String
{
    /// <summary>
    /// Regelt, wie die String-Funktionen Text aus Strings ausschneiden und wie sie mit Zeilenendn umgehen.
    /// </summary>
    public enum TextFetchMode
    {
        /// <summary>The start token will be ignored</summary>
        WithoutBeginning,
        /// <summary>The start token will be taken</summary>
        WithBeginning,
        /// <summary>The end token will be ignored</summary>
        WithoutEnd,
        /// <summary>The end token will be taken</summary>
        WithEnd,
        /// <summary>Linefeeds will be ignored</summary>
        WithoutLinefeed,
        /// <summary>Linefeeds will be processed</summary>
        WithLinefeed
    }


    public class StringProcessingException : Exception
    {
        public StringProcessingException() { }
        public StringProcessingException(string message) { }
    }


    
    
    /// <summary>
    /// Extension methods for strings
    /// Oliver Abraham, 7/2010, mail@oliver-abraham.de
    /// </summary>
    public static class StringExtensions
    {
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Löscht die ersten n character aus dem String
        /// </summary>
        /// <param name="input">Der String</param>
        /// <param name="count">Anzahl zu löschendr character</param>
        ///----------------------------------------------------------------------------------------
        public static void DeleteFirstNchars(ref string input, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("Der count darf nicht kleiner als 0 sein bei Funktion DeleteFirstNchars!");
            if (input.Length <= count)
                input = "";
            else
                input = input.Substring(count, input.Length - count);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Holt die ersten n character aus dem String und löscht sie auch dort
        /// </summary>
        /// <param name="input">input und output! String wird verkürzt!</param>
        /// <param name="count">Anzahl zu holendr character, oder soviel wie noch da ist</param>
        /// <returns>count character oder soviel wie noch da ist. </returns>
        ///----------------------------------------------------------------------------------------
        public static string GetAndRemoveFirstNchars(ref string input, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("Der count darf nicht kleiner als 0 sein bei Funktion GetAndRemoveFirstNchars!");
            if (input.Length <= count)
            {
                string output = input.Substring(0, input.Length);
                input = "";
                return output;
            }
            else
            {
                string output = input.Substring(0, count);
                DeleteFirstNchars(ref input, count);
                return output;
            }   
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Ersetzt in einem String ein Wort nur einmal, nicht wie String.Replace so oft es vorkommt.
        /// Gibt das Resultat zurück.
        /// </summary>
        /// <param name="input">String</param>
        /// <param name="searchterm">Zu suchend Stelle</param>
        /// <param name="replacewith">Reinzupackendr String</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string ReplaceOneTime(this string input, string searchterm, string replacewith)
        {
            int StartPosition = input.IndexOf(searchterm);
            if (StartPosition < 0)
                return input;

            string Vorher = "";
            if (StartPosition > 0)
                Vorher = input.Substring(0, StartPosition);
                
            string Nachher = input.Substring(StartPosition + searchterm.Length);

            return Vorher + replacewith + Nachher;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Entfernt allen Weissraum aus einem String (Leercharacter und Tabs)
        /// </summary>
        /// <param name="input">String</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string TrimWhitespaces(this string input)
        {
            return input.Trim('\t', ' ');
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Entfernt Leercharacter, Tabs und alle Zeilenvorschübe aus einem String.
        /// </summary>
        /// <param name="input">String</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string TrimWhitespacesAndNewlines(this string input)
        {
            return input.Trim('\r', '\n', '\t', ' ');
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Entfernt alle leeren Zeilen aus einem String. 
        /// Die Funktion arbeitet noch nicht in allen Situationen richtig.
        /// Zur Zeit entfernt sie nur eine Zeile, wenn davor und danach ein Zeilenvorschub steht.
        /// </summary>
        /// <param name="input">String</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string RemoveEmptyLines(this string input)
        {
            while (input.Contains("\r\n\r\n"))
                input = input.Replace("\r\n\r\n", "\r\n");
            return input;
        }


        
        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht den Zeilenbeg vor der übergebenen Position und gibt alles zwischen 
        /// dem Zeilenbeg und der übergebenen Position zurück.
        /// </summary>
        /// <param name="input">Mehrzeiliger String</param>
        /// <param name="pos">Position innerhalb der Zeile (nullbasierter Index)</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string FetchTextBetweenLineBeginningAndPos(this string input, int pos)
        {
            int end = pos;
            int beg = pos;

            if (beg > 0)
            {
                beg--;
                // Rückwärts gehen, bis wir den Zeilenbeg gefunden haben
                while (beg > 0 && input[beg] != '\r' && input[beg] != '\n')
                    beg--;
            }

            return input.Substring(beg, end - beg);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht ein bestimmtes Wort in einem String und gibt die gesamte Zeile zurück, 
        /// in der das Wort zum ersten Mal vorkommt.
        /// </summary>
        /// <param name="input">Mehrzeiliger String</param>
        /// <param name="searchterm">Stelle</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string FetchWholeLineContainingAToken(this string input, string searchterm)
        {
            int PosSuchwort = input.IndexOf(searchterm);
            if (PosSuchwort >= 0)
                return FetchWholeLineAtPosition(input, PosSuchwort);
            else
                return "";
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Gibt eine gesamte Zeile eines Strings zurück, in der der übergebene Index liegt.
        /// </summary>
        /// <param name="input">Mehrzeiliger String</param>
        /// <param name="pos">Nullbasierte Position innerhalb des Strings</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string FetchWholeLineAtPosition(this string input, int pos)
        {
            char[] Zeilentrenncharacter = new char[] { '\n', '\r' };

            // Rückwärts gehen und Zeilenbeg suchen
            int Zeilenbeg = pos;
            while (Zeilenbeg > 0 && 
                   input[Zeilenbeg - 1] != Zeilentrenncharacter[0] && 
                   input[Zeilenbeg - 1] != Zeilentrenncharacter[1] )
            {
                Zeilenbeg--;
            }

            // Jetzt Zeilenend suchen
            int Zeilenend = input.IndexOfAny(Zeilentrenncharacter, Zeilenbeg);
            if (Zeilenend > Zeilenbeg)
                return input.Substring(Zeilenbeg, Zeilenend - Zeilenbeg);
            else
                return "";
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und gibt den Text zwischen den Stellen zurück. (Ohne beg und end)
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string FetchTextBetweenTwoTokens( this string input,
                                                          string beg,
                                                          string end)
        {
            return FetchTextBetweenTwoTokens(input, beg, end, 
                                               TextFetchMode.WithoutBeginning,
                                               TextFetchMode.WithoutEnd);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und gibt den Text zwischen den Stellen zurück.
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="begMode">Flag das angibt, ob der begstext selbst auch zurückgegeben werden soll.</param>
        /// <param name="endMode">Flag das angibt, ob der endtext selbst auch zurückgegeben werden soll.</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string FetchTextBetweenTwoTokens(this string input, 
                                                       string beg, 
                                                       string end, 
                                                       TextFetchMode begMode, 
                                                       TextFetchMode endMode)
        {
            int Posbeg = input.IndexOf(beg);
            if (Posbeg == -1)
                return "";

            int Posend = input.IndexOf(end, Posbeg + beg.Length + 1);
            if (Posend == -1)
                Posend = input.Length - 1;

            if (begMode == TextFetchMode.WithoutBeginning)
                Posbeg += beg.Length;

            if (endMode == TextFetchMode.WithEnd)
                Posend += end.Length;

            return input.Substring(Posbeg, Posend - Posbeg);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und löscht alles dazwischen. (ohne nachfolgendn Zeilenvorschub)
        /// </summary>
        /// <param name="content">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <returns>True, wenn was entfernt wurde (um eine while-Schleife zu basteln)</returns>
        ///----------------------------------------------------------------------------------------
        public static string RemoveTextBetweenTwoTokens( this string content,
                                                              string beg,
                                                              string end)
        {
            return RemoveTextBetweenTwoTokens(content, beg, end, TextFetchMode.WithoutLinefeed);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und löscht alles dazwischen.
        /// </summary>
        /// <param name="content">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="lineEndMode">Flag das angibt, ob der nachfolgend Zeilenvorschub auch entfernt werden soll.</param>
        /// <returns>True, wenn was entfernt wurde (um eine while-Schleife zu basteln)</returns>
        ///----------------------------------------------------------------------------------------
        public static string RemoveTextBetweenTwoTokens( this string content,
                                                              string beg,
                                                              string end,
                                                              TextFetchMode lineEndMode)   // MitZeilenvorschub, OhneZeilenvorschub
        {
            bool dummy;
            return RemoveTextBetweenTwoTokens(content, beg, end, lineEndMode, out dummy);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und löscht alles dazwischen.
        /// </summary>
        /// <param name="content">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="lineEndMode">Flag das angibt, ob der nachfolgend Zeilenvorschub auch entfernt werden soll.</param>
        /// <param name="wasReplaced">True, wenn was entfernt wurde (um eine while-Schleife zu basteln)</param>
        /// <returns>Neuer String (Teil von "content")</returns>
        ///----------------------------------------------------------------------------------------
        public static string RemoveTextBetweenTwoTokens( this string content,
                                                              string beg,
                                                              string end,
                                                              TextFetchMode lineEndMode,   // MitZeilenvorschub, OhneZeilenvorschub
                                                              out bool wasReplaced)
        {
            return content.ReplaceTextBetweenTwoTokens(beg, end, "", 
                                                      TextFetchMode.WithoutBeginning, TextFetchMode.WithoutEnd, 
                                                      out wasReplaced);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und ersetzt den Text zwischen den Stellen.
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="newContent">Text, der dort eingesetzt wird.</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string ReplaceTextBetweenTwoTokens( this string input,
                                                             string beg,
                                                             string end,
                                                             string newContent)
        {
            return ReplaceTextBetweenTwoTokens (input, beg, end, newContent,
                                                   TextFetchMode.WithoutBeginning,
                                                   TextFetchMode.WithoutEnd);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und ersetzt den Text zwischen den Stellen.
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="newContent">Text, der dort eingesetzt wird.</param>
        /// <param name="begMode">Flag das angibt, ob der begstext selbst auch zurückgegeben werden soll.</param>
        /// <param name="endMode">Flag das angibt, ob der endtext selbst auch zurückgegeben werden soll.</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string ReplaceTextBetweenTwoTokens(this string input,
                                                             string beg,
                                                             string end,
                                                             string newContent,
                                                             TextFetchMode begMode,
                                                             TextFetchMode endMode)
        {
            bool haveReplaced;
            return input.ReplaceTextBetweenTwoTokens (beg, end, newContent, begMode, endMode,
                                                            out haveReplaced);
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und ersetzt den Text zwischen den Stellen.
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="newContent">Text, der dort eingesetzt wird.</param>
        /// <param name="begMode">Flag das angibt, ob der begstext selbst auch zurückgegeben werden soll.</param>
        /// <param name="endMode">Flag das angibt, ob der endtext selbst auch zurückgegeben werden soll.</param>
        /// <param name="haveReplaced">Rückgabe true, wenn eine Ersetzung erfolgte</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string ReplaceTextBetweenTwoTokens(this string input,
                                                             string beg,
                                                             string end,
                                                             string newContent,
                                                             TextFetchMode begMode,
                                                             TextFetchMode endMode,
                                                             out bool haveReplaced)
        {
            int dummyStartPosition = 0;
            return input.ReplaceTextBetweenTwoTokens(beg,
                                                          end,
                                                          newContent,
                                                          begMode,
                                                          endMode,
                                                          out haveReplaced,
                                                          ref dummyStartPosition);
        }


        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und ersetzt den Text zwischen den Stellen.
        /// </summary>
        /// <param name="input">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="newContent">Text, der dort eingesetzt wird.</param>
        /// <param name="begMode">Flag das angibt, ob der begstext selbst auch zurückgegeben werden soll.</param>
        /// <param name="endMode">Flag das angibt, ob der endtext selbst auch zurückgegeben werden soll.</param>
        /// <param name="haveReplaced">Rückgabe true, wenn eine Ersetzung erfolgte</param>
        /// <param name="startPosition">Gibt an, wo mit der Suche begonnen werden soll, falls beg oder end mehrfach vorkommen.</param>
        /// <returns>Teilstring aus "input"</returns>
        ///----------------------------------------------------------------------------------------
        public static string ReplaceTextBetweenTwoTokens( this string input,
                                                             string beg,
                                                             string end,
                                                             string newContent,
                                                             TextFetchMode begMode,
                                                             TextFetchMode endMode,
                                                             out bool haveReplaced,
                                                             ref int startPosition)
        {
            string @new = input;

            int Posbeg = @new.IndexOf(beg, startPosition);
            if (Posbeg == -1)
            {
                haveReplaced = false;
                return input;
            }

            int Posend = @new.IndexOf(end, Posbeg + beg.Length + 1);
            if (Posend == -1)
            {
                haveReplaced = false;
                return input;
            }

            if (begMode == TextFetchMode.WithoutBeginning)
                Posbeg += beg.Length;

            if (endMode == TextFetchMode.WithEnd)
                Posend += end.Length;

            if (Posend > Posbeg)
                @new = @new.Remove(Posbeg, Posend - Posbeg);
            @new = @new.Insert(Posbeg, newContent);

            haveReplaced = (@new != input);

            // Startposition für den nächsten Schleifendurchlauf berechnen
            int AnzahlEntferntecharacter = Posend - Posbeg;
            int AnzahlEingefügtecharacter = newContent.Length;
            startPosition = Posend + end.Length - AnzahlEntferntecharacter + AnzahlEingefügtecharacter;

            return @new;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht die beiden übergebenen Stellen "beg" und "end" im übergebenen String
        /// und löscht alles dazwischen.
        /// </summary>
        /// <param name="content">Mehrzeiliger oder einzeiliger String</param>
        /// <param name="beg">Zu suchendr String, der den beg markiert</param>
        /// <param name="end">Zu suchendr String, der das end markiert</param>
        /// <param name="lineEndMode">Flag das angibt, ob der nachfolgend Zeilenvorschub auch entfernt werden soll.</param>
        /// <returns>Neuer String (Teil von "content)</returns>
        ///----------------------------------------------------------------------------------------
        public static string RemoveTextBetweenTwoTokensAllPlaces(this string content, string beg, string end, TextFetchMode lineEndMode)
        {
            // Wiederholt allen Code aus der Datei entfernen
            string newContent = "";
            int startPosition = 0;
            bool haveReplaced = true;
            while (haveReplaced)
            {
                content = content.ReplaceTextBetweenTwoTokens(beg, end, newContent,
                                                                TextFetchMode.WithoutBeginning,
                                                                lineEndMode,
                                                                out haveReplaced,
                                                                ref startPosition);
            }
            return content;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Generiert einen String, der das übergebenen character n Mal enthält
        /// </summary>
        /// <param name="content">Unbenutzt</param>
        /// <param name="character">Das character, das genommen werden soll</param>
        /// <param name="count">So lang wird der String</param>
        /// <returns>Veränderter String</returns>
        ///----------------------------------------------------------------------------------------
        public static string GenerateCharacters(this string content, char character, int count)
        {
            string Rückgabe = "";
            for (int i = 0; i < count; i++)
                Rückgabe += character;
            return Rückgabe;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Füllt den String am beg mit einem character auf.
        /// </summary>
        /// <param name="content">Objekt</param>
        /// <param name="character">Das einzufügend character</param>
        /// <param name="length">Wenn das Objekt kürzer als diese length ist, wird aufgefüllt. 
        /// Ansonsten bleibt das Objekt unverändert.</param>
        /// <returns>verändertes Objekt (eventuell lengthr als vorher)</returns>
        ///----------------------------------------------------------------------------------------
        public static string PadLeft(this string content, char character, int length)
        {
            if (content.Length < length)
                return content.GenerateCharacters(character, length - content.Length) + content;
            else
                return content;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Füllt den String am Emde mit einem character auf.
        /// </summary>
        /// <param name="content">Objekt</param>
        /// <param name="character">Das einzufügend character</param>
        /// <param name="length">Wenn das Objekt kürzer als diese length ist, wird aufgefüllt. 
        /// Ansonsten bleibt das Objekt unverändert.</param>
        /// <returns>verändertes Objekt (eventuell lengthr als vorher)</returns>
        ///----------------------------------------------------------------------------------------
        public static string PadRight(this string content, char character, int length)
        {
            if (content.Length < length)
                return content + content.GenerateCharacters(character, length - content.Length);
            else
                return content;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Sucht in einem einzeiligen String ein Wort und rückt es auf die angegebene 
        /// Spaltenposition ein, falls es zu weit links steht.
        /// </summary>
        /// <param name="content">input</param>
        /// <param name="searchTerm">Hiernach wird im String gesucht</param>
        /// <param name="columnPosition">Zu dieser Spaltenposition wird das Wort eingerückt</param>
        /// <returns>Gibt den erweiterten String zurück</returns>
        ///----------------------------------------------------------------------------------------
        public static string Indent(this string content, string searchTerm, int columnPosition)
        {
            if (columnPosition < 0)
                throw new StringProcessingException ("Die Spaltenposition darf nicht negativ sein!");

            string Rückgabe = content;
            int Pos = content.IndexOf(searchTerm);
            if (Pos >= 0 && Pos < columnPosition)
                Rückgabe = content.Insert(Pos, content.GenerateCharacters(' ', columnPosition - Pos));
            return Rückgabe;
        }



        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Zerlegt einen String in eine Liste von Strings fester length. Das letzte Element kann kürzer sein.
        /// Falls der übergebene String lengthr als die übergebene Maximallength ist, 
        /// gibt sie mehrere Elemente zurück, ansonsten nur ein Element.
        /// </summary>
        /// 
        /// <param name="input">Quellstring</param>
        /// <param name="maximumLength">Falls der Quellstring lengthr als diese Zahl ist, 
        /// wird er in Teile mit dieser length geschnitten. Der Rest konnt ins letzte Listenelement.
        /// </param>
        /// 
        /// <returns>
        /// Gibt die Liste von Strings zurück. 
        /// Wenn der Übergebene String leer ist, wird eine leere Liste zurückgegeben.
        /// </returns>
        ///----------------------------------------------------------------------------------------
        public static List<string> SplitStringIfLongerThan(this string input, int maximumLength)
        {
            if (maximumLength <= 0)
                throw new ArgumentOutOfRangeException("Die Maximallength muss mindestens 1 sein!");

            List<string> parts = new List<string>();

            while (input.Length > maximumLength)
            {
                parts.Add(input.Substring(0, maximumLength));
                input = input.Substring(maximumLength);
            }
            if (input.Length > 0)
                parts.Add(input);

            return parts;
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        /// Setzt einen String in beliebige Anführungsstriche, mit oder ohne Character Stuffing
        /// </summary>
        /// <param name="input">Beliebiger String.</param>
        /// <param name="quoteStringBeg">Begrenzungscharacter</param>
        /// <param name="quoteStringEnd">Begrenzungscharacter</param>
        /// <param name="withCharacterStuffing">
        /// true beduetet, dass vor alle Vorkommen 
        /// des Begrenzungscharacters im String der stuffingCharacter eingefügt wird</param>
        /// 
        /// <param name="stuffingCharacter">
        /// character, das vor alle Vorkommen des 
        /// Begrenzungscharacters eingefügt wird</param>
        /// 
        /// <returns>String eingeschlossen in Anführungsstriche. 
        /// Im String vorhandene character werden sie NICHT verdoppelt.</returns>
        ///----------------------------------------------------------------------------------------
        public static string InQuotes (this string input,
                                       string quoteStringBeg = "'",
                                       string quoteStringEnd = "",
                                       bool withCharacterStuffing = false,
                                       string stuffingCharacter = "\\")
        {
            if (quoteStringEnd.Length == 0)
                quoteStringEnd = quoteStringBeg;

            string temp;
            if (withCharacterStuffing == true)
            {
                temp = input.Replace(quoteStringBeg, stuffingCharacter + quoteStringBeg);
                if (!quoteStringBeg.Equals(quoteStringEnd))
                    temp = temp.Replace(quoteStringEnd, stuffingCharacter + quoteStringEnd);
            }
            else
                temp = input;

            return quoteStringBeg + temp + quoteStringEnd;
        }
    }
}
