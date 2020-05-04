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

            //haal jou module uit commentaar!!
            ZaalModule zaalbeheer = new ZaalModule();
            FilmModule filmbeheer = new FilmModule();
            GebruikerModule gebruikerbeheer = new GebruikerModule();
            FilmschemaModule filmschemabeheer = new FilmschemaModule();
            StoelModule stoel = new StoelModule();

            bool loop = true;
            while (loop)
            {
                switch (menu.Run())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        zaalbeheer.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        filmbeheer.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        gebruikerbeheer.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        filmschemabeheer.Run();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        stoel.Run();
                        break;
                }
            }
        }
    }
}

