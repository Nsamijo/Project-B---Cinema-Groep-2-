using System;
using Bioscoop.Modules;

namespace Bioscoop
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.SetWindowSize(160, 35);
            MenuModule menu = new MenuModule();
            ReserveringModule reservatie = new ReserveringModule();
            bool loop = true;

            while (loop)
            {
                switch (menu.Run())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        reservatie.SelectFilm();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        reservatie.ReserveringLogin();
                        break;
                    case ConsoleKey.Insert:
                        Console.Clear();
                        menu.MenuAdmin();
                        break;
                }
            }
        }
    }
}