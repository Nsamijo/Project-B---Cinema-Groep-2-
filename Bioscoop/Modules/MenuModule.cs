using System;
using Bioscoop.Modules;
using Bioscoop.Repository;

namespace Bioscoop.Modules
{
    class MenuModule
    {
        public ConsoleKey Run()
        {
            MenuMain();
            return Helpers.Display.Keypress();
        }

        private void MenuMain()
        {
            //main menu
            Console.Clear(); Console.CursorVisible = false;
            Helpers.Display.PrintLine("Hoofdmenu                                                INS - Login \n");
            Helpers.Display.PrintLine("Welkom op het klanten portaal van de Bioscoop \n");
            Helpers.Display.PrintLine("Navigeer naar het filmoverzicht om alle film informatie in te zien en een film te reserveren");
            Helpers.Display.PrintLine("Als je een reservatie hebt aangemaakt kan je deze inzien bij beheren reservatie \n");
            Helpers.Display.PrintHeader("Nr.", "Menu");
            Helpers.Display.PrintTable("1", "Filmoverzicht");
            Helpers.Display.PrintTable("2", "Beheren reservatie");
        }
        public void MenuAdmin()
        {
            LoginModule admin = new LoginModule();
            while (admin.NuIngelogd() == null)
            {
                admin.Login(new Lezer().gebruikersInlezen());
            }

            if (admin.NuIngelogd().Naam.Equals("cancel"))
                return;

            bool loop = true;
            while (loop)
            {
                Console.Clear(); Console.CursorVisible = false;
                Helpers.Display.PrintLine("Bioscoop - Admin Portaal                            Welkom: " + admin.NuIngelogd().Naam);
                Helpers.Display.PrintLine("ESC - Uitloggen                                     INS - Medewerkers portaal");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintHeader("Nr.", "Menu");
                Helpers.Display.PrintTable("1", "Filmbeheer");
                Helpers.Display.PrintTable("2", "Filmschemabeheer");
                Helpers.Display.PrintTable("3", "Zaalbeheer");
                Helpers.Display.PrintTable("4", "Stoelenbeheer");
                Helpers.Display.PrintTable("5", "Gebruikerbeheer");

                ZaalModule zaalbeheer = new ZaalModule();
                FilmModule filmbeheer = new FilmModule();
                GebruikerModule gebruikerbeheer = new GebruikerModule();
                FilmschemaModule filmschemabeheer = new FilmschemaModule();
                StoelModule stoelbeheer = new StoelModule();

                switch (Helpers.Display.Keypress())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        filmbeheer.Run();
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        filmschemabeheer.Run();
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        zaalbeheer.Run();
                        break;
                    case ConsoleKey.D4:
                        Console.Clear();
                        stoelbeheer.Run();
                        break;
                    case ConsoleKey.D5:
                        Console.Clear();
                        gebruikerbeheer.Run();
                        break;
                    case ConsoleKey.Insert:
                        Console.Clear();
                        MenuMedewerker();
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        loop = false;
                        break;
                }
            }
        }
        private void MenuMedewerker()
        {
            bool loop = true;
            while (loop)
            {
                Console.Clear();
                Helpers.Display.PrintLine("Bioscoop - Medewerkers Portaal                       Welkom:");
                Helpers.Display.PrintLine("ESC - Uitloggen");
                Helpers.Display.PrintLine("");
                Helpers.Display.PrintHeader("Nr.", "Menu");
                Helpers.Display.PrintTable("1", "Reservatie beheer");
                Helpers.Display.PrintTable("2", "Reservatie Rapportage");
                Helpers.Display.PrintTable("3", "Film Rapportage");

                switch (Helpers.Display.Keypress())
                {
                    case ConsoleKey.D1:
                        Console.Clear();
                        //reservatie module
                        break;
                    case ConsoleKey.D2:
                        Console.Clear();
                        //rapportage reservaties
                        break;
                    case ConsoleKey.D3:
                        Console.Clear();
                        //rapportage films
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        loop = false;
                        break;
                }
            }
        }
    }
}
