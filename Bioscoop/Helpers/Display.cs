using System;
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
        public static void PrintHeader(string col1, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null, string col7 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   "                : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                    " : " "+ col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "                    " : " "+ col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "                    " : " "+ col4.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "                    " : " "+ col5.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "                    " : " "+ col6.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col7) ? "                    " : " "+ col7.PadRight(20)));

            Console.WriteLine(_sb.ToString());
        }

        public static void PrintTable(string col1, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null, string col7 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   "                  : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                    " : " "+ col2.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "                    " : " "+ col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "                    " : " "+ col4.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "                    " : " "+ col5.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "                    " : " "+ col6.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col7) ? "                    " : " "+ col7.PadRight(20)));

            Console.WriteLine(_sb.ToString());
        }
        public static void PrintTableInfo(string col1, string col2 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "                    " : " " + col1.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                                            " : " " + col2.PadRight(45)));

            Console.WriteLine(_sb.ToString());
        }
        public static void PrintTableFilm(string col1, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   " : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                                            " : " " + col2.PadRight(45)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "                    " : " " + col3.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "                    " : " " + col4.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "                    " : " " + col5.PadRight(20)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "                    " : " " + col6.PadRight(20)));

            Console.WriteLine(_sb.ToString());
        }
        public static void PrintTableRapportage(string col1, string col2 = null, string col3 = null, string col4 = null, string col5 = null, string col6 = null, string col7 = null, string col8 = null)
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Append((String.IsNullOrEmpty(col1) ? "   " : prefix + col1.PadRight(3)));
            _sb.Append((String.IsNullOrEmpty(col2) ? "                                          " : " " + col2.PadRight(45)));
            _sb.Append((String.IsNullOrEmpty(col3) ? "               " : " " + col3.PadRight(15)));
            _sb.Append((String.IsNullOrEmpty(col4) ? "               " : " " + col4.PadRight(10)));
            _sb.Append((String.IsNullOrEmpty(col5) ? "                                          " : " " + col5.PadRight(45)));
            _sb.Append((String.IsNullOrEmpty(col6) ? "               " : " " + col6.PadRight(15)));

            Console.WriteLine(_sb.ToString());
        }
    }
}
