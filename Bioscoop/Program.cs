using System;
using System.Globalization;
using Bioscoop.Modules;

namespace Bioscoop
{
    class Program
    {
        public static void Main(string[] args)
        {
            //globale data
            Console.SetWindowSize(160, 35);
            var culture = new CultureInfo("nl-NL");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

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