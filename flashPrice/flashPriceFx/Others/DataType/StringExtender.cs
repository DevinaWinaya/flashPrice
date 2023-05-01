using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace System
{
    public static partial class StringExtender
    {

        public static string UpperCase(this String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.ToUpper();            
        }

        public static string UpperCaseFirst(this String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            char[] a = str.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public static string UpperCaseWords(this String value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        static string[] SplitWords(string s)
        {
            //
            // Split on all non-word characters.
            // ... Returns an array of all the words.
            //
            return Regex.Split(s, @"\W+");
            // @      special verbatim string syntax
            // \W+    one or more non-word characters together
        }

        public static List<String> Wrap(string text, int maxLength)
        {
            // Return empty list of strings if the text was empty
            if (text.Length == 0) return new List<string>();

            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = "";

            foreach (var currentWord in words)
            {

                if ((currentLine.Length > maxLength) ||
                ((currentLine.Length + currentWord.Length) > maxLength))
                {
                    lines.Add(currentLine);
                    currentLine = "";
                }

                if (currentLine.Length > 0)
                    currentLine += " " + currentWord;
                else
                    currentLine += currentWord;

            }

            if (currentLine.Length > 0)
                lines.Add(currentLine);

            return lines;
        }



        /// <summary>
        /// singkat kata2. misal STORE ADM. SUPERVISOR akan return SAS
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string abreviateWords(this String value)
        {
            string xx = "";
            string[] names = SplitWords(value.Trim());
            for (int x = 0; x < names.Length; x++)
            {
                xx += names[x].Left(1);
            }
            return xx;
        }

        /// <summary>
        /// get left portion of a string        
        /// </summary>
        /// <param name="str">string to read.</param>
        /// <param name="length">length to get.</param>
        public static string Left(this String str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }

        /// <summary>
        /// get right portion of a string        
        /// </summary>
        /// <param name="str">string to read.</param>
        /// <param name="length">length to get.</param>
        public static string Right(this String str, int length)
        {
            return str.Substring(length > str.Length ? 0 : str.Length - length);
        }

        /// <summary>
        /// get middle portion of a string        
        /// </summary>
        /// <param name="str">string to read.</param>
        /// <param name="startPos">start position.</param>
        /// /// <param name="length">length to get.</param>
        public static string Mid(this String str, int startPos, int length)
        {
            String temp = str.Substring(startPos - 1, length);
            return temp;
        }

        /// <summary>
        /// reverse of a string
        /// </summary>
        /// <param name="str">string to be reverse.</param>        
        public static string ReverseString(this String str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        /// <summary>
        /// replace an empty string with parameter
        /// </summary>
        /// <param name="str">string to be reverse.</param>        
        public static string ReplaceEmpty(this String str, string replacement)
        {
            if (str == "")
            {
                return replacement;
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// Returns new string with the found search expression replaced with replacement string
        /// </summary>
        /// <param name="source">The string to do the replacement</param>
        /// <param name="search">The sequence of characters to find</param>
        /// <param name="replacement">The replacement string to replace the sequence of characters</param>
        /// <returns>New string with replaced sequence of characters</returns>
        public static string ReplaceIgnoreCase(this string source, string search, string replacement)
        {
            return Regex.Replace(source, Regex.Escape(search), replacement.Replace("$", "$$"), RegexOptions.IgnoreCase);
        }

        public static StringBuilder ReplaceIgnoreCase(this StringBuilder source, string search, string replacement)
        {
            var temp = source.ToString();
            return new StringBuilder(temp.ReplaceIgnoreCase(search, replacement));
        }

        /// <summary>
        /// Checks whether the string is equal to other string ignoring the character case
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, string other)
        {
            if(source == null)
                throw new ArgumentNullException("source");
            //return other != null && source.EqualsIgnoreCaseHelper(other);
            return other != null && source.Equals(other, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Checks whether the string is equal to other object ignoring the character case.
        /// The other object must be a <see cref="string"/>, otherwise will be false
        /// </summary>
        /// <param name="source"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string source, object other)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            var otherStr = other as string;
            //return otherStr != null && source.EqualsIgnoreCaseHelper(otherStr);
            return otherStr != null && string.Equals(source, otherStr, StringComparison.OrdinalIgnoreCase);
        }

        private static bool EqualsIgnoreCaseHelper(this string source, string other)
        {
            return source.Length == other.Length && source.ToLower().Equals(other.ToLower());
        }

        /// <summary>
        /// Encodes the <see cref="string"/> to HTML to show proper line breaks
        /// </summary>
        /// <param name="source">The source string to process</param>
        /// <returns>Newly encoded Html string</returns>
        public static string EncodeToHtml(this string source)
        {
            return Regex.Replace(source, "\\r?\\n", "<br />");
            //return new StringBuilder(source).Replace(Environment.NewLine, "<br />").ToString();
        }

        /// <summary>
        /// Performs parse of dd-MM-yyyy date format 
        /// </summary>
        /// <param name="dateStr">The string contains date to be parsed</param>
        /// <returns>A new DateTime object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static DateTime ParseIndoDate(this string dateStr)
        {
            return DateTime.ParseExact(dateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        /// <summary>
        /// Performs parse of dd-MM-yyyy HH:mm date Formaat
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseIndoDateTime(this string dateStr)
        {
            return DateTime.ParseExact(dateStr, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
        /// <summary>
        /// Performs parse of dd-MM-yyyy HH:mm:ss date Formaat
        /// </summary>
        /// <param name="dateStr"></param>
        /// <returns></returns>
        public static DateTime ParseIndoDateTimeComplete(this string dateStr)
        {
            return DateTime.ParseExact(dateStr, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        }


        /// <summary>
        /// Tries to parse string of dd-MM-yyyy HH:mm:ss date Format
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool TryParseIndoDateTimeComplete(this string dateStr, out DateTime dateTime)
        {
            return DateTime.TryParseExact(dateStr, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dateTime);
        }


        /// <summary>
        /// Tries to parse string of dd-MM-yyyy HH:mm date Format
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool TryParseIndoDateTime(this string dateStr, out DateTime dateTime)
        {
            return DateTime.TryParseExact(dateStr, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dateTime);
        }

        /// <summary>
        /// Tries to parse string of dd-MM-yyyy date format 
        /// </summary>
        /// <param name="dateStr">The string contains date to be parsed</param>
        /// <returns>A new DateTime object</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static bool TryParseIndoDate(this string dateStr, out DateTime dateTime)
        {
            return DateTime.TryParseExact(dateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out dateTime);
        }

        public static bool ContainsSpace(this string source)
        {
            if(source == null) throw new ArgumentNullException("source");

            return source.All(c => !char.IsWhiteSpace(c));
        }

        public static string NullIfEmpty(this string source)
        {
            return string.IsNullOrEmpty(source) ? null : source;
        }

        public static string NullIfWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source) ? null : source;
        }

        public static string EmptyIsNull(this string source)
        {
            return string.IsNullOrEmpty(source) ? "" : source;
        }


        public static string FromTo(this string source, int from, int to)
        {
            if(source == null) throw new ArgumentNullException("source");
            if (from < 0 || to < 0 || from > source.Length - 1 || to > source.Length - 1) throw new ArgumentOutOfRangeException();
            if(from > to)
                throw new ArgumentOutOfRangeException("from", "Start search index is higher than to position");

            return source.Substring(from, to - from + 1);
        }

        public static IEnumerable<string> SplitTrimIgnoreWhitespace(this string source, params char[] splitChar)
        {
            var splitter = new Regex(@"[" + Regex.Escape(new string(splitChar)) + @"]");

            var query =
                from s in splitter.Split(source)
                where !string.IsNullOrWhiteSpace(s)
                select s.Trim();

            return query;
        }
    }
}
