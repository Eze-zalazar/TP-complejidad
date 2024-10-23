using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace tpfinal
{
    internal class Utils
    {
        private static string patron;
        public static int lineCount=190;

        public static void init_patron()
        {
            /*patron = System.IO.Directory.GetCurrentDirectory() + "\\datasets\\dataset.csv";
            lineCount = File.ReadLines(@patron).Count();*/
        }

        public static void set_patron(string patron_parm)
        {
            patron = patron_parm;
            lineCount = File.ReadLines(@patron).Count();
        }
        public static string get_patron()
        {
            /*if (patron == null)
            {
                patron = System.IO.Directory.GetCurrentDirectory() + "\\datasets\\dataset.csv";
                lineCount = File.ReadLines(@patron).Count();

            }*/
            
            return patron;  
        }



        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            //string str = str_input.Replace(' ', '_');
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        
        public static IEnumerable<IList<string>> Parse(TextReader reader, char delimiter, char qualifier)
		{
			var inQuote = false;
			var record = new List<string>();
			var sb = new StringBuilder();

			while (reader.Peek() != -1) {
				var readChar = (char)reader.Read();

				if (readChar == '\n' || (readChar == '\r' && (char)reader.Peek() == '\n')) {
					// If it's a \r\n combo consume the \n part and throw it away.
					if (readChar == '\r')
						reader.Read();

					if (inQuote) {
						if (readChar == '\r')
							sb.Append('\r');
						sb.Append('\n');
					} else {
						if (record.Count > 0 || sb.Length > 0) {
							record.Add(sb.ToString());
							sb.Clear();
						}

						if (record.Count > 0)
							yield return record;

						record = new List<string>(record.Count);
					}
				} else if (sb.Length == 0 && !inQuote) {
					if (readChar == qualifier)
						inQuote = true;
					else if (readChar == delimiter) {
						record.Add(sb.ToString());
						sb.Clear();
					} else if (char.IsWhiteSpace(readChar)) {
						// Ignore leading whitespace
					} else
						sb.Append(readChar);
				} else if (readChar == delimiter) {
					if (inQuote)
						sb.Append(delimiter);
					else {
						record.Add(sb.ToString());
						sb.Clear();
					}
				} else if (readChar == qualifier) {
					if (inQuote) {
						if ((char)reader.Peek() == qualifier) {
							reader.Read();
							sb.Append(qualifier);
						} else
							inQuote = false;
					} else
						sb.Append(readChar);
				} else
					sb.Append(readChar);
			}

			if (record.Count > 0 || sb.Length > 0)
				record.Add(sb.ToString());

			if (record.Count > 0)
				yield return record;
		}
       
    }
}
