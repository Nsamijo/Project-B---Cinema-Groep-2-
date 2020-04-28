using System;
using Bioscoop.Modules;

namespace Bioscoop
{
    class Program
    {
        static void Main(string[] args)
        {
            //opvragen van jou module hoofd class
            MenuModule menu = new MenuModule(); 


            ZaalModule zaal = new ZaalModule();
            //FilmModule film = new FilmModule();
            //GebruikerModule gebruiker = new FilmModule();
            //FilmschemaModule schema = new FilmschemaModule();
            //StoelModule stoel = new StoelModule();

            bool loop = true;
            while (loop)
            {
                switch (menu.Run())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        zaal.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        //film.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        //gebruiker.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        //filmschema.Run();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        //stoel.Run();
                        break;
                }
            }
        }
    }
}

