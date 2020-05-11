using System;
using Bioscoop.Modules;

namespace Bioscoop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(155, 32);
            MenuModule menu = new MenuModule();
            bool loop = true;

            while (loop)
            {
                switch (menu.Run())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        //filmoverzicht
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        //beherenreservatieklant
                        break;
                    case ConsoleKey.Insert:
                        Console.Clear();
                        //login
                        menu.MenuAdmin();
                        break;
                }
            }
        }
    }
}

