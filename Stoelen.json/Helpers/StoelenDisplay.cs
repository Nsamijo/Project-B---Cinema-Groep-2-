using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    class StoelenDisplay
    {
        static readonly string prefix = " ";

        public static ConsoleKey Keypress()
        {
            return Console.ReadKey(true).Key;
        }

        public static void PrintLine(string line)
        {
            Console.WriteLine(prefix + line);
        }

        //tabbellen data
        public static void PrintHeader(string col1, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "            " : prefix + col1.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "            " : " " + col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "            " : " " + col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "            " : " " + col4.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "            " : " " + col5.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "            " : " " + col6.PadRight(20)));

            Console.WriteLine(_sb.ToString());

        }

        public static void PrintTable(string col1 = null, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "            " : prefix + col1.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "            " : " " + col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "            " : " " + col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "            " : " " + col4.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "            " : " " + col5.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "            " : " " + col6.PadRight(20)));

            Console.WriteLine(_sb.ToString());
        }



    }
}
