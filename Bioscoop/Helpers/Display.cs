using System;
using System.Collections.Generic;
using System.Text;

namespace Bioscoop.Helpers
{
    class Display //consolewriteline vervanger met tabellen
    {
        
        static readonly string prefix = " ";
        
        public static ConsoleKey Keypress() //keypress lees functie voor hoofdmenu
        {
            return Console.ReadKey(true).Key;
        }

        // in plaats van console.writeline
        public static void PrintLine(string line)
        {
            Console.WriteLine(prefix +  line);
        }

        //tabbellen data
        public static void PrintHeader(string col1, string col2 = null, string col3 = null, string col4 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   "                : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                  " : " "+ col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "                  " : " "+ col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "                  " : " "+ col4.PadRight(20)));

            Console.WriteLine(_sb.ToString());

/*            string tabs = new String('-', 80);
            Console.WriteLine(tabs);*/
        }

        public static void PrintTable(string col1, string col2 = null, string col3 = null, string col4 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   "                : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                  " : " "+ col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "                  " : " "+ col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "                  " : " "+ col4.PadRight(20)));

            Console.WriteLine(_sb.ToString());
        }

    }
}
